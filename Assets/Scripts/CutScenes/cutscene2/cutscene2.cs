using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutscene2 : MonoBehaviour
{

    [SerializeField] private LogueController Logue;
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private Camera CameraObject;
    [SerializeField] private MainCameraController Cam;
    // Start is called before the first frame update\
    
    void Awake(){
        Cam.enabled = false;
    }
    void Start()
    {
        StartCoroutine(StartCutscene2());
    }

    private IEnumerator StartCutscene2(){
        Player.SwitchControllable(false);
        yield return new WaitForSeconds(1);
        Letterbox.LetterboxOn(100, 1.5f);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "음 다들 모인것 같으니 긴급발표를 진행하겠네"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "우리마을의 토끼, 'Roo' 덕분에"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "해독제를 빠른시간안에 구할 수 있었고"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "그로 인해 역병으로 인한 사상자는 발생하지 않았네!"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "오..다행이다..."));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "다들 안심하고 역병에 대해 "));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("ElderBunny", "Roo는 나중에 나를 잠깐 보게\n긴급발표는 여기서 마치겠네"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "어 무슨 일이시지...?"));
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.DialogueBoxOff());
        Letterbox.LetterboxOff();
        yield return new WaitForSeconds(1);
        yield return Scene.LoadScene("Stage2");
    }
}
