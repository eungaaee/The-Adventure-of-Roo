using System.Collections;
using UnityEngine;

public class endcutscene : MonoBehaviour
{
    [SerializeField] private LogueController Logue;
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private Camera CameraObject;
    [SerializeField] private MainCameraController Cam;

    void Start(){
        Cam.enabled = false;
        StartCoroutine(FinalCutscene());
    }

    private IEnumerator FinalCutscene(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.BlackBoxOn());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetMonologue("그렇게해서"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("Roo는 시련을 극복하고 리더의 증표를 얻게되었고"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("마을로 돌아와 리더로써 인정받으며"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("마을을 다스렸어요."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("초대 리더정도 까진 아니었지만"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("카로타가 해준 리더의 수많은 이야기 덕분에"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.SetMonologue("마을을 어느정도 잘 다루게 되었답니다."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearMonologue());
        yield return StartCoroutine(Logue.BlackBoxOff());
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(Player.CutSceneMove(-16.24f));
        StartCoroutine(Player.CutSceneJump(7f));
        StartCoroutine(Player.CutSceneMove(-21.44f));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Scene.FadeOut());
        yield return StartCoroutine(Logue.SetMonologue("-The End-"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetMonologue("Thanks For Playing!"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetMonologue("Made by\n Prgrammer : 김동현, 이지원\nArt : 임정원\nSound : 김동하"));
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
