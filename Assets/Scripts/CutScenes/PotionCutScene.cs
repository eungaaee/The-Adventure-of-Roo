using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PotionCutScene : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private GameObject Potion;
    private MainCameraController CameraController;
    private SpriteRenderer spriteRenderer, PlayerSpriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
    }

    public IEnumerator StartCutScene() {
        // 우물로 뛰어들 위치로 이동
        int d = Player.transform.position.x < transform.position.x ? 1 : -1;
        yield return StartCoroutine(Player.CutSceneMove(d, 123));

        // 루 방향 반전, 우물 레이어 맨 위로
        yield return new WaitForSeconds(1);
        yield return PlayerSpriteRenderer.flipX = !PlayerSpriteRenderer.flipX;
        spriteRenderer.sortingOrder = 10;

        // 줌
        CameraController.Zoom(4, 1);

        // 우물로 뛰어들기
        yield return new WaitForSeconds(1);
        StartCoroutine(Player.CutSceneJump(7));
        StartCoroutine(Player.CutSceneMove(-1*d, 120));

        // 물약과 함께 우물에서 나오기
        yield return new WaitForSeconds(3);
        GetComponent<PotionController>().InstantiatePotion();
        StartCoroutine(Player.CutSceneJump(7));
        yield return StartCoroutine(Player.CutSceneMove(-1*d, 117));
        Player.SwitchControllable(true);

        // 줌 취소
        CameraController.Zoom(3, 1);

        // 컷씬 종료
        spriteRenderer.sortingOrder = 6;
    }
}
