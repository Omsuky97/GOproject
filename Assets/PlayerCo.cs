using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;


public class PlayerCo : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        // 입력에 따른 이동 방향 설정
        if (Input.GetKey(KeyCode.A)) // 왼쪽 위 (X: -1, Z: +1)
        {
            moveDirection += new Vector3(-1, 0, 1);
        }
        if (Input.GetKey(KeyCode.S)) // 왼쪽 아래 (X: -1, Z: -1)
        {
            moveDirection += new Vector3(-1, 0, -1);
        }
        if (Input.GetKey(KeyCode.W)) // 오른쪽 위 (X: +1, Z: +1)
        {
            moveDirection += new Vector3(1, 0, 1);
        }
        if (Input.GetKey(KeyCode.D)) // 오른쪽 아래 (X: +1, Z: -1)
        {
            moveDirection += new Vector3(1, 0, -1);
        }

        // 이동 방향이 설정된 경우
        if (moveDirection != Vector3.zero)
        {
            // 이동 방향을 정규화하여 속도 유지
            moveDirection.Normalize();

            // 캐릭터 이동
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // 캐릭터 회전 (이동 방향을 바라보도록 설정)
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }
}

