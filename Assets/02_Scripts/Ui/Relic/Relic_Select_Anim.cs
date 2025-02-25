using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic_Select_Anim : MonoBehaviour
{
    public Animator uiAnimator; // 애니메이터 컴포넌트


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
            uiAnimator.enabled = true;  // Animator가 비활성화 상태라면 활성화
            uiAnimator.Play("RelicSelect");
            Debug.Log(uiAnimator.name);
        }
    }


}
