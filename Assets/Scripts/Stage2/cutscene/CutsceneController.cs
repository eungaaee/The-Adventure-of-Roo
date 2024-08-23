using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private LogueController Logue;
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private Camera CameraObject;
    [SerializeField] private MainCameraController Cam;
    [SerializeField] private GateController Gate1;
    [SerializeField] private GateController Gate2;
    [SerializeField] private GateController Gate3;
    [SerializeField] private GateController Gate4;
    [SerializeField] private CarrotFace CarrotScr;
    [SerializeField] private SpriteRenderer CarrotSpr;
    [SerializeField] private SpriteRenderer StuckedCarrot;
    [SerializeField] private SpriteRenderer PlayerSpr;
    void Start(){
        Cam.enabled = false;
        StartCoroutine(Stage2Start());
    }
    void Update(){
        if(Gate1.PassedThisGate == true){
            Gate1.PassedThisGate = false;
            StartCoroutine(EnterFirstPuzzleScene());
        }
        if(Gate2.PassedThisGate == true){
            Gate2.PassedThisGate = false;
            StartCoroutine(EnterSecondPuzzleScene());
        }
        if(Gate3.PassedThisGate == true){
            Gate3.PassedThisGate = false;
            StartCoroutine(EnterThirdPuzzleScene());
        }
        if(Gate4.PassedThisGate == true){
            Gate4.PassedThisGate = false;
            StartCoroutine(EnterForthPuzzleScene());
        }
    }

    private IEnumerator Stage2Start(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(1);
        Letterbox.LetterboxOn(100, 1.5f);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "일단 다시한번 해독제를 구해준것에 감사함을 표하마"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "덕분에 사망자가 한명도 발생하지 않았어"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "그래서 곰곰히 무슨 보상을 줄지 생각해봤지만"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "한 번 당사자의 의견을 들어봐야겠다고 생각했네"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "혹시 원하는 것이 있나?"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "아! 혹시..."));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "증표의 시련이 봉쇄되기 전에\n한 번 도전해봐도 될까요....?"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "아실지는 모르겠지만 증표의 시련에\n도전할 수 있는 나이도 채웠고..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "?!...흐음..그걸로 충분하다면 알겠네..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "그나저나 특이하군.. 보상으로\n증표의 시련 도전을 얘기할줄은..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "저번에도 이야기 했지만 사상자가\n너무 많아 봉쇄를 결정한걸세..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "안에서 모든 임무를 완수해야만 밖으로 나올 수 있다는데"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "임무를 모두 수행하지 못해\n안에서 갇혀 죽은 토끼들이 한가득이라네..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "그래도 할거에요...계속 초대 리더를 동경했는걸요..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "...마을을 위한 마음은 리더로써 충분하다고 생각하네"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "그 마음으로 해독제를 구해와 주민들을 도와주었으니..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "시련은 오른쪽으로 쭉 가면 된다네..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "뭔가 자네는 할 수 있을 것만 같아..."));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return StartCoroutine(Logue.DialogueBoxOff());
        Letterbox.LetterboxOff();
        Player.SwitchControllable(true);
        yield return new WaitForSeconds(1);
        Cam.enabled = true;
    }

    private IEnumerator EnterFirstPuzzleScene(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(5f);
        Letterbox.LetterboxOn(100, 1.5f);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "여기가 증표의 시련을 받게되는 방인가...?"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.DialogueBoxOff());
        Letterbox.LetterboxOff();
        yield return new WaitForSeconds(1);
        yield return PlayerSpr.flipX = true;
        yield return new WaitForSeconds(0.5f);
        yield return PlayerSpr.flipX = false;
        yield return new WaitForSeconds(0.5f);
        yield return PlayerSpr.flipX = true;
        yield return new WaitForSeconds(1f);
        Cam.enabled = false;
        CameraObject.orthographicSize = 2;
        yield return CameraObject.gameObject.transform.DOMove(new Vector3(-7.68f, -24.47f, -10), 1);
        Letterbox.LetterboxOn(100, 1.5f);
        yield return Letterbox.SetBottomText("이건 뭐지?");
        yield return new WaitForSeconds(1f);
        yield return Letterbox.SetBottomText("일단 꺼내볼까...?");
        yield return new WaitForSeconds(1f);
        yield return StuckedCarrot.DOFade(0,1);
        yield return new WaitForSeconds(1f);
        CameraObject.gameObject.transform.position = new Vector3(-4.88f, -22.69f, -10);
        CameraObject.orthographicSize = 4;
        yield return new WaitForSeconds(1f);
        yield return CarrotSpr.DOFade(1,1);
        yield return Letterbox.SetTopText("??? : 어후 썩는줄 알았네...");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("??? : 그동안 들어온 토끼들은");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("??? : 도대체 날 왜 안뽑았던거지?");
        yield return new WaitForSeconds(2f);
        yield return CarrotScr.Carrot.sprite = CarrotScr.question;
        yield return Letterbox.SetTopText("??? : ? 넌 새로 들어온 토끼니?");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearTopText();
        yield return Letterbox.SetBottomText("어....어! 맞어");
        yield return new WaitForSeconds(2f);
        yield return CarrotScr.Carrot.sprite = CarrotScr.conceit;
        yield return Letterbox.SetTopText("카로타 : 난 카로타야!");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 내 파트너가 토끼들의 리더가 되고나서");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 여기서 토끼들 시련받는거나 안내해달라는데");
        yield return new WaitForSeconds(1.5f);
        yield return CarrotScr.Carrot.sprite = CarrotScr.hurt;
        yield return Letterbox.SetTopText("카로타 : 너무한거 아니야? 벌써 40년짼데");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 얘들이 내 안내는 무시하고");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 어떤 놈은 날 바위에다가 처박아뒀어!");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 힘만 드럽게 쎈 놈이었지...");
        yield return new WaitForSeconds(2f);
        yield return CarrotScr.Carrot.sprite = CarrotScr.suddenrealized;
        yield return Letterbox.SetTopText("카로타 : 아무튼! 이제 안내해줄게");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 시련은 총 3단계로");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 맨 마지막단계를 제외하면");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 각 시련들도 3레벨로 나뉘어");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 너는 이 모든 퍼즐을 풀고 나가면 돼!");
        yield return new WaitForSeconds(2f);
        yield return CarrotScr.Carrot.sprite = CarrotScr.happy;
        yield return Letterbox.SetTopText("카로타 : 못풀면...나랑 죽을때까지 말동무나 해야지");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearTopText();
        yield return Letterbox.SetBottomText("에...");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearBottomText();
        yield return CarrotScr.Carrot.sprite = CarrotScr.conceit;
        yield return Letterbox.SetTopText("카로타 : 참고로 너가 퍼즐을 풀면");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 나도 밖으로 나가서 자유를 만끽할 수 있어!");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 그러니 힘내라고 친구!");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearTopText();
        yield return Letterbox.SetBottomText("내가 못풀면 너도 한동안은 외톨이가 되겠네");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearBottomText();
        yield return CarrotScr.Carrot.sprite = CarrotScr.question;
        yield return Letterbox.SetTopText("카로타 : ? 왜?");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearTopText();
        yield return Letterbox.SetBottomText("장로토끼님이 이제 시련을 봉쇄한다고 하셨거든");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearBottomText();
        yield return CarrotScr.Carrot.sprite = CarrotScr.hurt;
        yield return Letterbox.SetTopText("카로타 : 헐...");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 또...외톨이가 되긴 싫어...");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.SetTopText("카로타 : 나..날 위해서라도 퍼즐을 클리어 해줘..");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearTopText();
        yield return Letterbox.SetBottomText("...노력해볼게...");
        yield return new WaitForSeconds(2f);
        yield return Letterbox.ClearBottomText();
        yield return CarrotScr.Carrot.sprite = CarrotScr.normal;
        CameraObject.gameObject.transform.position = new Vector3(0.5f, -20f, -10);
        CameraObject.orthographicSize = 6;
        yield return new WaitForSeconds(2f);
        Letterbox.LetterboxOff();
        Cam.enabled = false;
        Player.SwitchControllable(true);
    }

    private IEnumerator EnterSecondPuzzleScene(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(3f);
        CarrotScr.gameObject.transform.position = new Vector3(17.1f,-22.34f,0);
        yield return new WaitForSeconds(2f);
        Cam.enabled = false;
    }
    private IEnumerator EnterThirdPuzzleScene(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(3f);
        CarrotScr.gameObject.transform.position = new Vector3(42f,-23.39f,0);
        yield return new WaitForSeconds(2f);
        Cam.enabled = false;
    }
    private IEnumerator EnterForthPuzzleScene(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(3f);
        CarrotScr.gameObject.transform.position = new Vector3(78.27f,-22.95f,0);
        yield return new WaitForSeconds(2f);
        Cam.enabled = false;
    }
}
