using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour {
    public Image FadeImage;
    private const float FadeDuration = 0.8f;

    public IEnumerator FadeIn(float Duration = FadeDuration) {
        for (float t = Duration; t >= 0; t -= Time.deltaTime) {
            FadeImage.color = new Color(0, 0, 0, t / Duration);
            yield return null;
        }
        FadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut(float Duration = FadeDuration) {
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            FadeImage.color = new Color(0, 0, 0, t / Duration);
            yield return null;
        }
        FadeImage.color = new Color(0, 0, 0, 1);
    }
    
    public IEnumerator LoadScene(string sceneName) {
        yield return FadeOut();
        SceneManager.LoadScene(sceneName);
        yield return FadeIn();
    }
}