using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterboxController : MonoBehaviour {
    public float duration = 1;
    private RectTransform Top, Bottom;

    void Awake() {
        Top = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Bottom = transform.GetChild(1).gameObject.GetComponent<RectTransform>();
    }

    public IEnumerator LetterboxOn() {
        StartCoroutine(EyeBlinker(Top.sizeDelta, new Vector2(Top.sizeDelta.x, 100)));
        yield return null;
    }

    public IEnumerator LetterboxOff() {
        StartCoroutine(EyeBlinker(Top.sizeDelta, new Vector2(Top.sizeDelta.x, 0)));
        yield return null;
    }

    public IEnumerator EyeBlinker(Vector2 size, Vector2 targetSize) {
        float t = 0;
        while (t <= duration) {
            Top.sizeDelta = Bottom.sizeDelta = Vector2.Lerp(size, targetSize, t/duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}