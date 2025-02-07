using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    private Transform camTransform; // 카메라의 Transform
    private Vector3 originalPosition; // 카메라의 원래 위치

    private float shakeTime; // 흔들림 지속 시간
    private float shakePower; // 흔들림 강도

    public Rigidbody rigid;

    private void Start()
    {
        Camera.main.ScreenToWorldPoint(Vector2.zero);
        rigid = GetComponent<Rigidbody>();
        // 카메라 Transform 초기화
        camTransform = transform;
        originalPosition = camTransform.localPosition;

        if (GameManager.Instance.hit_color_Image != null)
        {
            GameManager.Instance.originalColor = GameManager.Instance.hit_color_Image.color;
        }
    }

    private void Update()
    {
        // 흔들림이 활성화된 상태
        if (shakeTime > 0)
        {
            // Random.insideUnitSphere를 사용하여 카메라 위치를 랜덤하게 이동
            camTransform.localPosition = originalPosition + Random.insideUnitSphere * shakePower;

            // 흔들림 시간 감소
            shakeTime -= Time.deltaTime;

            // 흔들림 시간이 끝나면 원래 위치로 복구
            if (shakeTime <= 0f)
            {
                shakeTime = 0f;
                camTransform.localPosition = originalPosition;
            }
        }
        //메테리얼 가져오면 구현 시도할것 (화면 흔들리면서 겉에 색 변경)
        //// UI 색상 복구 처리 (1초 후에 즉시 변경)
        //if (GameManager.Instance.hit_color_Image != null)
        //{
        //    Invoke(nameof(ResetColor), GameManager.Instance.colorFadeSpeed);
        //}
    }
    //void ResetColor()
    //{
    //    if (GameManager.Instance.hit_color_Image != null)
    //    {
    //        GameManager.Instance.hit_color_Image.color = GameManager.Instance.originalColor; // 1초 후 즉시 색상 변경
    //    }
    //}

    // 외부에서 흔들림을 트리거하는 함수
    public void Camera_Shake(float duration, float power)
    {
        shakeTime = duration;
        shakePower = power;

        // UI 색상 변경
        if (GameManager.Instance.hit_color_Image.color != null)
        {
            GameManager.Instance.hit_color_Image.color = GameManager.Instance.hit_Color;
        }
    }
}
