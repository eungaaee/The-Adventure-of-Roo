using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterboxController : MonoBehaviour {
    public float duration = 1;

    private RectTransform Top, Bottom;
    private TextMeshProUGUI TextObject;

    private IEnumerator MonoEye;
    private IEnumerator Writer;

    void Awake() {
        Top = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        Bottom = transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        TextObject = transform.Find("Bottom").transform.Find("Text").GetComponent<TextMeshProUGUI>();
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
            Top.sizeDelta = Bottom.sizeDelta = Vector2.Lerp(size, targetSize, Mathf.Sin(0.5f*Mathf.PI * t/duration));
            yield return null;
        }
    }

    public IEnumerator SetLetterboxText(string Text, float Duration = 0.5f) {
        if (Writer != null) StopCoroutine(Writer);
        if (TextObject.text != "") {
            Duration = 0.25f;
            Writer = ClearLetterboxText(Duration);
            yield return Writer;
        }
        Writer = Write(Text, Vector2.zero, false, Duration);
        yield return StartCoroutine(Writer);
    }

    public IEnumerator ClearLetterboxText(float Duration = 0.5f) {
        if (Writer != null) StopCoroutine(Writer);
        Writer = Write("", new Vector2(0, -100), true, Duration);
        yield return StartCoroutine(Writer);
    }

    private IEnumerator Write(string Text, Vector2 TargetPos, bool IsErase, float Duration) {
        Vector2 InitPos = TextObject.GetComponent<RectTransform>().anchoredPosition;
        float InitAlpha = TextObject.alpha;
        if (!IsErase) TextObject.text = Text;
        for (float t = 0; t < Duration; t += Time.deltaTime) {
            TextObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(InitPos, TargetPos, Mathf.Sin(0.5f*Mathf.PI * t/Duration));
            TextObject.alpha = Mathf.Lerp(InitAlpha, IsErase ? 0 : 1, t/Duration);
            yield return null;
        }
        if (IsErase) TextObject.text = "";
    }
}