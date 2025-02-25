using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_Select_Anim : MonoBehaviour
{
    public Animator uiAnimator; // �ִϸ����� ������Ʈ


    void Start()
    {
        if (uiAnimator != null)
        {
            uiAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

    void OnEnable()
    {
        if (uiAnimator != null)
        {
            uiAnimator.enabled = true;  // Animator�� ��Ȱ��ȭ ���¶�� Ȱ��ȭ
            uiAnimator.Play("RelicSelect");
            Debug.Log(uiAnimator.name);
        }
    }


}
