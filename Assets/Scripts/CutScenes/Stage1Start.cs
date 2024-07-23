using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Start : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private LogueController Logue;

    void Start() {
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene() {
        yield return new WaitForFixedUpdate();
        Player.SwitchControllable(false);
        StartCoroutine(Player.CutSceneMove(-83f, 3f));
        yield return new WaitForSeconds(1);

        Letterbox.LetterboxOn(100, 1.5f);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "���Ⱑ �ص����� �ִ� ���ΰ�...?"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "������ ����� ������� ��������..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "����䳢���� ������ ���� ����� ������ �˰ھ�..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "��...������ �����غ���..."));
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.DialogueBoxOff());
        yield return StartCoroutine(Scene.HalfFadeOut());

        yield return StartCoroutine(Logue.SetMonologue("���� ����� �������� �ǵ�� �ȵǰ���...?"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "����䳢���� ����,����,���ø� �����϶��ϼ���"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "�������� ���ع��� �ִٰ� �ϼ̾�"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "���� �� �ٴڵ� �����϶� �ϼ̴��� ������...?"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return StartCoroutine(Logue.DialogueBoxOff());
        StartCoroutine(Logue.ClearMonologue());

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetMonologue("�������� 6���̻� ������...�ȵɰ� ����..."));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "���⸸ �ص� ���� ���ĺ���"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "�ִ��� ��ֹ����� ���ϸ� ����"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return StartCoroutine(Logue.DialogueBoxOff());
        StartCoroutine(Logue.ClearMonologue());

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetMonologue("�칰 �ȿ��� �ص����� �־�\n�ص����� ���� �� �ð��� ������ ȿ���� ������ž�"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "����䳢���� �ص����� ȿ����\n�ð��� ������ ������� �׷��̾�"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "�ص����� ��� �ٷ� ���ƿ��°� ���� �� ����"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return StartCoroutine(Logue.DialogueBoxOff());
        StartCoroutine(Logue.ClearMonologue());

        yield return StartCoroutine(Scene.FadeIn());
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "���ݵ� ������ �䳢���� �������� ����ް� �����ž�..."));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "���� �ص����� ��������!"));
        yield return new WaitForSeconds(0.85f);
        yield return StartCoroutine(Logue.ClearDialogue());

        StartCoroutine(Logue.DialogueBoxOff());
        Letterbox.LetterboxOff();
        Player.SwitchControllable(true);
    }
}