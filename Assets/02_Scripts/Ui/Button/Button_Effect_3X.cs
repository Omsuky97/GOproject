using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Effect_3X : MonoBehaviour, IPointerDownHandler
{
    //���� ���� �� ��ġ �̻������� �͸� ����


    public RectTransform[] buttons; // 3���� ��ư�� �̸� ��� (Inspector���� �Ҵ�)
    private bool isExpanded = false; // ��ư�� Ȯ��Ǿ����� ����
    public float buttonSpacing = 50f; // ��ư �� ����
    public float spawnDelay = 0.05f; // ��ư�� ���������� ��Ÿ���� �ð� ����
    public float moveSpeed = 10f; // �ε巯�� �̵� �ӵ�
    private bool[] isMoving; // ��ư �̵� ���� üũ �迭

    public RectTransform parentButton; // �θ� ��ư�� RectTransform

    private void Start()
    {
        // ���� �� ��ư �����
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        isMoving = new bool[buttons.Length];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isExpanded)
        {
            StartCoroutine(ShowButtonsSmoothly());
            isExpanded = true;
        }
        else
        {
            StartCoroutine(HideButtonsToParent());
            isExpanded = false;
        }
    }

    private IEnumerator ShowButtonsSmoothly()
    {
        Vector3 parentWorldPos = parentButton.transform.position;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            RectTransform rt = buttons[i].GetComponent<RectTransform>();

            Vector3 startPos = parentWorldPos;
            Vector3 targetPos = parentWorldPos + new Vector3(0, -((i + 1) * buttonSpacing), 0);

            rt.position = startPos;
            StartCoroutine(MoveButton(rt, targetPos, i));

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator HideButtonsToParent()
    {
        Vector3 parentWorldPos = parentButton.transform.position;

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rt = buttons[i].GetComponent<RectTransform>();
            StartCoroutine(MoveButton(rt, parentWorldPos, i));
        }

        yield return new WaitForSeconds(0.3f); // ��� ��ư�� �̵��� �ð��� Ȯ���� �� ��Ȱ��ȭ
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator MoveButton(RectTransform rt, Vector3 targetPos, int index)
    {
        if (isMoving[index]) yield break; // �̵� ���̸� �ߺ� ���� ����
        isMoving[index] = true;

        float elapsedTime = 0f;
        float duration = 0.3f; // �̵� �ð�

        Vector3 startPos = rt.position;

        while (elapsedTime < duration)
        {
            rt.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rt.position = targetPos;
        isMoving[index] = false;
    }
}
