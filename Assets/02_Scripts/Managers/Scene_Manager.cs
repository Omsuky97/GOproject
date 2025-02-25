using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{
    public TextMeshProUGUI Loading_Text1;
    public TextMeshProUGUI Loading_Text2;
    public TextMeshProUGUI Loading_Text3; // 추가된 텍스트
    public Button Start_Button; // 게임 시작 버튼
    public Image Loadign_Bar; // 로딩 바 (게이지)
    public Image Transition_Image; // 씬 전환 전에 잠깐 보여줄 이미지
    public float fadeDuration = .5f;

    [Header("## -- Game_Sound_UI -- ##")]
    public GameObject Option_UI;

    [Header("## -- Game_Sound -- ##")]
    public Slider Game_Master_Sound_Slider;
    public Slider Game_SFX_Sound_Slider;
    public Slider Game_BGM_Sound_Slider;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Game_Master_Sound_Slider.onValueChanged.AddListener(Audio_Manager.instance.Set_Master_Volume);
        Game_SFX_Sound_Slider.onValueChanged.AddListener(Audio_Manager.instance.Set_SFX_Volume);
        Game_BGM_Sound_Slider.onValueChanged.AddListener(Audio_Manager.instance.Set_BGM_Volume);
    }
    private void Start()
    {
        // 로딩 바 초기 설정 (fillAmount를 0으로 유지하면서 alpha 값 유지)
        Loadign_Bar.fillAmount = 0f;
        SetImageAlpha(Loadign_Bar, 1); // 로딩 바가 보이게 설정

        SetTextAlpha(Loading_Text1, 0); // 텍스트는 숨김
        SetTextAlpha(Loading_Text2, 0);
        SetButtonAlpha(0); // 버튼도 숨김
        Start_Button.gameObject.SetActive(false);

        StartCoroutine(LoadingProgress());
    }
    void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color imgColor = img.color;
            imgColor.a = Mathf.Max(alpha, 0.01f); // 최소 0.01f 이상 유지하여 완전 투명 방지
            img.color = imgColor;
        }
    }
    public void StartGame()
    {
        StartCoroutine(ShowImageAndLoadScene("KSH"));
    }
    IEnumerator ShowImageAndLoadScene(string sceneName)
    {
        // 씬 전환 전 이미지 표시
        if (Transition_Image != null)
        {
            Transition_Image.gameObject.SetActive(true);
            yield return FadeInImage(Transition_Image);
        }

        // 2초 동안 이미지 표시
        yield return new WaitForSeconds(2.0f);

        // 씬 전환 실행
        SceneManager.LoadScene(sceneName);

        // 씬이 완전히 로드될 때까지 대기
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        // 씬 전환 후 1초 대기 후 이미지 페이드 아웃
        yield return new WaitForSeconds(1.0f);

        if (Transition_Image != null)
        {
            yield return FadeOutImage(Transition_Image);
        }

        // 씬 전환이 완료된 후 안전하게 삭제
        if (Transition_Image != null)
        {
            Destroy(Transition_Image.gameObject);
            Transition_Image = null; // 변수도 null로 설정하여 더 이상 참조되지 않도록 함
        }
    }
    public void Game_Sound_Option()
    {
        Time.timeScale = 0.0f;
    }
    public void Game_Sound_Option_Exit()
    {
        Time.timeScale = 1.0f;
    }
    IEnumerator FadeInImage(Image img)
    {
        float elapsedTime = 0f;
        Color originalColor = img.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }
    IEnumerator FadeOutImage(Image img)
    {
        if (img == null) yield break; // 오브젝트가 이미 삭제되었으면 함수 종료

        float elapsedTime = 0f;
        Color originalColor = img.color;

        while (elapsedTime < fadeDuration)
        {
            if (img == null) yield break; // 실행 도중에 삭제되었으면 중단

            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        if (img != null)
        {
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            img.gameObject.SetActive(false); // 최종적으로 숨김
        }
    }
    void UpdateLoadingUI(int progress)
    {
        string text = $"Loading... {progress}%";
        Loading_Text1.text = text;
        Loading_Text2.text = text;

        if (Loadign_Bar != null)
        {
            Loadign_Bar.fillAmount = Mathf.Clamp(progress / 100f, 0.01f, 1f);
            SetImageAlpha(Loadign_Bar, 1); // alpha 값을 계속 1로 설정하여 사라지지 않게 유지
        }

        if (progress >= 10)
        {
            SetTextAlpha(Loading_Text1, 1);
            SetTextAlpha(Loading_Text2, 1);
        }
    }
    void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color textColor = text.color;
        textColor.a = alpha;
        text.color = textColor;
    }
    IEnumerator FadeInText(TextMeshProUGUI text)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        text.alpha = 1;
    }
    void SetButtonAlpha(float alpha)
    {
        TextMeshProUGUI buttonText = Start_Button.GetComponentInChildren<TextMeshProUGUI>();
        Color buttonColor = buttonText.color;
        buttonColor.a = alpha;
        buttonText.color = buttonColor;
    }
    IEnumerator LoadingProgress()
    {
        int progress = 0;

        progress += Random.Range(10, 31);
        UpdateLoadingUI(progress);
        yield return new WaitForSeconds(0.5f);

        while (progress < 100)
        {
            progress += Random.Range(1, 6);
            progress = Mathf.Clamp(progress, 0, 100);
            UpdateLoadingUI(progress);

            float delay = Random.Range(0.1f, 0.3f);
            yield return new WaitForSeconds(delay);
        }

        // 100% 도달 후 1초 대기
        yield return new WaitForSeconds(1.0f);

        // 로딩 바 & 텍스트 동시에 페이드 아웃
        yield return StartCoroutine(FadeOutMultiple(Loadign_Bar, Loading_Text1, Loading_Text2, Loading_Text3));

        // 버튼 활성화
        Start_Button.gameObject.SetActive(true);
        yield return FadeInText(Start_Button.GetComponentInChildren<TextMeshProUGUI>());
    }
    IEnumerator FadeOutMultiple(Image img, TextMeshProUGUI text1, TextMeshProUGUI text2, TextMeshProUGUI text3)
    {
        float elapsedTime = 0f;
        Color imgColor = img.color;
        Color textColor1 = text1.color;
        Color textColor2 = text2.color;
        Color textColor3 = text3.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);

            // 모든 요소의 투명도 동시에 변경
            img.color = new Color(imgColor.r, imgColor.g, imgColor.b, alpha);
            text1.color = new Color(textColor1.r, textColor1.g, textColor1.b, alpha);
            text2.color = new Color(textColor2.r, textColor2.g, textColor2.b, alpha);
            text3.color = new Color(textColor3.r, textColor3.g, textColor3.b, alpha);

            yield return null;
        }

        // 최종적으로 완전히 투명하게 설정
        img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
        text1.color = new Color(textColor1.r, textColor1.g, textColor1.b, 0);
        text2.color = new Color(textColor2.r, textColor2.g, textColor2.b, 0);
        text3.color = new Color(textColor3.r, textColor3.g, textColor3.b, 0);

        // 요소 비활성화 (선택 사항)
        img.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
    }

}
