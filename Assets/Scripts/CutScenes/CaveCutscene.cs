using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class CaveCutscene : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private SceneController Scene;
    [SerializeField] private MainCameraController Cam;

    [SerializeField] private GameObject CaveBorder;
    private BoxCollider2D TriggerCollider;

    private void Awake() {
        TriggerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (Player.IsReset & CaveBorder.activeSelf) {
            TriggerCollider.enabled = true;
            CaveBorder.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            TriggerCollider.enabled = false;
            StartCoroutine(StartCutscene());
        }
    }

    private IEnumerator StartCutscene() {
        Player.SwitchControllable(false);
        Letterbox.LetterboxOn(100, 2);
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(Player.CutSceneMove(59f, 3));
        yield return new WaitForSeconds(1);

        StartCoroutine(Player.CutSceneJump(4));
        StartCoroutine(Player.CutSceneMove(62, 4));
        yield return new WaitForSeconds(1);

        Cam.Zoom(3, 1);
        StartCoroutine(Player.CutSceneMove(65.5f, 0.8f));
        yield return new WaitForSeconds(2);
        StartCoroutine(Scene.FadeOut());
        yield return new WaitForSeconds(3);
        StartCoroutine(Scene.FadeIn());
        // Cam.CancelZoom(1, false);

        yield return new WaitForSeconds(1);
        StartCoroutine(Player.CutSceneJump(6));
        StartCoroutine(Player.CutSceneMove(68, 4));

        yield return new WaitForSeconds(2);
        CaveBorder.SetActive(true);
        Player.SwitchControllable(true);
    }
}