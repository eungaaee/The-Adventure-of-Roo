using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutScene1 : MonoBehaviour
{
    [SerializeField] private LogueController Logue;
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private Camera CameraObject;
    [SerializeField] private MainCameraController Cam;
    [SerializeField] private cutscene1MoveTrigger cutscene1Move;

    void Awake(){
        Cam.enabled = false;
    }
    void Start(){
        StartCoroutine(StartCutscene1());
    }
    
    void Update(){
        if(cutscene1Move.CutScene1MoveTriggerBool == true){
            cutscene1Move.gameObject.SetActive(false);
            cutscene1Move.CutScene1MoveTriggerBool = false;
            StartCoroutine(MoveCutscene1());
        }
    }

    private IEnumerator StartCutscene1() {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.BlackBoxOn());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetMonologue("옛날 옛적"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("어느 숲에는\n토끼들이 사는 토끼마을이 있었어요."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("이 토끼마을에는 모두를 이끄는 초대 리더가 있었고"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("그 누구보다 마을을 완벽하게 이끈 리더였어요."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("그래서 모두가 리더를 칭송했고 \n그 리더처럼 되고싶어 했죠."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("시간이 지나고 리더 토끼는\n다른 토끼에게 리더자리를 물려주고자 했지만"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("많은 토끼들이 리더의 자격을 얻는데 실패했고"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("리더는 세상을 떠나기 직전\n자신의 부하인 장로토끼에게 유언을 남겼죠."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("'리더의 증표를 얻는 토끼가 다음 리더가 될 것이다..'"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("이후의 장로토끼들은\n임시적으로 리더의 역할을 수행하며"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("토끼에게 자격을 부여하고\n증표를 얻을 수 있는 시험을 받게 했지만"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("증표를 얻은 토끼는 아무도 없었어요."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("그렇게 시간이 지나고...."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.BlackBoxOff());
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(Player.CutSceneMove(56.63f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Player.CutSceneMove(61.63f));
        yield return new WaitForSeconds(0.5f);
        Letterbox.LetterboxOn(100, 1.5f);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "좀 있으면 장로 토끼님이 이야기 하실텐데..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "빨리 가야겠다!"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "증표의 시련에 도전할 수 있는 나이가 됐으니"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "이제 장로 토끼님도 바로 수락하실거야!"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return StartCoroutine(Logue.DialogueBoxOff());
        Letterbox.LetterboxOff();
        Cam.enabled = true;
        CameraObject.orthographicSize = 6f;
        Player.SwitchControllable(true);
        Letterbox.LetterboxOn(80, 1.5f);
        StartCoroutine(Letterbox.SetTopText("'A/D' 또는 '←/→'로 좌우 이동 '스페이스 바'로 점프"));
        StartCoroutine(Letterbox.SetBottomText("왼쪽으로 이동해 장로토끼를 만나자"));
    }
    private IEnumerator MoveCutscene1(){
        Cam.enabled = false;
        Player.SwitchControllable(false);
        yield return CameraObject.transform.DOMove(new Vector3(-26.16f,-3.38f, -10),1).SetEase(Ease.Linear);
        yield return StartCoroutine(Letterbox.ClearBottomText());
        yield return StartCoroutine(Letterbox.ClearTopText());
        Letterbox.LetterboxOff();
        yield return StartCoroutine(Player.CutSceneMove(-22.2f, 3));
        yield return new WaitForSeconds(1);
        Letterbox.LetterboxOn(100, 1.5f);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "지금부터 토끼마을 월말 발표를 하도록 하겠네."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "대부분의 마을 지표들은 동일한 수준을 보이고 있고.."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "하아...증표의 시련으로 인한 피해자는 계속 속출하고 있네..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "그래서 다른 관리직들과 이야기해봤다만..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "조만간 증표의 시련을 봉쇄할 예정일세..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "....어?....에?!"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "또한 갑자기 돌게 된 역병으로 인해 매우 심각한 상황일세..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "우리 숲의 바로 옆인 위험한 숲에서"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "해독제를 구할 수 있단 결과를 구하긴 했지만..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "토끼 주민들이 가기 어려운 숲인 만큼"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "정예들을 뽑고 갈 생각일세..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "(~숲에 대한 위험성을 강조한다.)"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "마지막으로 역병에 이미 걸린 주민들은 자택에서 자가격리를 하게나.."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "가능한 빨리 해독제를 구해오도록 노력하겠네..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "아니...이게 더 심각한 얘기네..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "정예들을 뽑아 갈 시간을 생각하면"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "아무리 빨리 돌아와도 고통받는 사람은 생길거야..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "안되겠다...조금 위험하겠지만..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "나 혼자라도 숲에 들어가 해독제를 구해오는게"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "주민들을 위한 길이야..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "빨리 가자..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Player.CutSceneMove(-12.04f));
        yield return new WaitForSeconds(1);
        yield return Scene.LoadScene("Stage1");
    }   
}

