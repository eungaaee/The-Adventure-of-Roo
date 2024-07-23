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
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "���Ⱑ �ص����� �ִ� ���ΰ�...?"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "������ ����� ������� ��������..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "����䳢���� ������ ���� ����� ������ �˰ھ�..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "��...������ �����غ���..."));
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        yield return StartCoroutine(FadeInStart());

        yield return StartCoroutine(TypeText("���� ����� �������� �ǵ�� �ȵǰ���...?"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "����䳢���� ����,����,���ø� �����϶��ϼ���"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "�������� ���ع��� �ִٰ� �ϼ̾�"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "���� �� �ٴڵ� �����϶� �ϼ̴��� ������...?"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        Text.text = "";

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TypeText("�������� 6���̻� ������...�ȵɰ� ����..."));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "���⸸ �ص� ���� ���ĺ���"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "�ִ��� ��ֹ����� ���ϸ� ����"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        Text.text = "";

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TypeText("�칰 �ȿ��� �ص����� �־�\n�ص����� ���� �� �ð��� ������ ȿ���� ������ž�"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "����䳢���� �ص����� ȿ����\n�ð��� ������ ������� �׷��̾�"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "�ص����� ��� �ٷ� ���ƿ��°� ���� �� ����"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.ClearDialogue());
        yield return StartCoroutine(Dialogue.DialogueBoxOff());
        Text.text = "";

        yield return StartCoroutine(FadeOutStart());
        yield return StartCoroutine(Dialogue.DialogueBoxOn());
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "���ݵ� ������ �䳢���� �������� ����ް� �����ž�..."));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Dialogue.SetDialogue("Roo", "���� �ص����� ��������!"));
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


