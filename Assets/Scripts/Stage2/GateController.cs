using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {
    private static readonly Vector2[,] cameraBoundary = new Vector2[3, 2] {
        {new Vector2(-0.5f, -20.5f), new Vector2(1.7f, -19.5f)},
        {new Vector2(-5.5f, -20.5f), new Vector2(5.5f, -19.5f)},
        {new Vector2(-5.5f, -20.5f), new Vector2(5.5f, -19.5f)}
    };
    private static readonly Vector2[,] playerBoundary = new Vector2[3, 2] {
        {new Vector2(-10, -25), new Vector2(10, -14)},
        {new Vector2(15, -25), new Vector2(35, -14)},
        {new Vector2(40, -25), new Vector2(65, -14)}
    };

    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private MainCameraController Cam;
    [SerializeField] private GameObject Player;
    [SerializeField] private SceneController SceneCtr;
    private Rigidbody2D PlayerRigid;
    private PlayerController PlayerCtr;
    private BoxCollider2D GateCollider;

    [SerializeField] private int targetMap;
    [SerializeField] private GameObject targetGate;
    [SerializeField] private bool useCoordinate;
    [SerializeField] private Vector2 targetPos;

    private void Awake() {
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
        PlayerCtr = Player.GetComponent<PlayerController>();
        GateCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (GateCollider.bounds.Contains(PlayerRigid.position) && Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(EnterGate());
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == Player) {
            Letterbox.LetterboxOn(150);
            StartCoroutine(Letterbox.SetBottomText("[E] 들어가기"));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject == Player) {
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
        }
    }

    private IEnumerator EnterGate() {
        StartCoroutine(Letterbox.ClearBottomText());
        Letterbox.LetterboxOff();

        yield return StartCoroutine(SceneCtr.FadeOut());

        Cam.SetBoundary(cameraBoundary[targetMap, 0], cameraBoundary[targetMap, 1]);
        PlayerCtr.SetBoundary(playerBoundary[targetMap, 0], playerBoundary[targetMap, 1]);

        if (useCoordinate) PlayerRigid.transform.position = targetPos;
        else Player.transform.position = targetGate.transform.position;

        yield return new WaitForSeconds(2);
        StartCoroutine(SceneCtr.FadeIn());
    }
}