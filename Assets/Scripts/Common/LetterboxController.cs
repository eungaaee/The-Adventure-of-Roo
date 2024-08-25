using System.Collections;
using UnityEngine;
using TMPro;

public class LetterboxController : MonoBehaviour {
    private const float boxDuration = 1, textDuration = 0.5f;
    public bool IsLetterboxOn = false;

    private RectTransform Top, Bottom;
    private TextMeshProUGUI TopTextObj, BottomTextObj;

    private IEnumerator MonoEye;
    private IEnumerator Writer1, Writer2;

    private void Awake() {
        Top = transform.Find("Top").gameObject.GetComponent<RectTransform>();
        Bottom = transform.Find("Bottom").gameObject.GetComponent<RectTransform>();
        TopTextObj = Top.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        BottomTextObj = Bottom.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void LetterboxOn(int targetSize = 200, float duration = boxDuration) {
        IsLetterboxOn = true;
        if (MonoEye != null) StopCoroutine(MonoEye);
        MonoEye = EyeBlinker(new Vector2(Top.sizeDelta.x, targetSize), duration);
        StartCoroutine(MonoEye);
    }

    public void LetterboxOff(float duration = boxDuration) {
        if (MonoEye != null) StopCoroutine(MonoEye);
        MonoEye = EyeBlinker(new Vector2(Top.sizeDelta.x, 0), duration);
        StartCoroutine(MonoEye);
        IsLetterboxOn = false;
    }

    private IEnumerator EyeBlinker(Vector2 targetSize, float duration) {
        Vector2 initSize = Top.sizeDelta;
        for (float t = 0; t <= duration; t += Time.deltaTime) {
            Top.sizeDelta = Bottom.sizeDelta = Vector2.Lerp(initSize, targetSize, Mathf.Sin(0.5f*Mathf.PI * t/duration));
            yield return null;
        }
    }

    public IEnumerator SetTopText(string Text, float duration = textDuration) {
        if (Writer1 != null) StopCoroutine(Writer1);
        if (TopTextObj.text != "") {
            duration /= 2;
            yield return StartCoroutine(ClearTopText(duration));
        }
        Writer1 = Write(TopTextObj, Text, Vector2.zero, false, duration);
        yield return StartCoroutine(Writer1);
    }

    public IEnumerator SetBottomText(string Text, float duration = textDuration) {
        if (Writer2 != null) StopCoroutine(Writer2);
        if (BottomTextObj.text != "") {
            duration /= 2;
            yield return StartCoroutine(ClearBottomText(duration));
        }
        Writer2 = Write(BottomTextObj, Text, Vector2.zero, false, duration);
        yield return StartCoroutine(Writer2);
    }

    public void EditTopText(string Text) {
        TopTextObj.text = Text;
    }

    public void EditBottomText(string Text) {
        BottomTextObj.text = Text;
    }

    public IEnumerator ClearTopText(float duration = textDuration) {
        if (Writer1 != null) StopCoroutine(Writer1);
        Writer1 = Write(TopTextObj, "", new Vector2(0, 100), true, duration);
        yield return StartCoroutine(Writer1);
    }

    public IEnumerator ClearBottomText(float duration = textDuration) {
        if (Writer2 != null) StopCoroutine(Writer2);
        Writer2 = Write(BottomTextObj, "", new Vector2(0, -100), true, duration);
        yield return StartCoroutine(Writer2);
    }

    private IEnumerator Write(TextMeshProUGUI TextObj, string Text, Vector2 TargetPos, bool IsErase, float duration) {
        Vector2 InitPos = TextObj.GetComponent<RectTransform>().anchoredPosition;
        float InitAlpha = TextObj.alpha;
        if (!IsErase) TextObj.text = Text;
        for (float t = 0; t < duration; t += Time.deltaTime) {
            TextObj.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(InitPos, TargetPos, Mathf.Sin(0.5f*Mathf.PI * t/duration));
            TextObj.alpha = Mathf.Lerp(InitAlpha, IsErase ? 0 : 1, t/duration);
            yield return null;
        }
        if (IsErase) TextObj.text = "";
    }
}