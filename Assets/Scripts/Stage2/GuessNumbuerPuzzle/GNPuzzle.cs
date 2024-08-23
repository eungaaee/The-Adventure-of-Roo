using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class GNPuzzle : MonoBehaviour {
    [SerializeField] private TextMeshPro questionField, answerField;
    [SerializeField] private SceneController SceneCtr;
    [SerializeField] private GameObject Gate;
    [SerializeField] private Transform Rightdoor;
    [SerializeField] private Transform Leftdoor;
    [SerializeField] private LetterboxController Letterbox;

    [SerializeField] private GameObject Player;

    private PlayerController PlayerCtr;
    private Sequence mySequence;
    private const int QuestionAmount = 3;

    private string[] question = new string[QuestionAmount] {
        "5 + 3 = 28    9 + 1 = 810\n8 + 6 = 214    5 + 4 = 19", // 숫자 두 개 차를 앞, 합을 뒤로 한거
        "34251 = 0      257381 = 2\n3141592 = 1    127546 = 1\n21782 = 2    473829 = 3", // 구멍
        "6600 = 0    7050 = 5\n11300 = 0    470506 = 5\n323200 = 0  1070304 = 10" // 0 사이 숫자 더한거
    };
    private string[] answerFieldText = new string[QuestionAmount] {
        "4 + 3 = _",
        "10293847 = _",
        "802030304 = _"
    };
    private int[] answer = new int[QuestionAmount] { 17, 4, 8 };

    private int playerAnswer = -1;
    private int questionIndex = 0;

    public bool isChecking, isFinished;

    private void Start() {
        PlayerCtr = GameObject.Find("Roo").GetComponent<PlayerController>();
        questionField.text = question[0];
        answerField.text = answerFieldText[0];
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

    public void CorrectCheck() {
        isChecking = true;

        if (playerAnswer == answer[questionIndex]) {
            questionIndex++;

            StartCoroutine(NextQuestion());
        } else StartCoroutine(Retry());
    }

    private IEnumerator Retry() {
        yield return new WaitForSeconds(0.5f);
        answerField.text = "Wrong!";

        yield return new WaitForSeconds(2);
        questionField.text = question[questionIndex];
        answerField.text = answerFieldText[questionIndex];

        playerAnswer = -1;

        isChecking = false;
    }

    private IEnumerator NextQuestion() {
        yield return new WaitForSeconds(0.5f);
        answerField.text = "Correct!";

        yield return new WaitForSeconds(2);
        if (questionIndex < QuestionAmount) {
            questionField.text = question[questionIndex];
            answerField.text = answerFieldText[questionIndex];

            playerAnswer = -1;

            isChecking = false;
        } else {
            StartCoroutine(Finish());
        }
    }

    private IEnumerator Finish() {
        questionField.text = "";
        yield return new WaitForSeconds(2);
        answerField.text = "ALL SOLVED!";
        yield return new WaitForSeconds(2);
        PlayerCtr.SwitchControllable(false);
        Letterbox.LetterboxOn(100);
        mySequence = DOTween.Sequence()
        .Append(Rightdoor.DOMoveX(1.49f, 2).SetEase(Ease.InQuart))
        .Join(Leftdoor.DOMoveX(-0.57f, 2).SetEase(Ease.InQuart));
        yield return new WaitForSeconds(1);
        isFinished = true;
        Gate.SetActive(true);
        PlayerCtr.SwitchControllable(true);
    }
}