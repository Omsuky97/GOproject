using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Effect_3X : MonoBehaviour, IPointerDownHandler
{
    //열고 닫은 후 위치 이상해지는 것만 수정


    public RectTransform[] buttons; // 3개의 버튼을 미리 등록 (Inspector에서 할당)
    private bool isExpanded = false; // 버튼이 확장되었는지 여부
    public float buttonSpacing = 50f; // 버튼 간 간격
    public float spawnDelay = 0.05f; // 버튼이 순차적으로 나타나는 시간 간격
    public float moveSpeed = 10f; // 부드러운 이동 속도
    private bool[] isMoving; // 버튼 이동 여부 체크 배열

    public RectTransform parentButton; // 부모 버튼의 RectTransform

    private void Start()
    {
        // 시작 시 버튼 숨기기
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

        for (int i = buttons.Length - 1; i >= 0; i--) // 3 → 2 → 1 순서
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
            StartCoroutine(MoveButton(rt, parentWorldPos, i, true)); // hideAfterMove = true
        }

        yield return null; // 기다릴 필요 없음 (각 버튼이 알아서 비활성화됨)
    }

    private IEnumerator MoveButton(RectTransform rt, Vector3 targetPos, int index, bool hideAfterMove = false)
    {
        if (isMoving[index]) yield break; // 이동 중이면 중복 실행 방지
        isMoving[index] = true;

        float duration = 0.3f; // 전체 이동 시간
        float elapsedTime = 0f; // 경과 시간

        Vector3 startPos = rt.position;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = t * (2 - t); // EaseOutQuad 적용 (빠르게 시작하고 천천히 멈춤)

            rt.position = Vector3.Lerp(startPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rt.position = targetPos; // 최종 위치 보정
        isMoving[index] = false;

        // 숨겨야 하는 경우 자동으로 비활성화
        if (hideAfterMove)
        {
            rt.gameObject.SetActive(false);
        }
    }
}
