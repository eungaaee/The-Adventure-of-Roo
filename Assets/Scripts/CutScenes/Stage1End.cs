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
        yield return StartCoroutine(Dialogue.SetDialogue("ElderBunny", "해독제를 가져온건가...?!"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "네!여기있어요!"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("ElderBunny", "말도 안 돼...하지만...너에게 숲은 정말로 위험했을텐데..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "이럴때가 아니에요! 빨리 마을로 가서 해독제로 토끼들을 치료해야해요!"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("ElderBunny", "맞는말이네...해독제를 전달해주고 오겠네!"));

        // Go to Stage 2
        yield return new WaitForSeconds(4);
        StartCoroutine(Scene.FadeOut(1));
        StartCoroutine(Scene.LoadScene("Stage2"));
    }
}
