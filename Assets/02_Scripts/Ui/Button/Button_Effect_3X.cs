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

        yield return new WaitForSeconds(0.3f); // 모든 버튼이 이동할 시간을 확보한 후 비활성화
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator MoveButton(RectTransform rt, Vector3 targetPos, int index)
    {
        if (isMoving[index]) yield break; // 이동 중이면 중복 실행 방지
        isMoving[index] = true;

        float elapsedTime = 0f;
        float duration = 0.3f; // 이동 시간

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
