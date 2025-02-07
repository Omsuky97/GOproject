using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Effect : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Image buttonImage; // ��ư�� �̹��� ������Ʈ
    private Vector2 originalPosition;
    private Color originalColor; // ���� ��ư ���� ����
    private float moveUpAmount = 10f; // ���� �̵��� �Ÿ�
    private float moveSpeed = 0.1f;  // �ε巯�� �̵� �ӵ�

    private static Button_Effect currentPressedButton = null; // ���� ���� ��ư ����

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonImage = GetComponent<Image>(); // ��ư�� Image ������Ʈ ��������
        if (buttonImage != null)
        {
            originalColor = buttonImage.color; // ���� ��ư ���� ����
        }
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // �ٸ� ��ư�� ���� ������ ���� ���·� ����
        if (currentPressedButton != null && currentPressedButton != this)
        {
            currentPressedButton.ResetButton();
        }

        // ���� ��ư�� ���� ���·� ����
        currentPressedButton = this;

        // ��ư ������ ������� ����
        if (buttonImage != null)
        {
            buttonImage.color = Color.white;
        }

        // �ε巴�� ���� �̵�
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPosition.y + moveUpAmount));
    }

    private IEnumerator MoveButton(float targetY)
    {
        while (Mathf.Abs(rectTransform.anchoredPosition.y - targetY) > 0.1f)
        {
            // X���� �����ϰ� Y�ุ �̵�
            rectTransform.anchoredPosition = new Vector2(originalPosition.x, Mathf.Lerp(rectTransform.anchoredPosition.y, targetY, moveSpeed * Time.deltaTime * 60));
            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, targetY);
    }

    private void ResetButton()
    {
        // ���� ��ġ�� ����
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPosition.y));

        // ��ư ���� ������� ����
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }
}
