using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Lean.Gui;
using TMPro;

public class Equip : MonoBehaviour
{
    public List<Relic_Data> Relic = new List<Relic_Data>();
    public int[] relic_id_num;
    public Image[] equip_Image;
    public GameObject[] Relic_Plus_Icon;
    bool exitLoop = false;
    public Relic_Exit_Button relic_exit_button;
    public Sprite non_equip_Image;
    public bool Relic_equip_Type = false;
    public int Relic_count;

    [Header("## -- Bullet_Particle -- ##")]
    public GameObject[] Relic_ON_Particle;

    public void Relic_Equip(Image relic_image, int relic_id)
    {
        for (int Slot_Count = 0; Slot_Count < relic_id_num.Length; Slot_Count++)
        {
            if (relic_id != relic_id_num[Slot_Count])
            {
                Relic_count += 1;
                if (Relic_count == relic_id_num.Length)
                {
                    Relic_equip_Type = true;
                    break;
                }
            }
        }
        Relic_count = 0;
        if (Relic_equip_Type == false) return;
        for (int Slot_Count = 0; Slot_Count < relic_id_num.Length+1; Slot_Count++)
        {
            if (relic_id_num[Slot_Count] == 0)
            {
                relic_id_num[Slot_Count] = relic_id;
                equip_Image[Slot_Count].sprite = relic_image.sprite;
                for (int Relic_NUm = 0; Relic_NUm < Relic.Count; Relic_NUm++)
                {
                    if (Relic[Relic_NUm].Relics_id == relic_id_num[Slot_Count])
                    {
                        GameManager.Instance.bullet_damage += (GameManager.Instance.bullet_damage *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //공격력 증가
                        GameManager.Instance.Attack_Delay -= Relic[Relic_NUm].Bullet_Fispeed_UP[Relic[Relic_NUm].Relics_Lv - 1];     //발사 속도 증가
                        Bullet_Manager.Instance.Bullet_Speed += (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //공격력 증가
                        Bullet_Manager.Instance.max_penetration += Relic[Relic_NUm].Bullet_Pec_Up[Relic[Relic_NUm].Relics_Lv - 1];           //관통 횟수 증가
                        Bullet.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //튕김 횟수 증가
                        Bullet_ShotGun.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //튕김 횟수 증가
                        Bullet_Manager.Instance.NucBack_distance += Relic[Relic_NUm].Bullet_Noc_Up[Relic[Relic_NUm].Relics_Lv - 1];         //넉백 증가
                        Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //총알 분열체 수 증가
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_NUm].Bullet_Split_Up[Relic[Relic_NUm].Relics_Lv - 1];         //총알 분열체 수 증가
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_NUm].Bullet_Speaker_Up[Relic[Relic_NUm].Relics_Lv - 1];    //총알 연속발사 확률
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_NUm].Aiming_Range_Up[Relic[Relic_NUm].Relics_Lv - 1];       //조준 범위 증가
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_NUm].Bullet_ShotGun_Count[Relic[Relic_NUm].Relics_Lv - 1];         //총알 효과 트리거 횟수
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_NUm].Bullet_Effec_Tg[Relic[Relic_NUm].Relics_Lv - 1];         //총알 효과 트리거 횟수

                        if (Relic[Relic_NUm].Bullet_Spirt_Type)
                        {
                            Bullet.Relic_ON_Particle[6].SetActive(true);
                            Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //총알 분열체 수 증가
                        }
                        if (Relic[Relic_NUm].Bullet_Pec)
                        {
                            Bullet.Relic_ON_Particle[0].SetActive(true);
                            Bullet_Manager.Instance.Bullet_Pec_Type = Relic[Relic_NUm].Bullet_Pec;
                        }
                        if (Relic[Relic_NUm].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = Relic[Relic_NUm].Bullet_Speaker_Type;    //총알 연속발사 확률
                        
                        if (Relic[Relic_NUm].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = Relic[Relic_NUm].Bullet_Return;            //총알 타겟 명중 후 되돌아오는 여부
                        if (Relic[Relic_NUm].Bullet_Bounce)
                        {
                            Bullet.Relic_ON_Particle[1].SetActive(true);
                            Bullet_Manager.Instance.Bullet_bounce_Type = Relic[Relic_NUm].Bullet_Bounce;           //가장멀리있는 적 사격 여부
                        }
                        if (Relic[Relic_NUm].Bullet_Sniping)
                        {
                            Bullet.Relic_ON_Particle[4].SetActive(true);
                            Bullet_Manager.Instance.Bullet_Target_type = Relic[Relic_NUm].Bullet_Sniping;           //가장멀리있는 적 사격 여부
                        }
                        if (Relic[Relic_NUm].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = Relic[Relic_NUm].Bullet_Gra_Speed;         //총알 점진적 이동 여부
                        if (Relic[Relic_NUm].Bullet_Bomb)
                        {
                            Bullet.Relic_ON_Particle[3].SetActive(true);
                            Bullet_Manager.Instance.Bullet_Boom_Type = Relic[Relic_NUm].Bullet_Bomb;              //총알 타겟 명중 시 범위 피해 여부
                        }
                        if (Relic[Relic_NUm].Bullet_Idt)
                        {
                            Bullet.Relic_ON_Particle[2].SetActive(true);
                            Bullet_Manager.Instance.Bullet_Guided_Type = Relic[Relic_NUm].Bullet_Idt;
                        }
                        if (Relic[Relic_NUm].Bullet_Noc)
                        {
                            Bullet.Relic_ON_Particle[5].SetActive(true);
                            Bullet_Manager.Instance.Bullet_NucBack_Type = Relic[Relic_NUm].Bullet_Noc;               //총알 넉백 여부 --추가
                        }
                        if (Relic[Relic_NUm].Bullet_Effec_Type)
                        {
                            Bullet.Relic_ON_Particle[7].SetActive(true);
                            Bullet_Manager.Instance.Bullet_Bezier_Type = Relic[Relic_NUm].Bullet_Effec_Type;         //총알 효과 트리거 횟수
                        }
                        if (Relic[Relic_NUm].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = Relic[Relic_NUm].Bullet_ShotGun_Type;               //총알 넉백 여부 --추가
                        exitLoop = true;
                        relic_exit_button.relic_Num[Slot_Count] = relic_id;
                        break;
                    }
                }
                Relic_equip_Type = false;
                Relic_Plus_Icon[Slot_Count].SetActive(false);
                if (exitLoop) break;
                exitLoop = false;
            }
        }
    }
    public void Relic_Equip_Exit(int relic_id)
    {
        for (int Slot_Count = 0; Slot_Count < relic_id_num.Length + 1; Slot_Count++)
        {
            if (relic_id != 0)
            {
                for (int Relic_Num = 0; Relic_Num < Relic.Count; Relic_Num++)
                {
                    if (relic_id_num[Relic_Num] == relic_id)
                    {
                        equip_Image[Relic_Num].sprite = non_equip_Image;
                        GameManager.Instance.bullet_damage -= (GameManager.Instance.bullet_damage *= Relic[Relic_Num].Bullet_Pow_UP[Relic[Relic_Num].Relics_Lv - 1]);   //공격력 증가
                        GameManager.Instance.Attack_Delay += Relic[Relic_Num].Bullet_Fispeed_UP[Relic[Relic_Num].Relics_Lv - 1];     //발사 속도 증가
                        Bullet_Manager.Instance.Bullet_Speed -= (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_Num].Bullet_Pow_UP[Relic[Relic_Num].Relics_Lv - 1]);   //공격력 증가
                        Bullet_Manager.Instance.max_penetration -= Relic[Relic_Num].Bullet_Pec_Up[Relic[Relic_Num].Relics_Lv - 1];           //관통 횟수 증가
                        Bullet.maxBounces -= Relic[Relic_Num].Bullet_Buc_Up[Relic[Relic_Num].Relics_Lv - 1];           //튕김 횟수 증가
                        Bullet_ShotGun.maxBounces -= Relic[Relic_Num].Bullet_Buc_Up[Relic[Relic_Num].Relics_Lv - 1];           //튕김 횟수 증가
                        Bullet_Manager.Instance.NucBack_distance -= Relic[Relic_Num].Bullet_Noc_Up[Relic[Relic_Num].Relics_Lv - 1];         //넉백 증가
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_Num].Bullet_Split_Up[Relic[Relic_Num].Relics_Lv - 1];         //총알 분열체 수 증가
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_Num].Bullet_Speaker_Up[Relic[Relic_Num].Relics_Lv - 1];    //총알 연속발사 확률
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_Num].Aiming_Range_Up[Relic[Relic_Num].Relics_Lv - 1];       //조준 범위 증가
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_Num].Bullet_ShotGun_Count[Relic[Relic_Num].Relics_Lv - 1];         //총알 효과 트리거 횟수
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_Num].Bullet_Effec_Tg[Relic[Relic_Num].Relics_Lv - 1];         //총알 효과 트리거 횟수

                        if (Relic[Relic_Num].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = !Relic[Relic_Num].Bullet_Spirt_Type;         //총알 분열체 수 증가
                        if (Relic[Relic_Num].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = !Relic[Relic_Num].Bullet_Speaker_Type;    //총알 연속발사 확률
                        if (Relic[Relic_Num].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = !Relic[Relic_Num].Bullet_Return;            //총알 타겟 명중 후 되돌아오는 여부
                        if (Relic[Relic_Num].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = !Relic[Relic_Num].Bullet_Sniping;           //가장멀리있는 적 사격 여부
                        if (Relic[Relic_Num].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = !Relic[Relic_Num].Bullet_Gra_Speed;         //총알 점진적 이동 여부
                        if (Relic[Relic_Num].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = !Relic[Relic_Num].Bullet_Bomb;              //총알 타겟 명중 시 범위 피해 여부
                        if (Relic[Relic_Num].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = !Relic[Relic_Num].Bullet_Idt;               //총알 유도 여부
                        if (Relic[Relic_Num].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = !Relic[Relic_Num].Bullet_Noc;               //총알 넉백 여부 --추가
                        if (Relic[Relic_Num].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = !Relic[Relic_Num].Bullet_Effec_Type;         //총알 효과 트리거 횟수
                        if (Relic[Relic_Num].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = !Relic[Relic_Num].Bullet_ShotGun_Type;               //총알 넉백 여부 --추가
                        exitLoop = true;
                        relic_id_num[Relic_Num] = 0;
                        relic_exit_button.relic_Num[Relic_Num] = 0;
                        Relic_Plus_Icon[Relic_Num].SetActive(true);
                        break;
                    }
                }
                if (exitLoop) break;
                exitLoop = false;
            }
        }

    }
    public void Relic_Equip_Update(int relic_id)
    {
        for (int Slot_Count = 0; Slot_Count < relic_id_num.Length + 1; Slot_Count++)
        {
            if (relic_id_num[Slot_Count] == 0) return;
                if (relic_id_num[Slot_Count] == relic_id)
                {
                for (int Relic_NUm = 0; Relic_NUm < Relic.Count; Relic_NUm++)
                {
                    if (Relic[Relic_NUm].Relics_id == relic_id_num[Slot_Count])
                    {
                        GameManager.Instance.bullet_damage += (GameManager.Instance.bullet_damage *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv] - 2);   //공격력 증가
                        GameManager.Instance.Attack_Delay += Relic[Relic_NUm].Bullet_Fispeed_UP[Relic[Relic_NUm].Relics_Lv - 2];     //발사 속도 증가
                        Bullet_Manager.Instance.Bullet_Speed -= (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 2]);   //공격력 증가
                        Bullet_Manager.Instance.max_penetration -= Relic[Relic_NUm].Bullet_Pec_Up[Relic[Relic_NUm].Relics_Lv - 2];           //관통 횟수 증가
                        Bullet.maxBounces -= Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 2];           //튕김 횟수 증가
                        Bullet_ShotGun.maxBounces -= Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 2];           //튕김 횟수 증가
                        Bullet_Manager.Instance.NucBack_distance -= Relic[Relic_NUm].Bullet_Noc_Up[Relic[Relic_NUm].Relics_Lv - 2];         //넉백 증가
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_NUm].Bullet_Split_Up[Relic[Relic_NUm].Relics_Lv - 2];         //총알 분열체 수 증가
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_NUm].Bullet_Speaker_Up[Relic[Relic_NUm].Relics_Lv - 2];    //총알 연속발사 확률
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_NUm].Aiming_Range_Up[Relic[Relic_NUm].Relics_Lv - 2];       //조준 범위 증가
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_NUm].Bullet_ShotGun_Count[Relic[Relic_NUm].Relics_Lv - 2];         //총알 효과 트리거 횟수
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_NUm].Bullet_Effec_Tg[Relic[Relic_NUm].Relics_Lv - 2];         //총알 효과 트리거 횟수

                        if (Relic[Relic_NUm].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = !Relic[Relic_NUm].Bullet_Spirt_Type;         //총알 분열체 수 증가
                        if (Relic[Relic_NUm].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = !Relic[Relic_NUm].Bullet_Speaker_Type;    //총알 연속발사 확률
                        if (Relic[Relic_NUm].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = !Relic[Relic_NUm].Bullet_Return;            //총알 타겟 명중 후 되돌아오는 여부
                        if (Relic[Relic_NUm].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = !Relic[Relic_NUm].Bullet_Sniping;           //가장멀리있는 적 사격 여부
                        if (Relic[Relic_NUm].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = !Relic[Relic_NUm].Bullet_Gra_Speed;         //총알 점진적 이동 여부
                        if (Relic[Relic_NUm].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = !Relic[Relic_NUm].Bullet_Bomb;              //총알 타겟 명중 시 범위 피해 여부
                        if (Relic[Relic_NUm].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = !Relic[Relic_NUm].Bullet_Idt;               //총알 유도 여부
                        if (Relic[Relic_NUm].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = !Relic[Relic_NUm].Bullet_Noc;               //총알 넉백 여부 --추가
                        if (Relic[Relic_NUm].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = !Relic[Relic_NUm].Bullet_Effec_Type;         //총알 효과 트리거 횟수
                        if (Relic[Relic_NUm].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = !Relic[Relic_NUm].Bullet_ShotGun_Type;               //총알 넉백 여부 --추가
                    }
                    if (Relic[Relic_NUm].Relics_id == relic_id_num[Slot_Count])
                    {
                        GameManager.Instance.bullet_damage += (GameManager.Instance.bullet_damage *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //공격력 증가
                        GameManager.Instance.Attack_Delay -= Relic[Relic_NUm].Bullet_Fispeed_UP[Relic[Relic_NUm].Relics_Lv - 1];     //발사 속도 증가
                        Bullet_Manager.Instance.Bullet_Speed += (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //공격력 증가
                        Bullet_Manager.Instance.max_penetration += Relic[Relic_NUm].Bullet_Pec_Up[Relic[Relic_NUm].Relics_Lv - 1];           //관통 횟수 증가
                        Bullet.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //튕김 횟수 증가
                        Bullet_ShotGun.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //튕김 횟수 증가
                        Bullet_Manager.Instance.NucBack_distance += Relic[Relic_NUm].Bullet_Noc_Up[Relic[Relic_NUm].Relics_Lv - 1];         //넉백 증가
                        Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //총알 분열체 수 증가
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_NUm].Bullet_Split_Up[Relic[Relic_NUm].Relics_Lv - 1];         //총알 분열체 수 증가
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_NUm].Bullet_Speaker_Up[Relic[Relic_NUm].Relics_Lv - 1];    //총알 연속발사 확률
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_NUm].Aiming_Range_Up[Relic[Relic_NUm].Relics_Lv - 1];       //조준 범위 증가
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_NUm].Bullet_ShotGun_Count[Relic[Relic_NUm].Relics_Lv - 1];         //총알 효과 트리거 횟수
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_NUm].Bullet_Effec_Tg[Relic[Relic_NUm].Relics_Lv - 1];         //총알 효과 트리거 횟수

                        if (Relic[Relic_NUm].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //총알 분열체 수 증가
                        if (Relic[Relic_NUm].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = Relic[Relic_NUm].Bullet_Speaker_Type;    //총알 연속발사 확률
                        if (Relic[Relic_NUm].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = Relic[Relic_NUm].Bullet_Return;            //총알 타겟 명중 후 되돌아오는 여부
                        if (Relic[Relic_NUm].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = Relic[Relic_NUm].Bullet_Sniping;           //가장멀리있는 적 사격 여부
                        if (Relic[Relic_NUm].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = Relic[Relic_NUm].Bullet_Gra_Speed;         //총알 점진적 이동 여부
                        if (Relic[Relic_NUm].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = Relic[Relic_NUm].Bullet_Bomb;              //총알 타겟 명중 시 범위 피해 여부
                        if (Relic[Relic_NUm].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = Relic[Relic_NUm].Bullet_Idt;               //총알 유도 여부
                        if (Relic[Relic_NUm].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = Relic[Relic_NUm].Bullet_Noc;               //총알 넉백 여부 --추가
                        if (Relic[Relic_NUm].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = Relic[Relic_NUm].Bullet_Effec_Type;         //총알 효과 트리거 횟수
                        if (Relic[Relic_NUm].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = Relic[Relic_NUm].Bullet_ShotGun_Type;               //총알 넉백 여부 --추가
                        exitLoop = true;
                        relic_exit_button.relic_Num[Slot_Count] = relic_id;
                        break;
                    }
                }
                Relic_Plus_Icon[Slot_Count].SetActive(false);
                if (exitLoop) break;
                exitLoop = false;
            }
        }
    }
}
