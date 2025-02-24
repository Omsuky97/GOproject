using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enumy_Monster : MonoBehaviour
{
    public static Enumy_Monster Instance;
    //���
    private List<Collider> Hit_Boomerang_Bullet = new List<Collider>(); // �̹� ���� �� ���
    Rigidbody monster_rigid;
    Animator anim;
    public Rigidbody targe_rigid;
    private float hit_damage;
    public Transform result;

    [Header("## -- Monster_Statas_List -- ##")]
    public int Moster_Id;
    public float Monster_MaxHp;
    public float Monster_Hp;
    public float Monster_Atk;
    public float Monster_Def;
    public float Monster_Goid;
    public int Monster_AtkType;
    public int Monster_DrItem;
    public string Monster_Prefabs;
    public float Monster_MoveSpeed;

    [Header("## -- Monster_Attack -- ##")]
    //���� ����
    public float scanRange;
    public RaycastHit[] target_player;
    public LayerMask targetLayer;
    public Transform nearestTarget;

    //�� ���ǹ� ���߿� ���������� �ۼ�
    public bool isLive = true;
    public bool monster_run;
    public bool monster_attack = false;

    [Header("## -- Monster_Hit -- ##")]
    public GameObject Hit_effect_prefab; // �¾��� �� ������ ��ƼŬ ������
    WaitForFixedUpdate wait;
    public bool Enemy_hit_Type;
    public GameObject hit_damage_text_pro;
    public string hit_damage_text_pos_name;
    public float Enemy_Hiy_Time;
    public float Hit_Delta_Time;
    public float True_Hit_Time = 2.0f;
    public int Hit_Sound;

    private void Awake()
    {
        Instance = this;
        monster_rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        wait = new WaitForFixedUpdate();
    }
    private void FixedUpdate()
    {
        if (!isLive) return;
        Target_Move_Rotator();

        target_player = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, scanRange, targetLayer);
        nearestTarget = GetNearest();

    }
    void Target_Move_Rotator()
    {
        Hit_Delta_Time += Time.deltaTime;
        if (!isLive ||  Enemy_hit_Type) return; //anim.GetCurrentAnimatorStateInfo(0).IsName("monIdol")
        // �������� ���� ��� (Y�� ����)
        Vector3 dirvec = targe_rigid.position - monster_rigid.position;
        dirvec.y = -3f; // ���� ���� ����
        // �̵� ���� ���
        Vector3 nextVec = dirvec.normalized * Monster_MoveSpeed * Time.fixedDeltaTime;
        // ���͸� ��� �������� ȸ��
        if (dirvec != Vector3.zero)
        {
            dirvec.y = 0f; // ���� ���� ����
            // ��� �������� ȸ�� ���
            Quaternion targetRotation = Quaternion.LookRotation(dirvec);

            // �ε巴�� ȸ�� (Y�ุ ȸ��)
            monster_rigid.rotation = Quaternion.Slerp(
                monster_rigid.rotation,
                targetRotation,
                Time.fixedDeltaTime * 10f
            );
            monster_run = true;
        }
        anim.SetBool("Monster_Run", monster_run);
        anim.SetBool("monster_attack", monster_attack);
        monster_rigid.MovePosition(monster_rigid.position + nextVec);
        monster_rigid.velocity = Vector3.zero;
    }
    private void OnEnable()
    {
        gameObject.tag = "Enemy";
        isLive = true;
        monster_run = true;
        monster_attack = false;
        Enemy_hit_Type = false;
        Monster_Hp = Monster_MaxHp;
        targe_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();
    }
    public void Init(MonsterData data)
    {
        // CSV �����ͷκ��� ������ ���� �ʱ�ȭ
        Monster_MoveSpeed = data.MonsterMoveSpeed;
        Monster_MaxHp = data.MonsterMaxHp;
        Monster_Atk = data.MonsterAtk;
        Monster_Def = data.MonsterDef;
        Monster_Goid = data.MonsterGoid;
        Monster_AtkType = data.MonsterAtkType;
        Monster_DrItem = data.MonsterDrItem;
        Monster_Prefabs = data.MonsterPrefabs;

        Monster_Hp = Monster_MaxHp;

        // ��Ÿ �ʱ�ȭ �۾�
        isLive = true;
        monster_run = true;
        monster_attack = false;

        targe_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();

        // �ִϸ����� �ʱ�ȭ (�ʿ��ϴٸ�)
        anim = GetComponentInChildren<Animator>();
    }
    //Ÿ������
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit target in target_player)
        {
            Vector3 mypos = transform.position;
            Vector3 targetpos = target.transform.position;
            float curDiff = Vector3.Distance(mypos, targetpos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
                Monster_Attack_Anim();
            }
            else
            {
                monster_run = true;
                monster_attack = false;
            }
        }

        return result;
    }
    //����
    void Monster_Attack_Anim()
    {
        monster_run = false;
        monster_attack = true;
        anim.SetBool("Monster_Run", monster_run);
        anim.SetBool("monster_attack", monster_attack);
    }
    IEnumerator KnocBack()
    {
        Enemy_hit_Type = true;
        //anim.SetBool("Monster_Hit", Enemy_hit_Type);

        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVector = transform.position - playerPos;
        monster_rigid.AddForce(dirVector.normalized * Bullet_Manager.Instance.NucBack_distance, ForceMode.Impulse);


        yield return new WaitForSeconds(Enemy_Hiy_Time); // 2�� ���

        Enemy_hit_Type = false;
        //anim.SetBool("Monster_Hit", Enemy_hit_Type);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            string type_name = "Monster";
            // �ߺ� ���� ����: ������ �¾Ҵ� ���̸� ����
            if (Hit_Boomerang_Bullet.Contains(other)) return;
            if(other.gameObject.name == "Bullet_Boomerang(Clone)") Hit_Boomerang_Bullet.Add(other);
            switch (other.gameObject.name)
            {
                case "Bullet_Boom":
                    hit_damage = other.gameObject.GetComponent<Bullet_Boom>().boom_damage;
                    break;
                case "Bullet_Split(Clone)":
                    hit_damage = other.gameObject.GetComponent<Bullet_Split>().Split_damage;
                    break;
                case "Bullet(Clone)":
                    hit_damage = other.gameObject.GetComponent<Bullet>().damage;
                    break;
                case "Bullet_Boomerang(Clone)":
                    hit_damage = other.gameObject.GetComponent<Bullet_Boomerang>().Boomerang_damage;
                    break;
                case "Bullet_ShotGun(Clone)":
                    hit_damage = other.gameObject.GetComponent<Bullet_ShotGun>().ShortGun_damage;
                    break;
            }
            if (hit_damage != 0)
            {
                if (Bullet_Manager.Instance.Bullet_NucBack_Type)
                { 
                    if (Hit_Delta_Time >= True_Hit_Time)
                    {
                        Hit_Delta_Time = 0;
                        StartCoroutine(KnocBack());
                    }
                }

                Audio_Manager.instance.Get_Monster_Hit_Sound(Hit_Sound);
                //Base_Chartacter_Essential_Funtion.instance.Take_Hit_Text_Damage(hit_damage_text_pro, gameObject, hit_damage_text_pos_name, hit_damage);
                Base_Chartacter_Essential_Funtion.instance.TakeDamage(gameObject, ref Monster_Hp, hit_damage, isLive, type_name);
            }
        }
        if(Monster_Hp <= 0)
        {
            Base_Chartacter_Essential_Funtion.instance.Hit_Palticle(Hit_effect_prefab, gameObject);
        }
    }
}
