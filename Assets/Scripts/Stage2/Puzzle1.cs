using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

public class Puzzle1 : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] Lights1;
    [SerializeField] private SpriteRenderer[] Lights2;
    [SerializeField] private SpriteRenderer[] Lights3;

    [SerializeField] private GameObject Player;
    [SerializeField] private buttonpress1 buttonpressed1;
    [SerializeField] private buttonpress2 buttonpressed2;
    [SerializeField] private buttonpress3 buttonpressed3;
    [SerializeField] private LetterboxController Letterbox;
    private PlayerController PlayerCtr;

    private Color red = Color.red;
    private Color green = Color.green;
    private int Round = 1;

    private static readonly int[] ChangeColor1 = new int[3] { 0, 2, 1 };    // 3개
    private static readonly int[,] ChangeColor2 = new int[3, 3]             // 5개
    {
        {0,1,2},
        {1,2,4},
        {2,3,4}
    };
    private static readonly int[,] ChangeColor3 = new int[3, 3]             // 7개
    {
        {0,2,3,5},
        {1,4,5},
        {1,6,7}
    };

    void Start()
    {
        PlayerCtr = GameObject.Find("Roo").GetComponent<PlayerController>();
        buttonpressed1 = GameObject.Find("button1").GetComponent<buttonpress1>();
        buttonpressed2 = GameObject.Find("button2").GetComponent<buttonpress2>();
        buttonpressed3 = GameObject.Find("button3").GetComponent<buttonpress3>();
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
            if (Lights2[0].color  == green && Lights2[1].color == green && Lights2[2].color == green && Lights2[3].color == green && Lights2[4].color == green) {
                StartCoroutine(Puzzl1LevelChange3());
            }
        }
        if (Round == 3) {
            if (buttonpressed1.button1pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights2[ChangeColor2[2, i]].color == green) Lights2[ChangeColor2[2, i]].color = red;
                    else Lights2[ChangeColor2[2, i]].color = green;
                    buttonpressed3.button3pressed = false;
                }
            }
            if (buttonpressed2.button2pressed == true) {
                if (Lights3[ChangeColor2[Round-1, 1]].color == green) Lights3[ChangeColor2[Round-1, 1]].color = red;
                else Lights3[ChangeColor2[Round-1, 1]].color = green;
                buttonpressed2.button2pressed = false;
            }
            if (buttonpressed3.button3pressed == true) {
                for (int i = 0; i < 3; i++) {
                    if (Lights3[ChangeColor2[Round-1, 2]].color == green) Lights3[ChangeColor2[Round-1, 2]].color = red;
                    else Lights3[ChangeColor2[Round-1, 2]].color = green;
                    buttonpressed3.button3pressed = false;
                }
            }
            if (Lights2[0].color  == green && Lights2[1].color == green && Lights2[2].color == green && Lights2[3].color == green && Lights2[4].color == green) {
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
        StartCoroutine(Letterbox.SetBottomText("모든 색을 초록색으로 바꾸었다!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        StartCoroutine(Letterbox.SetBottomText("더 어려운 퍼즐이 보이기 시작한다..."));
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
        StartCoroutine(Letterbox.SetBottomText("모든 색을 초록색으로 바꾸었다!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        StartCoroutine(Letterbox.SetBottomText("제일 어려운 퍼즐이 보이기 시작한다..."));
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
        Round = 3;
        PlayerCtr.Controllable = false;
        StartCoroutine(PlayerCtr.CutSceneMove(-7));
        yield return new WaitForSeconds(2);
        Letterbox.LetterboxOn(150);
        StartCoroutine(Letterbox.SetBottomText("모든 색을 초록색으로 바꾸었다!"));
        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
        StartCoroutine(Letterbox.SetBottomText("다음 퍼즐로 향하는 문이 열린다!"));
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
