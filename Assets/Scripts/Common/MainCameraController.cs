using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraController : MonoBehaviour {
    public Transform RooTransform;
    public LetterboxController Letterbox;
    public Vector2 MinCameraBoundary, MaxCameraBoundary;
    private Vector2 InitMinCameraBoundary, InitMaxCameraBoundary;
    private Vector2 CurMinCameraBoundary, CurMaxCameraBoundary;
    private float FollowSpeed = 2.5f;
    private const float ShakeAmount = 0.25f, ShakeDuration = 0.25f;
    private float DefaultZoomSize, CurZoomAmount;

    private IEnumerator MonoScope;
    private IEnumerator VibrateGenerator;

    private void Awake() {
        RooTransform = GameObject.Find("Roo").GetComponent<Transform>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();

        InitMinCameraBoundary = MinCameraBoundary;
        InitMaxCameraBoundary = MaxCameraBoundary;
        DefaultZoomSize = Camera.main.orthographicSize;
    }

    private void LateUpdate() {
        Vector3 targetPos = new Vector3(RooTransform.position.x, RooTransform.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, MinCameraBoundary.x, MaxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, MinCameraBoundary.y, MaxCameraBoundary.y);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * FollowSpeed);
    }

    // Zoom with ZoomBoundary
    public void Zoom(GameObject ZoomBoundary) {
        SetBoundary((Vector2)Variables.Object(ZoomBoundary).Get("MinZoomedCameraBoundary"),
            (Vector2)Variables.Object(ZoomBoundary).Get("MaxZoomedCameraBoundary"));

        float Amount = (float)Variables.Object(ZoomBoundary).Get("ZoomAmount");
        float Duration = (float)Variables.Object(ZoomBoundary).Get("ZoomDuration");

        // for Cancel Instant Zoom
        CurMinCameraBoundary = MinCameraBoundary;
        CurMaxCameraBoundary = MaxCameraBoundary;
        CurZoomAmount = Amount;

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize-Amount, Duration);
        StartCoroutine(MonoScope);
    }

    // Instant Zoom
    public void Zoom(float Amount, float Duration) {
        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize-CurZoomAmount-Amount, Duration);
        StartCoroutine(MonoScope);
    }

    // Instant Zoom with CameraPosition
    public void Zoom(float Amount, float Duration, Vector2 CameraPosition) {
        SetBoundary(CameraPosition, CameraPosition);

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize-CurZoomAmount-Amount, Duration);
        StartCoroutine(MonoScope);
    }

    // Cancel Zoom with ZoomBoundary
    public void CancelZoom(GameObject ZoomBoundary) {
        SetBoundary(InitMinCameraBoundary, InitMaxCameraBoundary);

        float Duration = (float)Variables.Object(ZoomBoundary).Get("ZoomDuration");

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize, Duration);
        StartCoroutine(MonoScope);
    }

    // Cancel Instant Zoom
    public void CancelZoom(float Duration, bool resetCameraPosition = true) {
        if (resetCameraPosition) SetBoundary(CurMinCameraBoundary, CurMaxCameraBoundary);

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize-CurZoomAmount, Duration);
        StartCoroutine(MonoScope);
    }

    private IEnumerator Scope(float Lens, float Duration) {
        float InitOrthoSize = Camera.main.orthographicSize;
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            Camera.main.orthographicSize = Mathf.Lerp(InitOrthoSize, Lens, Mathf.Sin(0.5f*Mathf.PI * t/Duration));
            yield return null;
        }
    }

    public void Shake(float Amount = ShakeAmount, float Duration = ShakeDuration) {
        if (VibrateGenerator != null) StopCoroutine(VibrateGenerator);
        Vector3 InitPos = transform.position;
        VibrateGenerator = Vibrate(InitPos, Amount, Duration);
        StartCoroutine(VibrateGenerator);
    }

    private IEnumerator Vibrate(Vector3 InitPos, float Amount, float Duration) {
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            transform.position = InitPos + (Vector3)Random.insideUnitCircle * Amount;
            yield return null;
        }
        transform.position = InitPos;
    }

    public void SetBoundary(Vector2 minBoundary, Vector2 maxBoundary) {
        MinCameraBoundary = minBoundary;
        MaxCameraBoundary = maxBoundary;
    }
}