using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;          // Image
    public TMP_Text stageClearText;  // "Stage Clear!"
    public float fadeTime = 0.5f;
    public float showTextTime = 0.8f;

    void Awake()
    {
        // 초기 상태
        if (fadeImage != null) SetAlpha(fadeImage, 0f);
        if (stageClearText != null) stageClearText.gameObject.SetActive(false);
    }

    public IEnumerator PlayStageClearSequence(string message)
    {
        // Fade Out
        yield return Fade(0f, 1f);

        // Text
        if (stageClearText != null)
        {
            stageClearText.text = message;
            stageClearText.gameObject.SetActive(true);
            yield return new WaitForSeconds(showTextTime);
            stageClearText.gameObject.SetActive(false);
        }

        // Fade In
        yield return Fade(1f, 0f);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / fadeTime);
            SetAlpha(fadeImage, a);
            yield return null;
        }
        SetAlpha(fadeImage, to);
    }

    void SetAlpha(Image img, float a)
    {
        if (img == null) return;
        var c = img.color;
        c.a = a;
        img.color = c;
    }
}

