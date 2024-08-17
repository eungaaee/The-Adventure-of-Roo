using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class SignTrigger : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private LogueController Logue;
    [SerializeField] private SceneController SceneCtr;
    [SerializeField] private NumberPuzzle PuzzleScript;

    private bool detectKey = false;

    private void Update() {
        if (detectKey && Input.GetKeyDown(KeyCode.E)) {
            detectKey = false;
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
            StartCoroutine(ShowDescription());
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            Letterbox.LetterboxOn(100);
            StartCoroutine(Letterbox.SetBottomText("[E] 자세히 보기"));
            detectKey = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
            detectKey = false;
        }
    }

    private IEnumerator ShowDescription() {
        yield return StartCoroutine(SceneCtr.HalfFadeOut());
        yield return StartCoroutine(Logue.SetMonologue("여기에 게임 대략 설명하기."));
        yield return new WaitForSeconds(1);

        StartCoroutine(SceneCtr.FadeOut(1));
        yield return StartCoroutine(Logue.ClearMonologue(1));

        yield return new WaitForSeconds(0.5f);
        // Start Game
        PuzzleScript.enabled = true;
        yield return StartCoroutine(SceneCtr.FadeIn(1));
    }
}
