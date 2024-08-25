using System.Collections;
using UnityEngine;

public class GateController : MonoBehaviour {
    private static readonly Vector2[,] cameraBoundary = new Vector2[4, 2] {
        {new Vector2(0.5f, -20f), new Vector2(0.5f, -20f)},
        {new Vector2(25.5f, -20f), new Vector2(25.5f, -20f)},
        {new Vector2(50.5f, -20f), new Vector2(50.5f, -20f)},
        {new Vector2(75.5f, -20f), new Vector2(75.5f, -20f)}
    };
    /* private static readonly Vector2[,] playerBoundary = new Vector2[3, 2] {
        {new Vector2(-10, -25), new Vector2(10, -14)},
        {new Vector2(15.76f, -25.05f), new Vector2(35.36f, -14.19f)},
        {new Vector2(40.23f, -25.68f), new Vector2(60.56f, -14.19f)}
    }; */

    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private MainCameraController Cam;
    [SerializeField] private GameObject Player;
    [SerializeField] private Camera CameraObject;
    [SerializeField] private SceneController SceneCtr;
    private BoxCollider2D GateCollider;
    private Rigidbody2D PlayerRigid;

    [SerializeField] private int targetMap = 0;
    [SerializeField] private GameObject targetGate;
    [SerializeField] private bool useCoordinate;
    [SerializeField] public bool PassedThisGate = false;
    [SerializeField] private Vector2 targetPos;

    private bool detectKey;

    private void Awake() {
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
        GateCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (detectKey && Input.GetKeyUp(KeyCode.E)) {
            GateCollider.enabled = false;
            PassedThisGate = true;
            StartCoroutine(EnterGate());
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == Player) {
            detectKey = true;
            Letterbox.LetterboxOn(100);
            StartCoroutine(Letterbox.SetBottomText("<sprite=17> 들어가기"));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject == Player) {
            detectKey = false;
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
        }
    }

    private IEnumerator EnterGate() {
        StartCoroutine(Letterbox.ClearBottomText());
        Letterbox.LetterboxOff();
        yield return StartCoroutine(SceneCtr.FadeOut());
        
        Cam.SetBoundary(cameraBoundary[targetMap, 0], cameraBoundary[targetMap, 1]);
        // PlayerCtr.SetBoundary(playerBoundary[targetMap, 0], playerBoundary[targetMap, 1]);
        CameraObject.orthographicSize = 6;

        if (useCoordinate) PlayerRigid.transform.position = targetPos;
        else Player.transform.position = targetGate.transform.position;
        
        yield return new WaitForSeconds(2);
        StartCoroutine(SceneCtr.FadeIn());
    }
}