using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesCamera : MonoBehaviour
{
    public Transform target; // ���� ĳ����
    public Vector3 offset = new Vector3(-10, 15, -10); // ī�޶� ��ġ ������
    public float followSpeed = 5f; // �ε巴�� ���󰡴� �ӵ�
    public float rotationX = 45f; // X�� ȸ�� ���� (������ �Ʒ��� �������� ����)
    public float rotationY = 45f; // Y�� ȸ�� ���� (�밢�� ����)

    private Quaternion fixedRotation; // ������ ī�޶� ȸ����

    void Start()
    {
        // ī�޶��� ������ ȸ���� ����
        fixedRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = fixedRotation; // �ʱ� ȸ���� ����
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // 1. Ÿ�� ��ġ + ���������� ī�޶� ��ġ ���
            Vector3 desiredPosition = target.position + offset;

            // 2. �ε巴�� ī�޶� ��ġ �̵�
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

            // 3. ȸ�� ����
            transform.rotation = fixedRotation;
        }
    }
}

