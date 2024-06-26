using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f;

    public IEnumerator FadeOut()
    {
        float alpha = 0.0f;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime / fadeSpeed;
            SetColorImage(alpha);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float alpha = 1.0f;
        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            SetColorImage(alpha);
            yield return null;
        }
    }


    private void SetColorImage(float alpha)
    {
        if (fadeOutUIImage != null)
        {
            Color color = fadeOutUIImage.color;
            color.a = alpha;
            fadeOutUIImage.color = color;
        }
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    public void AfterLoadScene()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return FadeOut();
        SceneManager.LoadScene(sceneName);
    }
}