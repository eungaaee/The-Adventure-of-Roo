using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1End : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private DialogueController Dialogue;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            StartCoroutine(StartCutscene());
        }
    }

    private IEnumerator StartCutscene() {
        // 장로토끼와 대화할 장소로 이동
        yield return new WaitForFixedUpdate();
        Player.SwitchControllable(false);
        Letterbox.LetterboxOn(100, 1.5f);
        StartCoroutine(Player.CutSceneJump(1));
        StartCoroutine(Player.CutSceneMove(-78.5f, 1.5f));

        // 대화 시작
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("ElderBunny", "수고했어, 루.\n너의 용기로 이 마을을 구해냈어."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "감사합니다, 장로토끼!"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("ElderBunny", "하지만 너의 여정은 아직 끝나지 않았어.\n더 많은 시련이 기다리고 있을 거야."));

        // Go to Stage 2
        yield return new WaitForSeconds(4);
        StartCoroutine(Scene.FadeOut(1));
        StartCoroutine(Scene.LoadScene("Stage2"));
    }
}
