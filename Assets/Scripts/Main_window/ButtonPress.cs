using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour {
    private Image pabImage;
    private RectTransform pabTransform;
    [SerializeField] SceneController Scene;

    [SerializeField] private float fadeDuration = 2.0f, blinkDuration = 1.0f;
    [SerializeField] private Vector2 targetScale = new Vector2(3000, 3000);

    private bool isFading = false;
    private Coroutine blinkCoroutine;

    private void Awake() {
        pabImage = transform.Find("PressAnyButton").GetComponent<Image>();
        pabTransform = pabImage.GetComponent<RectTransform>();
    }

    private void Start() {
        blinkCoroutine = StartCoroutine(BlinkText());
    }

    private void Update() {
        if (Input.anyKeyDown && !isFading) {
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            StartCoroutine(FadeAndGrow());
        }
    }

    private IEnumerator BlinkText() {
        float halfDuration = blinkDuration / 2f;

        while (true) {
            for (float t = 0; t <= halfDuration; t += Time.deltaTime) {
                float normalizedTime = t / halfDuration;
                pabImage.color = new Color(1, 1, 1, Mathf.Lerp(1.0f, 0.0f, normalizedTime));
                yield return null;
            }

            for (float t = 0; t <= halfDuration; t += Time.deltaTime) {
                float normalizedTime = t / halfDuration;
                pabImage.color = new Color(1, 1, 1, Mathf.Lerp(0.0f, 1.0f, normalizedTime));
                yield return null;
            }
        }
    }

    private IEnumerator FadeAndGrow() {
        GetComponent<AudioSource>().Play();
        isFading = true;
        Vector2 initialScale = pabTransform.sizeDelta;
        const float initialAlpha = 1;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime) {
            float normalizedTime = t / fadeDuration;
            pabTransform.sizeDelta = Vector2.Lerp(initialScale, targetScale, normalizedTime);
            pabImage.color = new Color(1, 1, 1, Mathf.Lerp(initialAlpha, 0.0f, normalizedTime));
            yield return null;
        }
        pabTransform.sizeDelta = targetScale;
        pabImage.color = new Color(1, 1, 1, 0.0f);

        StartCoroutine(Scene.LoadScene("Stage1"));
    }
}