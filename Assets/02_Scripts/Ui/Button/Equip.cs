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
                        GameManager.Instance.bullet_damage += (GameManager.Instance.bullet_damage *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv-1]);   //���ݷ� ����
                        GameManager.Instance.Attack_Delay -= Relic[Relic_NUm].Bullet_Fispeed_UP[Relic[Relic_NUm].Relics_Lv - 1];     //�߻� �ӵ� ����
                        Bullet_Manager.Instance.Bullet_Speed += (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //���ݷ� ����
                        Bullet_Manager.Instance.max_penetration += Relic[Relic_NUm].Bullet_Pec_Up[Relic[Relic_NUm].Relics_Lv - 1];           //���� Ƚ�� ����
                        Bullet.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //ƨ�� Ƚ�� ����
                        Bullet_ShotGun.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //ƨ�� Ƚ�� ����
                        Bullet_Manager.Instance.NucBack_distance += Relic[Relic_NUm].Bullet_Noc_Up[Relic[Relic_NUm].Relics_Lv - 1];         //�˹� ����
                        Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //�Ѿ� �п�ü �� ����
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_NUm].Bullet_Split_Up[Relic[Relic_NUm].Relics_Lv - 1];         //�Ѿ� �п�ü �� ����
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_NUm].Bullet_Speaker_Up[Relic[Relic_NUm].Relics_Lv - 1];    //�Ѿ� ���ӹ߻� Ȯ��
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_NUm].Aiming_Range_Up[Relic[Relic_NUm].Relics_Lv - 1];       //���� ���� ����
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_NUm].Bullet_ShotGun_Count[Relic[Relic_NUm].Relics_Lv - 1];         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_NUm].Bullet_Effec_Tg[Relic[Relic_NUm].Relics_Lv - 1];         //�Ѿ� ȿ�� Ʈ���� Ƚ��

                        if (Relic[Relic_NUm].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //�Ѿ� �п�ü �� ����
                        if (Relic[Relic_NUm].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = Relic[Relic_NUm].Bullet_Speaker_Type;    //�Ѿ� ���ӹ߻� Ȯ��
                        if (Relic[Relic_NUm].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = Relic[Relic_NUm].Bullet_Return;            //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
                        if (Relic[Relic_NUm].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = Relic[Relic_NUm].Bullet_Sniping;           //����ָ��ִ� �� ��� ����
                        if (Relic[Relic_NUm].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = Relic[Relic_NUm].Bullet_Gra_Speed;         //�Ѿ� ������ �̵� ����
                        if (Relic[Relic_NUm].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = Relic[Relic_NUm].Bullet_Bomb;              //�Ѿ� Ÿ�� ���� �� ���� ���� ����
                        if (Relic[Relic_NUm].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = Relic[Relic_NUm].Bullet_Idt;               //�Ѿ� ���� ����
                        if (Relic[Relic_NUm].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = Relic[Relic_NUm].Bullet_Noc;               //�Ѿ� �˹� ���� --�߰�
                        if (Relic[Relic_NUm].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = Relic[Relic_NUm].Bullet_Effec_Type;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        if (Relic[Relic_NUm].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = Relic[Relic_NUm].Bullet_ShotGun_Type;               //�Ѿ� �˹� ���� --�߰�
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
                        GameManager.Instance.bullet_damage -= (GameManager.Instance.bullet_damage *= Relic[Relic_Num].Bullet_Pow_UP[Relic[Relic_Num].Relics_Lv - 1]);   //���ݷ� ����
                        GameManager.Instance.Attack_Delay += Relic[Relic_Num].Bullet_Fispeed_UP[Relic[Relic_Num].Relics_Lv - 1];     //�߻� �ӵ� ����
                        Bullet_Manager.Instance.Bullet_Speed -= (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_Num].Bullet_Pow_UP[Relic[Relic_Num].Relics_Lv - 1]);   //���ݷ� ����
                        Bullet_Manager.Instance.max_penetration -= Relic[Relic_Num].Bullet_Pec_Up[Relic[Relic_Num].Relics_Lv - 1];           //���� Ƚ�� ����
                        Bullet.maxBounces -= Relic[Relic_Num].Bullet_Buc_Up[Relic[Relic_Num].Relics_Lv - 1];           //ƨ�� Ƚ�� ����
                        Bullet_ShotGun.maxBounces -= Relic[Relic_Num].Bullet_Buc_Up[Relic[Relic_Num].Relics_Lv - 1];           //ƨ�� Ƚ�� ����
                        Bullet_Manager.Instance.NucBack_distance -= Relic[Relic_Num].Bullet_Noc_Up[Relic[Relic_Num].Relics_Lv - 1];         //�˹� ����
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_Num].Bullet_Split_Up[Relic[Relic_Num].Relics_Lv - 1];         //�Ѿ� �п�ü �� ����
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_Num].Bullet_Speaker_Up[Relic[Relic_Num].Relics_Lv - 1];    //�Ѿ� ���ӹ߻� Ȯ��
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_Num].Aiming_Range_Up[Relic[Relic_Num].Relics_Lv - 1];       //���� ���� ����
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_Num].Bullet_ShotGun_Count[Relic[Relic_Num].Relics_Lv - 1];         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_Num].Bullet_Effec_Tg[Relic[Relic_Num].Relics_Lv - 1];         //�Ѿ� ȿ�� Ʈ���� Ƚ��

                        if (Relic[Relic_Num].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = !Relic[Relic_Num].Bullet_Spirt_Type;         //�Ѿ� �п�ü �� ����
                        if (Relic[Relic_Num].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = !Relic[Relic_Num].Bullet_Speaker_Type;    //�Ѿ� ���ӹ߻� Ȯ��
                        if (Relic[Relic_Num].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = !Relic[Relic_Num].Bullet_Return;            //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
                        if (Relic[Relic_Num].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = !Relic[Relic_Num].Bullet_Sniping;           //����ָ��ִ� �� ��� ����
                        if (Relic[Relic_Num].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = !Relic[Relic_Num].Bullet_Gra_Speed;         //�Ѿ� ������ �̵� ����
                        if (Relic[Relic_Num].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = !Relic[Relic_Num].Bullet_Bomb;              //�Ѿ� Ÿ�� ���� �� ���� ���� ����
                        if (Relic[Relic_Num].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = !Relic[Relic_Num].Bullet_Idt;               //�Ѿ� ���� ����
                        if (Relic[Relic_Num].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = !Relic[Relic_Num].Bullet_Noc;               //�Ѿ� �˹� ���� --�߰�
                        if (Relic[Relic_Num].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = !Relic[Relic_Num].Bullet_Effec_Type;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        if (Relic[Relic_Num].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = !Relic[Relic_Num].Bullet_ShotGun_Type;               //�Ѿ� �˹� ���� --�߰�
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
                        GameManager.Instance.bullet_damage += (GameManager.Instance.bullet_damage *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv] - 2);   //���ݷ� ����
                        GameManager.Instance.Attack_Delay += Relic[Relic_NUm].Bullet_Fispeed_UP[Relic[Relic_NUm].Relics_Lv - 2];     //�߻� �ӵ� ����
                        Bullet_Manager.Instance.Bullet_Speed -= (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 2]);   //���ݷ� ����
                        Bullet_Manager.Instance.max_penetration -= Relic[Relic_NUm].Bullet_Pec_Up[Relic[Relic_NUm].Relics_Lv - 2];           //���� Ƚ�� ����
                        Bullet.maxBounces -= Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 2];           //ƨ�� Ƚ�� ����
                        Bullet_ShotGun.maxBounces -= Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 2];           //ƨ�� Ƚ�� ����
                        Bullet_Manager.Instance.NucBack_distance -= Relic[Relic_NUm].Bullet_Noc_Up[Relic[Relic_NUm].Relics_Lv - 2];         //�˹� ����
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_NUm].Bullet_Split_Up[Relic[Relic_NUm].Relics_Lv - 2];         //�Ѿ� �п�ü �� ����
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_NUm].Bullet_Speaker_Up[Relic[Relic_NUm].Relics_Lv - 2];    //�Ѿ� ���ӹ߻� Ȯ��
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_NUm].Aiming_Range_Up[Relic[Relic_NUm].Relics_Lv - 2];       //���� ���� ����
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_NUm].Bullet_ShotGun_Count[Relic[Relic_NUm].Relics_Lv - 2];         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_NUm].Bullet_Effec_Tg[Relic[Relic_NUm].Relics_Lv - 2];         //�Ѿ� ȿ�� Ʈ���� Ƚ��

                        if (Relic[Relic_NUm].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = !Relic[Relic_NUm].Bullet_Spirt_Type;         //�Ѿ� �п�ü �� ����
                        if (Relic[Relic_NUm].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = !Relic[Relic_NUm].Bullet_Speaker_Type;    //�Ѿ� ���ӹ߻� Ȯ��
                        if (Relic[Relic_NUm].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = !Relic[Relic_NUm].Bullet_Return;            //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
                        if (Relic[Relic_NUm].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = !Relic[Relic_NUm].Bullet_Sniping;           //����ָ��ִ� �� ��� ����
                        if (Relic[Relic_NUm].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = !Relic[Relic_NUm].Bullet_Gra_Speed;         //�Ѿ� ������ �̵� ����
                        if (Relic[Relic_NUm].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = !Relic[Relic_NUm].Bullet_Bomb;              //�Ѿ� Ÿ�� ���� �� ���� ���� ����
                        if (Relic[Relic_NUm].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = !Relic[Relic_NUm].Bullet_Idt;               //�Ѿ� ���� ����
                        if (Relic[Relic_NUm].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = !Relic[Relic_NUm].Bullet_Noc;               //�Ѿ� �˹� ���� --�߰�
                        if (Relic[Relic_NUm].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = !Relic[Relic_NUm].Bullet_Effec_Type;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        if (Relic[Relic_NUm].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = !Relic[Relic_NUm].Bullet_ShotGun_Type;               //�Ѿ� �˹� ���� --�߰�
                    }
                    if (Relic[Relic_NUm].Relics_id == relic_id_num[Slot_Count])
                    {
                        GameManager.Instance.bullet_damage += (GameManager.Instance.bullet_damage *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //���ݷ� ����
                        GameManager.Instance.Attack_Delay -= Relic[Relic_NUm].Bullet_Fispeed_UP[Relic[Relic_NUm].Relics_Lv - 1];     //�߻� �ӵ� ����
                        Bullet_Manager.Instance.Bullet_Speed += (Bullet_Manager.Instance.Bullet_Speed *= Relic[Relic_NUm].Bullet_Pow_UP[Relic[Relic_NUm].Relics_Lv - 1]);   //���ݷ� ����
                        Bullet_Manager.Instance.max_penetration += Relic[Relic_NUm].Bullet_Pec_Up[Relic[Relic_NUm].Relics_Lv - 1];           //���� Ƚ�� ����
                        Bullet.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //ƨ�� Ƚ�� ����
                        Bullet_ShotGun.maxBounces += Relic[Relic_NUm].Bullet_Buc_Up[Relic[Relic_NUm].Relics_Lv - 1];           //ƨ�� Ƚ�� ����
                        Bullet_Manager.Instance.NucBack_distance += Relic[Relic_NUm].Bullet_Noc_Up[Relic[Relic_NUm].Relics_Lv - 1];         //�˹� ����
                        Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //�Ѿ� �п�ü �� ����
                        Bullet_Manager.Instance.Bullet_Spirt_Count += Relic[Relic_NUm].Bullet_Split_Up[Relic[Relic_NUm].Relics_Lv - 1];         //�Ѿ� �п�ü �� ����
                        Bullet_Manager.Instance.Bullet_Speaker_Count -= Relic[Relic_NUm].Bullet_Speaker_Up[Relic[Relic_NUm].Relics_Lv - 1];    //�Ѿ� ���ӹ߻� Ȯ��
                        Bullet_Manager.Instance.Bullet_Scan_Range += Relic[Relic_NUm].Aiming_Range_Up[Relic[Relic_NUm].Relics_Lv - 1];       //���� ���� ����
                        Bullet_Manager.Instance.Bullet_ShotGun_Count += Relic[Relic_NUm].Bullet_ShotGun_Count[Relic[Relic_NUm].Relics_Lv - 1];         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        Bullet_Manager.Instance.Bullet_Bezier_Count -= Relic[Relic_NUm].Bullet_Effec_Tg[Relic[Relic_NUm].Relics_Lv - 1];         //�Ѿ� ȿ�� Ʈ���� Ƚ��

                        if (Relic[Relic_NUm].Bullet_Spirt_Type) Bullet_Manager.Instance.Bullet_Spirt_Type = Relic[Relic_NUm].Bullet_Spirt_Type;         //�Ѿ� �п�ü �� ����
                        if (Relic[Relic_NUm].Bullet_Speaker_Type) Bullet_Manager.Instance.Bullet_Speaker_Type = Relic[Relic_NUm].Bullet_Speaker_Type;    //�Ѿ� ���ӹ߻� Ȯ��
                        if (Relic[Relic_NUm].Bullet_Return) Bullet_Manager.Instance.Bullet_Boomerang_Type = Relic[Relic_NUm].Bullet_Return;            //�Ѿ� Ÿ�� ���� �� �ǵ��ƿ��� ����
                        if (Relic[Relic_NUm].Bullet_Sniping) Bullet_Manager.Instance.Bullet_Target_type = Relic[Relic_NUm].Bullet_Sniping;           //����ָ��ִ� �� ��� ����
                        if (Relic[Relic_NUm].Bullet_Gra_Speed) Bullet_Manager.Instance.Bullet_Propulsion_Type = Relic[Relic_NUm].Bullet_Gra_Speed;         //�Ѿ� ������ �̵� ����
                        if (Relic[Relic_NUm].Bullet_Bomb) Bullet_Manager.Instance.Bullet_Boom_Type = Relic[Relic_NUm].Bullet_Bomb;              //�Ѿ� Ÿ�� ���� �� ���� ���� ����
                        if (Relic[Relic_NUm].Bullet_Idt) Bullet_Manager.Instance.Bullet_Guided_Type = Relic[Relic_NUm].Bullet_Idt;               //�Ѿ� ���� ����
                        if (Relic[Relic_NUm].Bullet_Noc) Bullet_Manager.Instance.Bullet_NucBack_Type = Relic[Relic_NUm].Bullet_Noc;               //�Ѿ� �˹� ���� --�߰�
                        if (Relic[Relic_NUm].Bullet_Effec_Type) Bullet_Manager.Instance.Bullet_Bezier_Type = Relic[Relic_NUm].Bullet_Effec_Type;         //�Ѿ� ȿ�� Ʈ���� Ƚ��
                        if (Relic[Relic_NUm].Bullet_ShotGun_Type) Bullet_Manager.Instance.Bullet_ShotGun_Type = Relic[Relic_NUm].Bullet_ShotGun_Type;               //�Ѿ� �˹� ���� --�߰�
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
