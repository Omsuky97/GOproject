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

        // �Է¿� ���� �̵� ���� ����
        if (Input.GetKey(KeyCode.A)) // ���� �� (X: -1, Z: +1)
        {
            moveDirection += new Vector3(-1, 0, 1);
        }
        if (Input.GetKey(KeyCode.S)) // ���� �Ʒ� (X: -1, Z: -1)
        {
            moveDirection += new Vector3(-1, 0, -1);
        }
        if (Input.GetKey(KeyCode.W)) // ������ �� (X: +1, Z: +1)
        {
            moveDirection += new Vector3(1, 0, 1);
        }
        if (Input.GetKey(KeyCode.D)) // ������ �Ʒ� (X: +1, Z: -1)
        {
            moveDirection += new Vector3(1, 0, -1);
        }

        // �̵� ������ ������ ���
        if (moveDirection != Vector3.zero)
        {
            // �̵� ������ ����ȭ�Ͽ� �ӵ� ����
            moveDirection.Normalize();

            // ĳ���� �̵�
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // ĳ���� ȸ�� (�̵� ������ �ٶ󺸵��� ����)
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }
}

