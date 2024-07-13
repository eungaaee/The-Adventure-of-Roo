using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public Transform RooTransform;
    public LetterboxController Letterbox;
    public Vector2 MinCameraBoundary, MaxCameraBoundary;
    private Vector2 InitMinCameraBoundary, InitMaxCameraBoundary;
    private float FollowSpeed = 2.5f;
    private const float ShakeAmount = 0.25f, ShakeDuration = 0.25f;
    private float ZoomDuration, ZoomAmount;
    private float InitZoomSize;

    private IEnumerator MonoScope;
    private IEnumerator VibrateGenerator;

    void Awake() {
        RooTransform = GameObject.Find("Roo").GetComponent<Transform>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        InitZoomSize = Camera.main.orthographicSize;
        InitMinCameraBoundary = MinCameraBoundary;
        InitMaxCameraBoundary = MaxCameraBoundary;
    }

    void LateUpdate() {
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
            Camera.main.orthographicSize = Mathf.Lerp(InitOrthoSize, Lens, Mathf.Sin(0.5f*Mathf.PI * t/ZoomDuration));
            yield return null;
        }
    }

    public IEnumerator Shake(float Amount = ShakeAmount, float Duration = ShakeDuration) {
        if (VibrateGenerator != null) StopCoroutine(VibrateGenerator);
        Vector3 InitPos = transform.position;
        VibrateGenerator = Vibrate(InitPos, Amount, Duration);
        yield return StartCoroutine(VibrateGenerator);
        transform.position = InitPos;
    }

    private IEnumerator Vibrate(Vector3 InitPos, float Amount, float Duration) {
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            transform.position = InitPos + (Vector3)Random.insideUnitCircle * Amount;
            yield return null;
        }
    }
}