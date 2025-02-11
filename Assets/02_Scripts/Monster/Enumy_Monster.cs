using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enumy_Monster : MonoBehaviour
{
    //기능
    Rigidbody monster_rigid;
    Animator anim;
    public Rigidbody targe_rigid;
    WaitForFixedUpdate wait;

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
    public GameObject die_effect_prefab; // 맞았을 때 실행할 파티클 프리팹
    public bool monster_isBlinking = false;    // 반짝임 여부
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
        monster_rigid.MovePosition(monster_rigid.position + nextVec);
        monster_rigid.velocity = Vector3.zero;
    }
    private void OnEnable()
    {
        isLive = true;
        monster_run = true;
        monster_attack = false;
        monster_isBlinking = true;
        Monster_Hp = Monster_MaxHp;
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
        Start_Blick(); // 피격 시 반짝임 효과 초기화
        monster_isBlinking = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            string type_name = "Monster";
            if(Monster_Hp > 0) StartCoroutine(BlinkEffect());
            float hit_damage = other.gameObject.GetComponent<Bullet>().damage;
            Base_Chartacter_Essential_Funtion.instance.Take_Hit_Text_Damage(hit_damage_text_pro, gameObject, hit_damage_text_pos_name, hit_damage);
            Base_Chartacter_Essential_Funtion.instance.TakeDamage(gameObject, ref Monster_Hp, hit_damage, isLive, type_name);
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

        // 원래 색상으로 복원
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

            if (mat.HasProperty("_Color")) // '_Color' 속성이 있는 경우만 저장
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
            // 흰색으로 전환
            SetColor();
            yield return new WaitForSeconds(blink_distance);

            StartCoroutine(KnocBack());
        }

        monster_isBlinking = false;
    }
    private void SetColor()
    {
        // 모든 Renderer의 색상 변경
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
