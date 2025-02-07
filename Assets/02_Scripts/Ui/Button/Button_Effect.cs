using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Effect : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Image buttonImage; // 버튼의 이미지 컴포넌트
    private Vector2 originalPosition;
    private Color originalColor; // 원래 버튼 색상 저장
    private float moveUpAmount = 10f; // 위로 이동할 거리
    private float moveSpeed = 0.1f;  // 부드러운 이동 속도

    private static Button_Effect currentPressedButton = null; // 현재 눌린 버튼 추적

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonImage = GetComponent<Image>(); // 버튼의 Image 컴포넌트 가져오기
        if (buttonImage != null)
        {
            originalColor = buttonImage.color; // 원래 버튼 색상 저장
        }
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 다른 버튼이 눌려 있으면 원래 상태로 복구
        if (currentPressedButton != null && currentPressedButton != this)
        {
            currentPressedButton.ResetButton();
        }

        // 현재 버튼을 눌린 상태로 설정
        currentPressedButton = this;

        // 버튼 색상을 흰색으로 변경
        if (buttonImage != null)
        {
            buttonImage.color = Color.white;
        }

        // 부드럽게 위로 이동
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPosition.y + moveUpAmount));
    }

    private IEnumerator MoveButton(float targetY)
    {
        while (Mathf.Abs(rectTransform.anchoredPosition.y - targetY) > 0.1f)
        {
            // X축은 고정하고 Y축만 이동
            rectTransform.anchoredPosition = new Vector2(originalPosition.x, Mathf.Lerp(rectTransform.anchoredPosition.y, targetY, moveSpeed * Time.deltaTime * 60));
            yield return null;
        }
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, targetY);
    }

    private void ResetButton()
    {
        // 원래 위치로 복귀
        StopAllCoroutines();
        StartCoroutine(MoveButton(originalPosition.y));

        // 버튼 색상 원래대로 복구
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }
}
