using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxController : MonoBehaviour {
    public float duration;
    private RectTransform Top, Bottom;

    private IEnumerator MonoEye; // StopCoroutine

    void Awake() {
        Top = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Bottom = transform.GetChild(1).gameObject.GetComponent<RectTransform>();
    }

    public IEnumerator LetterboxOn() {
        if (MonoEye != null) StopCoroutine(MonoEye);
        MonoEye = EyeBlinker(Top.sizeDelta, new Vector2(Top.sizeDelta.x, 200));
        yield return StartCoroutine(MonoEye);
    }

    public IEnumerator LetterboxOff() {
        if (MonoEye != null) StopCoroutine(MonoEye);
        MonoEye = EyeBlinker(Top.sizeDelta, new Vector2(Top.sizeDelta.x, 0));
        yield return StartCoroutine(MonoEye);
    }

    private IEnumerator EyeBlinker(Vector2 size, Vector2 targetSize) {
        for (float t = 0; t <= duration; t += Time.deltaTime) {
            Top.sizeDelta = Bottom.sizeDelta = Vector2.Lerp(size, targetSize, t/duration);
            yield return null;
        }
    }
}