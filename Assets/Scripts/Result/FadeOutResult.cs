using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOutResult : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.0f;

    void Start()
    {
        // 最初は完全に透明にしておく
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f);
        }
    }

    public void StartFade(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        float t = 0;
        Color color = fadeImage.color;

        // フェード中
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Time.timeScale = 1f;
        IsGameManager.isGameEnded = false;
        SceneManager.LoadScene(sceneName);
    }
}
