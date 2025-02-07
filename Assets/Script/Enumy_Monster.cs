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

    [Header("## -- Monster_Statas_List -- ##")]
    public float monster_speed;
    public float monster_hp;
    public float monster_max_hp;
    public float monster_damage;

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
    //대상으로 이동 및 타겟대상바라보기
    //이거 정리해볼 것(불가능하면 하지말것)
    void Target_Move_Rotator()
    {
        // 대상까지의 방향 계산 (Y축 제외)
        Vector3 dirvec = targe_rigid.position - monster_rigid.position;
        dirvec.y = -3f; // 수직 방향 제거
        // 이동 벡터 계산
        Vector3 nextVec = dirvec.normalized * monster_speed * Time.fixedDeltaTime;
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
        monster_hp = monster_max_hp;
        targe_rigid = GameManager.Instance.player.GetComponent<Rigidbody>();
    }
    public void Init(Monster_Spawn_Data data)
    {
        monster_speed = data.monster_speed;
        monster_hp = data.monster_hp;
        monster_max_hp = data.monster_max_hp;
        monster_damage = data.monster_damage;
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
            if(monster_hp > 0) StartCoroutine(BlinkEffect());
            other.gameObject.SetActive(false);
            float hit_damage = other.gameObject.GetComponent<Bullet>().damage;
            Base_Chartacter_Essential_Funtion.instance.Take_Hit_Text_Damage(hit_damage_text_pro, gameObject, hit_damage_text_pos_name, hit_damage);
            Base_Chartacter_Essential_Funtion.instance.TakeDamage(gameObject, ref monster_hp, hit_damage, isLive, type_name);
        }
        if(monster_hp <= 0)
        {
            RestoreOriginalColors();
            Base_Chartacter_Essential_Funtion.instance.Hit_Palticle(die_effect_prefab, gameObject);
        }
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

            // 원래 색상으로 복원
            RestoreOriginalColors();
            yield return new WaitForSeconds(blink_distance);
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
