using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
public class GNPuzzle : MonoBehaviour {
    [SerializeField] private TextMeshPro answerField;

    private int playerAnswer = 0;
    
    private void Update() {
        Debug.Log(playerAnswer);
    }

    public void IncreasePlayerAnswer() {
        if (playerAnswer > 999) return;
        playerAnswer++;
        UpdateAnswerText();
    }

    public void DecreasePlayerAnswer() {
        if (playerAnswer < 1) return;
        playerAnswer--;
        UpdateAnswerText();
    }

    private void UpdateAnswerText() {
        string newText = "";
        foreach (char c in answerField.text) {
            newText += c;
            if (c == '=') break;
        }
        newText += $" {playerAnswer}";
        answerField.text = newText;
    }
}