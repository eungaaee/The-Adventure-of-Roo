using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PotionCutscene : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private GameObject Potion;
    [SerializeField] private CheckpointManager Checkpoint;
    private MainCameraController CameraController;
    private LetterboxController Letterbox;
    private SpriteRenderer spriteRenderer, PlayerSpriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
    }

    public IEnumerator StartCutscene() {
        // 우물로 뛰어들 위치로 이동
        yield return new WaitForFixedUpdate();
        Player.SwitchControllable(false);
        Player.SetSpeed(4);
        yield return StartCoroutine(Player.CutSceneMove(123));

        // 루 방향 반전, 우물 레이어 맨 위로
        yield return new WaitForSeconds(1);
        yield return PlayerSpriteRenderer.flipX = !PlayerSpriteRenderer.flipX;
        spriteRenderer.sortingOrder = 10;

        // 줌
        CameraController.Zoom(1, 1, new Vector2(119, -4));

        // 우물로 뛰어들기
        yield return new WaitForSeconds(1);
        StartCoroutine(Player.CutSceneJump(7));
        yield return StartCoroutine(Player.CutSceneMove(120));

        // 물약과 함께 우물에서 나오기
        yield return new WaitForSeconds(2);
        CameraController.Shake(0.1f, 0.5f);
        yield return new WaitForSeconds(2);
        CameraController.Shake(0.1f, 0.8f);
        yield return new WaitForSeconds(2);
        GetComponent<PotionController>().InstantiatePotion();
        StartCoroutine(Player.CutSceneJump(7.5f));
        yield return StartCoroutine(Player.CutSceneMove(117));

        // 크게 보여주기
        yield return new WaitForSeconds(2);
        CameraController.Zoom(2, 1, new Vector2(117, -5));

        // 줌 취소 및 우물 레이어 원래대로
        yield return new WaitForSeconds(3);
        CameraController.CancelZoom(2);
        Letterbox.LetterboxOn(100);
        spriteRenderer.sortingOrder = 6;

        // 비석 보여주기
        yield return new WaitForSeconds(2);
        CameraController.Zoom(2, 1, new Vector2(114, -5));
        Letterbox.LetterboxOn(250);
        yield return new WaitForSeconds(1);
        Player.SetSpeed(2);
        StartCoroutine(Letterbox.SetBottomLetterboxText("[A]/[←] 비석 살펴보기"));

        // 이동 제한 해제, 카메라에 가두기
        yield return new WaitForSeconds(1);
        Player.SwitchControllable(true);
        Player.BindToCamera();
        
        // 저장 했는지 확인하기
        while (true) {
            if (Checkpoint.IsSaved) break;
            yield return null;
        }
        Player.ResetSpeed();
        Player.UnbindToCamera();
        CameraController.CancelZoom(2);
        yield return new WaitForSeconds(1);
        Letterbox.LetterboxOn(200);
    }
}   
