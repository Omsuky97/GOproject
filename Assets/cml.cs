using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesCamera : MonoBehaviour
{
    public Transform target; // 따라갈 캐릭터
    public Vector3 offset = new Vector3(-10, 15, -10); // 카메라 위치 오프셋
    public float followSpeed = 5f; // 부드럽게 따라가는 속도
    public float rotationX = 45f; // X축 회전 각도 (위에서 아래로 내려보는 각도)
    public float rotationY = 45f; // Y축 회전 각도 (대각선 방향)

    private Quaternion fixedRotation; // 고정된 카메라 회전값

    void Start()
    {
        // 카메라의 고정된 회전값 설정
        fixedRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = fixedRotation; // 초기 회전값 적용
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 1. 타겟 위치 + 오프셋으로 카메라 위치 계산
            Vector3 desiredPosition = target.position + offset;

            // 2. 부드럽게 카메라 위치 이동
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // 3. 회전 고정
            transform.rotation = fixedRotation;
        }
    }
}

