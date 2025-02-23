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
    public TextMeshProUGUI Loading_Text3; // �߰��� �ؽ�Ʈ
    public Button Start_Button; // ���� ���� ��ư
    public Image Loadign_Bar; // �ε� �� (������)
    public Image Transition_Image; // �� ��ȯ ���� ��� ������ �̹���
    public float fadeDuration = .5f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // �ε� �� �ʱ� ���� (fillAmount�� 0���� �����ϸ鼭 alpha �� ����)
        Loadign_Bar.fillAmount = 0f;
        SetImageAlpha(Loadign_Bar, 1); // �ε� �ٰ� ���̰� ����

        SetTextAlpha(Loading_Text1, 0); // �ؽ�Ʈ�� ����
        SetTextAlpha(Loading_Text2, 0);
        SetButtonAlpha(0); // ��ư�� ����
        Start_Button.gameObject.SetActive(false);

        StartCoroutine(LoadingProgress());
    }
    void SetImageAlpha(Image img, float alpha)
    {
        if (img != null)
        {
            Color imgColor = img.color;
            imgColor.a = Mathf.Max(alpha, 0.01f); // �ּ� 0.01f �̻� �����Ͽ� ���� ���� ����
            img.color = imgColor;
        }
    }
    public void StartGame()
    {
        StartCoroutine(ShowImageAndLoadScene("KSH"));
    }
    IEnumerator ShowImageAndLoadScene(string sceneName)
    {
        // �� ��ȯ �� �̹��� ǥ��
        if (Transition_Image != null)
        {
            Transition_Image.gameObject.SetActive(true);
            yield return FadeInImage(Transition_Image);
        }

        // 2�� ���� �̹��� ǥ��
        yield return new WaitForSeconds(2.0f);

        // �� ��ȯ ����
        SceneManager.LoadScene(sceneName);

        // ���� ������ �ε�� ������ ���
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == sceneName);

        // �� ��ȯ �� 1�� ��� �� �̹��� ���̵� �ƿ�
        yield return new WaitForSeconds(1.0f);

        if (Transition_Image != null)
        {
            yield return FadeOutImage(Transition_Image);
        }

        // �� ��ȯ�� �Ϸ�� �� �����ϰ� ����
        if (Transition_Image != null)
        {
            Destroy(Transition_Image.gameObject);
            Transition_Image = null; // ������ null�� �����Ͽ� �� �̻� �������� �ʵ��� ��
        }
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
        if (img == null) yield break; // ������Ʈ�� �̹� �����Ǿ����� �Լ� ����

        float elapsedTime = 0f;
        Color originalColor = img.color;

        while (elapsedTime < fadeDuration)
        {
            if (img == null) yield break; // ���� ���߿� �����Ǿ����� �ߴ�

            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        if (img != null)
        {
            img.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            img.gameObject.SetActive(false); // ���������� ����
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
            SetImageAlpha(Loadign_Bar, 1); // alpha ���� ��� 1�� �����Ͽ� ������� �ʰ� ����
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

        // 100% ���� �� 1�� ���
        yield return new WaitForSeconds(1.0f);

        // �ε� �� & �ؽ�Ʈ ���ÿ� ���̵� �ƿ�
        yield return StartCoroutine(FadeOutMultiple(Loadign_Bar, Loading_Text1, Loading_Text2, Loading_Text3));

        // ��ư Ȱ��ȭ
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

            // ��� ����� ���� ���ÿ� ����
            img.color = new Color(imgColor.r, imgColor.g, imgColor.b, alpha);
            text1.color = new Color(textColor1.r, textColor1.g, textColor1.b, alpha);
            text2.color = new Color(textColor2.r, textColor2.g, textColor2.b, alpha);
            text3.color = new Color(textColor3.r, textColor3.g, textColor3.b, alpha);

            yield return null;
        }

        // ���������� ������ �����ϰ� ����
        img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0);
        text1.color = new Color(textColor1.r, textColor1.g, textColor1.b, 0);
        text2.color = new Color(textColor2.r, textColor2.g, textColor2.b, 0);
        text3.color = new Color(textColor3.r, textColor3.g, textColor3.b, 0);

        // ��� ��Ȱ��ȭ (���� ����)
        img.gameObject.SetActive(false);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
    }

}
