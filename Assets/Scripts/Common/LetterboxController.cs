using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterboxController : MonoBehaviour {
    public float Duration = 1;
    public bool IsLetterboxOn = false;

    private RectTransform Top, Bottom;
    private TextMeshProUGUI TopTextObj, BottomTextObj;

    private IEnumerator MonoEye;
    private IEnumerator Writer;

    private void Awake() {
        Top = transform.Find("Top").gameObject.GetComponent<RectTransform>();
        Bottom = transform.Find("Bottom").gameObject.GetComponent<RectTransform>();
        TopTextObj = Top.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        BottomTextObj = Bottom.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void LetterboxOn() {
        IsLetterboxOn = true;
        if (MonoEye != null) StopCoroutine(MonoEye);
        MonoEye = EyeBlinker(Top.sizeDelta, new Vector2(Top.sizeDelta.x, 200));
        StartCoroutine(MonoEye);
    }

    public void LetterboxOff() {
        if (MonoEye != null) StopCoroutine(MonoEye);
        MonoEye = EyeBlinker(Top.sizeDelta, new Vector2(Top.sizeDelta.x, 0));
        StartCoroutine(MonoEye);
        IsLetterboxOn = false;
    }

    private IEnumerator EyeBlinker(Vector2 size, Vector2 targetSize) {
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            Top.sizeDelta = Bottom.sizeDelta = Vector2.Lerp(size, targetSize, Mathf.Sin(0.5f*Mathf.PI * t/Duration));
            yield return null;
        }
    }

    public IEnumerator SetLetterboxText(string Text, int TopOrBottom = 1, float Duration = 0.5f) {
        TextMeshProUGUI TextObj;
        if (TopOrBottom == 0) TextObj = TopTextObj;
        else TextObj = BottomTextObj;

        if (Writer != null) StopCoroutine(Writer);
        if (TextObj.text != "") {
            Duration = 0.25f;
            Writer = ClearLetterboxText(TopOrBottom, Duration);
            yield return Writer;
        }
        Writer = Write(TextObj, Text, Vector2.zero, false, Duration);
        yield return StartCoroutine(Writer);
    }

    public IEnumerator ClearLetterboxText(int TopOrBottom = 1, float Duration = 0.5f) {
        TextMeshProUGUI TextObj;
        Vector2 TargetPos;
        if (TopOrBottom == 0) {
            TextObj = TopTextObj;
            TargetPos = new Vector2(0, 100);
        } else {
            TextObj = BottomTextObj;
            TargetPos = new Vector2(0, -100);
        }

        if (Writer != null) StopCoroutine(Writer);
        Writer = Write(TextObj, "", TargetPos, true, Duration);
        yield return StartCoroutine(Writer);
    }

    private IEnumerator Write(TextMeshProUGUI TextObj, string Text, Vector2 TargetPos, bool IsErase, float Duration) {
        Vector2 InitPos = TextObj.GetComponent<RectTransform>().anchoredPosition;
        float InitAlpha = TextObj.alpha;
        if (!IsErase) TextObj.text = Text;
        for (float t = 0; t < Duration; t += Time.deltaTime) {
            TextObj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(InitPos, TargetPos, Mathf.Sin(0.5f*Mathf.PI * t/Duration));
            TextObj.alpha = Mathf.Lerp(InitAlpha, IsErase ? 0 : 1, t/Duration);
            yield return null;
        }
        if (IsErase) TextObj.text = "";
    }
}