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
    //기능
    private List<Collider> Hit_Boomerang_Bullet = new List<Collider>(); // 이미 맞은 적 목록
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
    //공격 범위
    public float scanRange;
    public RaycastHit[] target_player;
    public LayerMask targetLayer;
    public Transform nearestTarget;

    //이 조건문 나중에 열거형으로 작성
    public bool isLive = true;
    public bool monster_run;
    public bool monster_attack = false;

    [Header("## -- Monster_Hit -- ##")]
    public GameObject Hit_effect_prefab; // 맞았을 때 실행할 파티클 프리팹
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
        // 대상까지의 방향 계산 (Y축 제외)
        Vector3 dirvec = targe_rigid.position - monster_rigid.position;
        dirvec.y = -3f; // 수직 방향 제거
        // 이동 벡터 계산
        Vector3 nextVec = dirvec.normalized * Monster_MoveSpeed * Time.fixedDeltaTime;
        // 몬스터를 대상 방향으로 회전
        if (dirvec != Vector3.zero)
        {
            dirvec.y = 0f; // 수직 방향 제거
            // 대상 방향으로 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(dirvec);

            // 부드럽게 회전 (Y축만 회전)
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
        Hit_effect_prefab.SetActive(false);
        targe_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();
    }
    public void Init(MonsterData data)
    {
        // CSV 데이터로부터 몬스터의 스탯 초기화
        Monster_MoveSpeed = data.MonsterMoveSpeed;
        Monster_MaxHp = data.MonsterMaxHp;
        Monster_Atk = data.MonsterAtk;
        Monster_Def = data.MonsterDef;
        Monster_Goid = data.MonsterGoid;
        Monster_AtkType = data.MonsterAtkType;
        Monster_DrItem = data.MonsterDrItem;
        Monster_Prefabs = data.MonsterPrefabs;

        Monster_Hp = Monster_MaxHp;

        // 기타 초기화 작업
        isLive = true;
        monster_run = true;
        monster_attack = false;

        targe_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();

        // 애니메이터 초기화 (필요하다면)
        anim = GetComponentInChildren<Animator>();
    }
    //타겟지정
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
    //공격
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


        yield return new WaitForSeconds(Enemy_Hiy_Time); // 2초 대기

        Enemy_hit_Type = false;
        //anim.SetBool("Monster_Hit", Enemy_hit_Type);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            string type_name = "Monster";
            // 중복 공격 방지: 기존에 맞았던 적이면 무시
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
                Base_Chartacter_Essential_Funtion.instance.TakeDamage(gameObject, ref Monster_Hp, hit_damage, isLive, type_name, Hit_effect_prefab);

            }
        }
    }
}
