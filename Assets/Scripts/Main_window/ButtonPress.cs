using System.Collections;
using UnityEngine;

public class SpriteController : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public float fadeDuration = 2.0f;
    public Vector3 targetScale = new Vector3(2.0f, 2.0f, 2.0f);
    public float blinkDuration = 1.0f;

    private bool isFading = false;
    private Coroutine blinkCoroutine;

    private void Start() {
        blinkCoroutine = StartCoroutine(BlinkAlpha());
    }

    private void Update() {
        if (Input.anyKeyDown && !isFading) {
            if (blinkCoroutine != null) {
                StopCoroutine(blinkCoroutine);
            }
            StartCoroutine(FadeAndGrow());
        }
    }

    private IEnumerator BlinkAlpha() {
        Color initialColor = spriteRenderer.color;
        float halfDuration = blinkDuration / 2f;

        while (true) {

            for (float t = 0; t <= halfDuration; t += Time.deltaTime) {
                float normalizedTime = t / halfDuration;
                spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(1.0f, 0.0f, normalizedTime));
                yield return null;
            }

            for (float t = 0; t <= halfDuration; t += Time.deltaTime) {
                float normalizedTime = t / halfDuration;
                spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(0.0f, 1.0f, normalizedTime));
                yield return null;
            }
        }
    }

    private IEnumerator FadeAndGrow() {
        isFading = true;
        Vector3 initialScale = transform.localScale;
        Color initialColor = spriteRenderer.color;
        float initialAlpha = initialColor.a;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime) {
            float normalizedTime = t / fadeDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, normalizedTime);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialAlpha, 0.0f, normalizedTime));
            yield return null;
        }

        transform.localScale = targetScale;
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f);
    }
}
