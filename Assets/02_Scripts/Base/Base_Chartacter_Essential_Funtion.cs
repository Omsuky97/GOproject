using UnityEngine;
using static Audio_Manager;

public class Base_Chartacter_Essential_Funtion : MonoBehaviour, IEssential_funtion
{
    public static Base_Chartacter_Essential_Funtion instance;
    public GameObject hitEffect;
    private void Awake()
    {
        instance = this;
    }
    //Text_pro, target이름, 데미지, 생존 여부
    public void TakeDamage(GameObject take_object, ref float health, float damage, bool live, string type)
    {
        if (type == "Player")
        {
            if (health > 0)
            {
                health -= damage;
                Audio_Manager.instance.Get_Player_Hit_Sound();
            }
            if(health <= 0)
            {
                GameManager.Instance.die_player.SetActive(false);
                GameManager.Instance.Game_Over();
            }
        }

        if (type == "Monster")
        {
            if (health > 0)
            {
                Hit_Effect(take_object.transform.position + new Vector3(0, 1, 0));
                health -= damage;
            }
            if (health <= 0)
            {
                GameManager.Instance.kill_enemy_count += 1;
                GameManager.Instance.gold_count += 5;
                GameManager.Instance.Stage_Level_UP();
                live = false;
                take_object.SetActive(false);
                take_object.transform.position = new Vector3(545f, 5f, 500f);
            }
            if (Bullet_Manager.Instance.Bullet_Guided_Type) Bullet_ShotGun.Bullet_Target = new Vector3(0, 0, 0);
        }
    }

    //Text_pro,target_object이름, target이름, 데미지
    public void Take_Hit_Text_Damage(GameObject damage_text_pro, GameObject target_object,string text_position_target, float damage)
    {
        GameObject pro_text = Instantiate(damage_text_pro);
        GUI_Damage_Text datage_text = pro_text.GetComponent<GUI_Damage_Text>();
        Transform headMarker = target_object.transform.Find(text_position_target);
        datage_text.SetDamage(headMarker);
        datage_text.player_damage = damage;
    }
    //파티클 프리펩, 파티클대상
    public void Hit_Palticle(GameObject hit_effect_prefab, GameObject take_object)
    {
        GameObject effect = Instantiate(hit_effect_prefab, take_object.transform.position, Quaternion.identity);
        Destroy(effect, 1f);
    }
    public void Hit_Effect(Vector3 Hit_Object)
    {
        GameObject effect = Instantiate(hitEffect, Hit_Object, Quaternion.identity);
        Destroy(effect, 1f);
    }
}
