using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;
using DG.Tweening;

public class Puzzle1 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] Lights1;
    [SerializeField] private SpriteRenderer[] Lights2;
    [SerializeField] private SpriteRenderer[] Lights3;

    [SerializeField] private GameObject Player;
    [SerializeField] private buttonpress1 buttonpressed1;
    [SerializeField] private buttonpress2 buttonpressed2;
    [SerializeField] private buttonpress3 buttonpressed3;
    [SerializeField] private buttonpress4 buttonpressed4;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private Transform Rightdoor;
    [SerializeField] private Transform Leftdoor;
    [SerializeField] private MainCameraController MainCamera;
    private PlayerController PlayerCtr;
    private GateController GateControl;
    private Sequence mySequence;
    private Color red = Color.red;
    private Color green = Color.green;
    public int Round = 1;

    private static readonly int[] ChangeColor1 = new int[3] { 0, 2, 1 };    // 3��
    private static readonly int[,] ChangeColor2 = new int[4, 3]             // 5��
    {
        {0,2,3},
        {1,2,4},
        {0,3,4},
        {0,1,3}
    };// 0 1 2 3 4
    private static readonly int[,] ChangeColor3 = new int[4, 3]             // 7��
    {
        {0,2,5},
        {0,1,2},
        {2,3,4},
        {2,5,6}
    };// 0 1 2 3 4 5 6 

    void Start()
    {
        PlayerCtr = GameObject.Find("Roo").GetComponent<PlayerController>();
        buttonpressed1 = GameObject.Find("button1").GetComponent<buttonpress1>();
        buttonpressed2 = GameObject.Find("button2").GetComponent<buttonpress2>();
        buttonpressed3 = GameObject.Find("button3").GetComponent<buttonpress3>();
        buttonpressed4 = GameObject.Find("button4").GetComponent<buttonpress4>();
        GateControl = GameObject.Find("FirstFloorGate").GetComponent<GateController>();
    }

    // Update is called once per frame
    void Update() {
        if (Round == 1) {
            if (buttonpressed1.button1pressed == true) {
                if (Lights1[ChangeColor1[0]].color == green) Lights1[ChangeColor1[0]].color = red;
                else Lights1[ChangeColor1[0]].color = green;
                buttonpressed1.button1pressed = false;
            }
            if (buttonpressed2.button2pressed == true) {
                if (Lights1[ChangeColor1[1]].color == green) Lights1[ChangeColor1[1]].color = red;
                else Lights1[ChangeColor1[1]].color = green;
                buttonpressed2.button2pressed = false;
            }
            if (buttonpressed3.button3pressed == true) {
                if (Lights1[ChangeColor1[2]].color == green) Lights1[ChangeColor1[2]].color = red;
                else Lights1[ChangeColor1[2]].color = green;
                buttonpressed3.button3pressed = false;
            }
            if (buttonpressed4.button4pressed == true) {
                if (Lights1[ChangeColor1[0]].color == green) Lights1[ChangeColor1[2]].color = red;
                else Lights1[ChangeColor1[0]].color = green;
                buttonpressed4.button4pressed = false;
            }
            if (Lights1[0].color  == green && Lights1[1].color == green && Lights1[2].color == green) {
                StartCoroutine(Puzzl1LevelChange2());
            }
        }
        if (Round == 2) {
            if (buttonpressed1.button1pressed == true) {
                for(int i = 0; i < 3; i++) {
                    if (Lights2[ChangeColor2[0, i]].color == green) Lights2[ChangeColor2[0, i]].color = red;
                    else Lights2[ChangeColor2[0, i]].color = green;
                    buttonpressed1.button1pressed = false;
                }
            }
            if (buttonpressed2.button2pressed == true) {
                for (int i = 0;i < 3;i++) {
                    if (Lights2[ChangeColor2[1, i]].color == green) Lights2[ChangeColor2[1, i]].color = red;
                    else Lights2[ChangeColor2[1, i]].color = green;
                    buttonpressed2.button2pressed = false;
                }
            }
            if (buttonpressed3.button3pressed == true) {
                for (int i = 0;i < 3; i++) {
                    if (Lights2[ChangeColor2[2,i]].color == green) Lights2[ChangeColor2[2, i]].color = red;
                    else Lights2[ChangeColor2[2, i]].color = green;
                    buttonpressed3.button3pressed = false;
                }
            }
            if (buttonpressed4.button4pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights2[ChangeColor2[3, i]].color == green) Lights2[ChangeColor2[3, i]].color = red;
                    else Lights2[ChangeColor2[3, i]].color = green;
                    buttonpressed4.button4pressed = false;
                }
            }
            if (Lights2[0].color  == green && Lights2[1].color == green && Lights2[2].color == green && Lights2[3].color == green && Lights2[4].color == green) {
                StartCoroutine(Puzzl1LevelChange3());
            }
        }
        if (Round == 3) {
            if (buttonpressed1.button1pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights3[ChangeColor3[0, i]].color == green) Lights3[ChangeColor3[0, i]].color = red;
                    else Lights3[ChangeColor3[0, i]].color = green;
                    buttonpressed1.button1pressed = false;
                }
            }
            if (buttonpressed2.button2pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights3[ChangeColor3[1, i]].color == green) Lights3[ChangeColor3[1, i]].color = red;
                    else Lights3[ChangeColor3[1, i]].color = green;
                    buttonpressed2.button2pressed = false;
                }
            }
            if (buttonpressed3.button3pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights3[ChangeColor3[2, i]].color == green) Lights3[ChangeColor3[2, i]].color = red;
                    else Lights3[ChangeColor3[2, i]].color = green;
                    buttonpressed3.button3pressed = false;
                }
            }
            if (buttonpressed4.button4pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights3[ChangeColor3[3, i]].color == green) Lights3[ChangeColor3[3, i]].color = red;
                    else Lights3[ChangeColor3[3, i]].color = green;
                    buttonpressed4.button4pressed = false;
                }
            }
            if (Lights3[0].color  == green && Lights3[1].color == green && Lights3[2].color == green && Lights3[3].color == green && Lights3[4].color == green&& Lights3[5].color == green && Lights3[6].color == green) {
                StartCoroutine(Puzzl1Clear());
            }
        }
    }

    public IEnumerator Puzzl1LevelChange2() {
        Round = 2;
        PlayerCtr.Controllable = false;
        StartCoroutine(PlayerCtr.CutSceneMove(-7));
        yield return new WaitForSeconds(2);
        Letterbox.LetterboxOn(150);
        StartCoroutine(Letterbox.SetBottomText("��� ���� �ʷϻ����� �ٲپ���!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        StartCoroutine(Letterbox.SetBottomText("�� ����� ������ ���̱� �����Ѵ�..."));
        for (int i = 0; i < Lights1.Length; i++) {
            StartCoroutine(lightsFadeIn(Lights1[i], 1));
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < Lights2.Length; i++) {
            StartCoroutine(lightsFadeOut(Lights2[i], 1));
        }
        StartCoroutine(Letterbox.ClearBottomText());
        Letterbox.LetterboxOff();
        PlayerCtr.Controllable = true;
    }

    public IEnumerator Puzzl1LevelChange3() {
        Round = 3;
        PlayerCtr.Controllable = false;
        StartCoroutine(PlayerCtr.CutSceneMove(-7));
        yield return new WaitForSeconds(2);
        Letterbox.LetterboxOn(150);
        StartCoroutine(Letterbox.SetBottomText("��� ���� �ʷϻ����� �ٲپ���!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        StartCoroutine(Letterbox.SetBottomText("���� ����� ������ ���̱� �����Ѵ�..."));
        for (int i = 0; i < Lights2.Length; i++) {
            StartCoroutine(lightsFadeIn(Lights2[i], 1));
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < Lights3.Length; i++) {
            StartCoroutine(lightsFadeOut(Lights3[i], 1));
        }
        StartCoroutine(Letterbox.ClearBottomText());
        Letterbox.LetterboxOff();
        PlayerCtr.Controllable = true;
    }

    public IEnumerator Puzzl1Clear() {
        Round = 0;
        PlayerCtr.Controllable = false;
        StartCoroutine(PlayerCtr.CutSceneMove(-7));
        yield return new WaitForSeconds(2);
        Letterbox.LetterboxOn(150);
        StartCoroutine(Letterbox.SetBottomText("��� ���� �ʷϻ����� �ٲپ���!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        StartCoroutine(Letterbox.SetBottomText("���� ����� ���ϴ� ���� ������!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        mySequence = DOTween.Sequence()
        .Append(Rightdoor.DOMoveX(3.4f, 2).SetEase(Ease.InQuart))
        .Join(Leftdoor.DOMoveX(-3.08f, 2).SetEase(Ease.InQuart));
        yield return new WaitForSeconds(1);
        Letterbox.LetterboxOff();
        GateControl.pass = true;
        PlayerCtr.Controllable = true;
    }

    public IEnumerator lightsFadeIn(SpriteRenderer lights, float Duration) {
        float initAlpha = lights.color.a;
        for (float t = Duration; t >= 0; t -= Time.deltaTime) {
            lights.color = new Color(0, 1, 0, t / Duration * initAlpha);
            yield return null;
        }
        lights.color = new Color(0, 1, 0, 0);
    }

    public IEnumerator lightsFadeOut(SpriteRenderer lights, float Duration) {
        float initAlpha = lights.color.a;
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            lights.color = new Color(1, 0, 0, initAlpha + t / Duration * (1-initAlpha));
            yield return null;
        }
        lights.color = new Color(1, 0, 0, 1);
    }
}
