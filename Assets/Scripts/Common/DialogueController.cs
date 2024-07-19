using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {
    private Image Talker;
    private TextMeshProUGUI TextObj;
    private CanvasGroup DialogueBox;

    [SerializeField] private Sprite Roo_Idle, ElderBunny;

    private void Awake() {
        Talker = transform.Find("Talker").GetComponent<Image>();
        TextObj = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        DialogueBox = GetComponent<CanvasGroup>();
    }

    private void SetTalker(string name) {
        switch (name) {
            case "Roo":
                Talker.sprite = Roo_Idle;
                break;
            case "ElderBunny":
                Talker.sprite = ElderBunny;
                break;
            case "null":
            default:
                Talker.sprite = null;
                break;
        }
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

    public IEnumerator SetDialogue(string name, string text) {
        if (TextObj.text != "") yield return StartCoroutine(ClearDialogue());
        TextObj.color = new Color(0, 0, 0, 1);
        SetTalker(name);
        foreach (char letter in text) {
            TextObj.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator ClearDialogue() {
        const float duration = 0.5f;
        for (float t = duration; t >= 0; t -= Time.deltaTime) {
            TextObj.color = new Color(0, 0, 0, Mathf.Sin(0.5f*Mathf.PI * t/duration));
            yield return null;
        }
        TextObj.color = new Color(0, 0, 0, 0);
        TextObj.text = "";
    }
}