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

    private IEnumerator MonoScope;

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
    }

    public IEnumerator ZoomIn(Collider2D ZoomBoundary) {
        MinCameraBoundary = (Vector2)Variables.Object(ZoomBoundary).Get("MinZoomedCameraBoundary");
        MaxCameraBoundary = (Vector2)Variables.Object(ZoomBoundary).Get("MaxZoomedCameraBoundary");

        ZoomAmount = (float)Variables.Object(ZoomBoundary).Get("ZoomAmount");
        ZoomDuration = (float)Variables.Object(ZoomBoundary).Get("ZoomDuration");

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(InitZoomSize-ZoomAmount);
        yield return StartCoroutine(MonoScope);
    }

    public IEnumerator ZoomOut() {
        MinCameraBoundary = InitMinCameraBoundary;
        MaxCameraBoundary = InitMaxCameraBoundary;

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(InitZoomSize);
        yield return StartCoroutine(MonoScope);
    }

    private IEnumerator Scope(float Lens) {
        float InitOrthoSize = Camera.main.orthographicSize;
        for (float t = 0; t <= ZoomDuration; t += Time.deltaTime) {
            Camera.main.orthographicSize = Mathf.Lerp(InitOrthoSize, Lens, t/ZoomDuration);
            yield return null;
        }
    }

    public IEnumerator Shake() {
        Vector3 initPos = transform.position;
        for (float t = 0; t <= ShakeDuration; t += Time.deltaTime) {
            transform.position = (Vector3)Random.insideUnitCircle * ShakeAmount + initPos;
            t -= Time.deltaTime;
            yield return null;
        }
        transform.position = initPos;
    }
}