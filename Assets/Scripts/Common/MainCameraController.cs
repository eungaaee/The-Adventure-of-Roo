using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public Transform RooTransform;
    public LetterboxController Letterbox;
    public Vector2 MinCameraBoundary, MaxCameraBoundary;
    private Vector2 InitMinCameraBoundary, InitMaxCameraBoundary;
    public float FollowSpeed = 2.5f;
    private float ShakeDuration = 0.25f, ShakeAmount = 0.25f;
    private float ZoomDuration, ZoomAmount;
    private float InitZoomSize;

    void Awake() {
        RooTransform = GameObject.Find("Roo").GetComponent<Transform>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        InitZoomSize = Camera.main.orthographicSize;
        InitMinCameraBoundary = MinCameraBoundary;
        InitMaxCameraBoundary = MaxCameraBoundary;
    }

    void FixedUpdate() {
        Vector3 targetPos = new Vector3(RooTransform.position.x, RooTransform.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, MinCameraBoundary.x, MaxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, MinCameraBoundary.y, MaxCameraBoundary.y);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * FollowSpeed);

        if (RooTransform.position.x < transform.position.x - Camera.main.orthographicSize*Camera.main.aspect
            || RooTransform.position.x > transform.position.x + Camera.main.orthographicSize*Camera.main.aspect) {
            StopCoroutine(ZoomIn());
            StartCoroutine(ZoomOut());
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("zoomboundary")) {
            StartCoroutine(Letterbox.LetterboxOn());
            StartCoroutine(ZoomIn());
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("zoomboundary")) {
            StartCoroutine(Letterbox.LetterboxOff());
            StartCoroutine(ZoomOut());
        }
    }

    private IEnumerator ZoomIn() {
        MinCameraBoundary = (Vector2)Variables.Object(GameObject.Find("ZoomBoundary")).Get("MinZoomedCameraBoundary");
        MaxCameraBoundary = (Vector2)Variables.Object(GameObject.Find("ZoomBoundary")).Get("MaxZoomedCameraBoundary");

        ZoomAmount = (float)Variables.Object(GameObject.Find("ZoomBoundary")).Get("ZoomAmount");
        ZoomDuration = (float)Variables.Object(GameObject.Find("ZoomBoundary")).Get("ZoomDuration");

        float t = 0;
        while (t <= ZoomDuration) {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, InitZoomSize-ZoomAmount, t/ZoomDuration);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ZoomOut() {
        MinCameraBoundary = InitMinCameraBoundary;
        MaxCameraBoundary = InitMaxCameraBoundary;

        float t = 0;
        while (t <= ZoomDuration) {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, InitZoomSize, t/ZoomDuration);
            t += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator Shake() {
        float t = ShakeDuration;
        Vector3 initPos = transform.position;
        while (t > 0) {
            transform.position = (Vector3)Random.insideUnitCircle * ShakeAmount + initPos;
            t -= Time.deltaTime;
            yield return null;
        }
        transform.position = initPos;
    }
}