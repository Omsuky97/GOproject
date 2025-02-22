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
    //���
    private List<Collider> Hit_Boomerang_Bullet = new List<Collider>(); // �̹� ���� �� ���
    Rigidbody monster_rigid;
    Animator anim;
    public Rigidbody targe_rigid;
    WaitForFixedUpdate wait;
    private float hit_damage;
    Transform result;

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
    public GameObject die_effect_prefab; // �¾��� �� ������ ��ƼŬ ������
    public bool monster_isBlinking = false;    // ��¦�� ����
    public GameObject hit_damage_text_pro;
    public string hit_damage_text_pos_name;
    public float NucBack_distance = 10.0f;

    [Header("## -- Blink_Hit -- ##")]
    public float blink_distance = 0.2f;
    public short blink_count = 1;
    public Color enemy_hit_Color = new Color(1f, 1f, 1f, 0.5f);
    private Renderer[] renderers;
    private Color[] originalColors;

    private void Awake()
    {
        monster_rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        wait = new WaitForFixedUpdate();
    }
    private void Start()
    {
        Start_Blick();
    }

    private void FixedUpdate()
    {
        if (!isLive) return;
        target_player = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, scanRange, targetLayer);
        nearestTarget = GetNearest();

        if (monster_attack == true && monster_run == false) return;
        Target_Move_Rotator();
    }
    void Target_Move_Rotator()
    {
        if (monster_isBlinking) return;
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
        monster_rigid.MovePosition(monster_rigid.position + nextVec);
        monster_rigid.velocity = Vector3.zero;
    }
    private void OnEnable()
    {
        gameObject.tag = "Enemy";
        isLive = true;
        monster_run = true;
        monster_attack = false;
        monster_isBlinking = true;
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
        Start_Blick(); // �ǰ� �� ��¦�� ȿ�� �ʱ�ȭ
        monster_isBlinking = false;
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
        }

        return result;
    }
    //����
    void Monster_Attack_Anim()
    {
        if (result == null)
        {
            monster_run = true;
            monster_attack = false;
        }
        else
        {
            monster_run = false;
            monster_attack = true;
        }

        anim.SetBool("Monster_Run", monster_run);
        anim.SetBool("monster_attack", monster_attack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            string type_name = "Monster";
            if(Monster_Hp > 0) StartCoroutine(BlinkEffect());

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
            }
            if(hit_damage != 0)
            {
                Base_Chartacter_Essential_Funtion.instance.Take_Hit_Text_Damage(hit_damage_text_pro, gameObject, hit_damage_text_pos_name, hit_damage);
                Base_Chartacter_Essential_Funtion.instance.TakeDamage(gameObject, ref Monster_Hp, hit_damage, isLive, type_name);
            }
        }
        if(Monster_Hp <= 0)
        {
            RestoreOriginalColors();
            Base_Chartacter_Essential_Funtion.instance.Hit_Palticle(die_effect_prefab, gameObject);
        }
    }
    IEnumerator KnocBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVector = transform.position - playerPos;
        monster_rigid.AddForce(dirVector.normalized * NucBack_distance, ForceMode.Impulse);

        // ���� �������� ����
        RestoreOriginalColors();
        yield return new WaitForSeconds(blink_distance);
    }
    private void Start_Blick()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].material;

            if (mat.HasProperty("_Color")) // '_Color' �Ӽ��� �ִ� ��츸 ����
            {
                originalColors[i] = mat.color;
            }
        }
    }
    private System.Collections.IEnumerator BlinkEffect()
    {
        monster_isBlinking = true;
        for (int i = 0; i < blink_count; i++)
        {
            // ������� ��ȯ
            SetColor();
            yield return new WaitForSeconds(blink_distance);

            StartCoroutine(KnocBack());
        }

        monster_isBlinking = false;
    }
    private void SetColor()
    {
        // ��� Renderer�� ���� ����
        foreach (Renderer renderer in renderers)
        {
            if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
            {
                foreach (var mat in renderer.materials)
                {
                    mat.color = enemy_hit_Color;
                }
            }
        }
    }
    private void RestoreOriginalColors()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] is MeshRenderer || renderers[i] is SkinnedMeshRenderer)
            {
                foreach (var mat in renderers[i].materials)
                {
                    mat.color = originalColors[i];
                }
            }
        }
    }
}
