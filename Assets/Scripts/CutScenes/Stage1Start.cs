using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Start : MonoBehaviour
{
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private DialogueController Dialogue;
    private SpriteRenderer PlayerS;
    private Image FadeImage;
    [SerializeField] private TextMeshProUGUI Text;
    void Start()
    {
        PlayerS = Player.GetComponent<SpriteRenderer>();
        FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene() {
        yield return new WaitForFixedUpdate();
        Player.SwitchControllable(false);
        StartCoroutine(Player.CutSceneMove(-83f, 3f));
        yield return new WaitForSeconds(1);

        Letterbox.LetterboxOn(100, 1.5f);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "여기가 해독제가 있는 숲인가...?"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "위험한 기운이 여기까지 느껴지네..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "장로토끼님이 가까이 가지 말라는 이유를 알겠어..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "후...생각을 정리해보자..."));
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        yield return StartCoroutine(FadeInStart());

        yield return StartCoroutine(TypeText("숲의 존재와 오염물을 건들면 안되겠지...?"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "장로토끼님이 전갈,나비,가시를 조심하라하셨지"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "군데군데 독극물도 있다고 하셨어"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "돌로 된 바닥도 조심하라 하셨던것 같은데...?"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        Text.text = "";

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TypeText("데미지는 6번이상 받으면...안될거 같아..."));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "보기만 해도 많이 아파보여"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "최대한 장애물들을 피하며 가자"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        Text.text = "";

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TypeText("우물 안에는 해독제가 있어\n해독제는 꺼낸 후 시간이 지나면 효과가 사라질거야"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "장로토끼님이 해독제의 효능이\n시간이 지나면 사라진다 그려셨어"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "해독제를 얻고 바로 돌아오는게 좋을 것 같아"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        Text.text = "";

        yield return StartCoroutine(FadeOutStart());
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "지금도 마을의 토끼들은 역병으로 고통받고 있을거야..."));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "빨리 해독제를 가져오자!"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());

        StartCoroutine(Dialogue.DialogueBoxOff());
        Letterbox.LetterboxOff();
        Player.SwitchControllable(true);
    }
    public IEnumerator FadeInStart() {
        for (float f = 0f; f < 0.92; f += 0.02f) {
            Color c = FadeImage.GetComponent<Image>().color;
            c.a = f;
            FadeImage.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.015f);
        }
        yield return new WaitForSeconds(1);
    }

    public IEnumerator FadeOutStart() {
        for (float f = 0.9f; f >= 0; f -= 0.02f) {
            Color c = FadeImage.GetComponent<Image>().color;
            c.a = f;
            FadeImage.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.015f);
        }
        yield return new WaitForSeconds(1);
    }

    IEnumerator TypeText(string fullText) {
        foreach (char letter in fullText.ToCharArray()) {
            Text.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }
}


