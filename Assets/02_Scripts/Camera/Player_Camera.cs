using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    private Transform camTransform; // ī�޶��� Transform
    private Vector3 originalPosition; // ī�޶��� ���� ��ġ

    private float shakeTime; // ��鸲 ���� �ð�
    private float shakePower; // ��鸲 ����

    public Rigidbody rigid;

    private void Start()
    {
        Camera.main.ScreenToWorldPoint(Vector2.zero);
        rigid = GetComponent<Rigidbody>();
        // ī�޶� Transform �ʱ�ȭ
        camTransform = transform;
        originalPosition = camTransform.localPosition;

        if (GameManager.Instance.hit_color_Image != null)
        {
            GameManager.Instance.originalColor = GameManager.Instance.hit_color_Image.color;
        }
    }

    private void Update()
    {
        // ��鸲�� Ȱ��ȭ�� ����
        if (shakeTime > 0)
        {
            // Random.insideUnitSphere�� ����Ͽ� ī�޶� ��ġ�� �����ϰ� �̵�
            camTransform.localPosition = originalPosition + Random.insideUnitSphere * shakePower;

            // ��鸲 �ð� ����
            shakeTime -= Time.deltaTime;

            // ��鸲 �ð��� ������ ���� ��ġ�� ����
            if (shakeTime <= 0f)
            {
                shakeTime = 0f;
                camTransform.localPosition = originalPosition;
            }
        }
        //���׸��� �������� ���� �õ��Ұ� (ȭ�� ��鸮�鼭 �ѿ� �� ����)
        //// UI ���� ���� ó�� (1�� �Ŀ� ��� ����)
        //if (GameManager.Instance.hit_color_Image != null)
        //{
        //    Invoke(nameof(ResetColor), GameManager.Instance.colorFadeSpeed);
        //}
    }
    //void ResetColor()
    //{
    //    if (GameManager.Instance.hit_color_Image != null)
    //    {
    //        GameManager.Instance.hit_color_Image.color = GameManager.Instance.originalColor; // 1�� �� ��� ���� ����
    //    }
    //}

    // �ܺο��� ��鸲�� Ʈ�����ϴ� �Լ�
    public void Camera_Shake(float duration, float power)
    {
        shakeTime = duration;
        shakePower = power;

        // UI ���� ����
        if (GameManager.Instance.hit_color_Image.color != null)
        {
            GameManager.Instance.hit_color_Image.color = GameManager.Instance.hit_Color;
        }
    }
}
