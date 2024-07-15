using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour {
    public Image FadeImage;
    public float FadeSpeed = 0.8f;

    public IEnumerator FadeIn() {
        float Alpha = 1.0f;
        while (Alpha > 0.0f) {
            Alpha -= Time.deltaTime / FadeSpeed;
            SetImageAlpha(Alpha);
            yield return null;
        }
    }

    public IEnumerator FadeOut() {
        float Alpha = 0.0f;
        while (Alpha < 1.0f) {
            Alpha += Time.deltaTime / FadeSpeed;
            SetImageAlpha(Alpha);
            yield return null;
        }
    }

    private void SetImageAlpha(float Alpha) {
        if (FadeImage != null) {
            Color NewColor = FadeImage.color;
            NewColor.a = Alpha;
            FadeImage.color = NewColor;
        }
    }
    
    public void LoadScene(string sceneName) {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    public void AfterLoadScene() {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeAndLoadScene(string sceneName) {
        yield return FadeOut();
        SceneManager.LoadScene(sceneName);
    }
}