using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogueController : MonoBehaviour {
    [SerializeField] private Image Talker;
    [SerializeField] private TextMeshProUGUI DialogueTextObj, MonologueTextObj;
    [SerializeField] private CanvasGroup DialogueBox;

    [SerializeField] private Sprite Roo_Idle, ElderBunny;

    private void SetTalker(string name) {
        Talker.sprite = name switch {
            "Roo" => Roo_Idle,
            "ElderBunny" => ElderBunny,
            "null" => null,
            _ => null,
        };
    }

    public IEnumerator DialogueBoxOn() {
        const float duration = 1f;
        for (float t = 0; t <= duration; t += Time.deltaTime) {
            DialogueBox.alpha = Mathf.Sin(0.5f*Mathf.PI * t/duration);
            yield return null;
        }
        DialogueBox.alpha = 1;
        Talker.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator DialogueBoxOff() {
        Talker.color = new Color(1, 1, 1, 0);
        const float duration = 1f;
        for (float t = duration; t >= 0; t -= Time.deltaTime) {
            DialogueBox.alpha = Mathf.Sin(0.5f*Mathf.PI * t/duration);
            yield return null;
        }
        DialogueBox.alpha = 0;
    }

    public IEnumerator SetDialogue(string name, string text, float relativeInterval = 1) {
        if (DialogueTextObj.text != "") yield return StartCoroutine(ClearDialogue());
        DialogueTextObj.color = new Color(0, 0, 0, 1);
        SetTalker(name);
        foreach (char letter in text) {
            DialogueTextObj.text += letter;
            yield return new WaitForSeconds(0.05f * relativeInterval);
        }
    }

    public IEnumerator ClearDialogue(float duration = 0.5f) {
        for (float t = duration; t >= 0; t -= Time.deltaTime) {
            DialogueTextObj.color = new Color(0, 0, 0, Mathf.Sin(0.5f*Mathf.PI * t/duration));
            yield return null;
        }
        DialogueTextObj.color = new Color(0, 0, 0, 0);
        DialogueTextObj.text = "";
    }

    public IEnumerator SetMonologue(string text, float relativeInterval = 1) {
        if (MonologueTextObj.text != "") yield return StartCoroutine(ClearDialogue());
        Color curColor = MonologueTextObj.color;
        curColor.a = 1;
        MonologueTextObj.color = curColor;
        yield return new WaitForFixedUpdate();
        
        foreach (char letter in text) {
            MonologueTextObj.text += letter;
            yield return new WaitForSeconds(0.05f * relativeInterval);
        }
    }

    public IEnumerator ClearMonologue(float duration = 0.5f) {
        Color curColor = MonologueTextObj.color;
        for (float t = duration; t >= 0; t -= Time.deltaTime) {
            curColor.a = Mathf.Sin(0.5f*Mathf.PI * t/duration);
            MonologueTextObj.color = curColor;
            yield return null;
        }
        curColor.a = 0;
        MonologueTextObj.color = curColor;
        MonologueTextObj.text = "";
    }
}