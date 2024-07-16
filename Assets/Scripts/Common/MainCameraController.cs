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
    private float FollowSpeed = 2.5f;
    private const float ShakeAmount = 0.25f, ShakeDuration = 0.25f;
    private float DefaultZoomSize;

    private IEnumerator MonoScope;
    private IEnumerator VibrateGenerator;

    void Awake() {
        RooTransform = GameObject.Find("Roo").GetComponent<Transform>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();

        InitMinCameraBoundary = MinCameraBoundary;
        InitMaxCameraBoundary = MaxCameraBoundary;
        DefaultZoomSize = Camera.main.orthographicSize;
    }

    void LateUpdate() {
        Vector3 targetPos = new Vector3(RooTransform.position.x, RooTransform.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, MinCameraBoundary.x, MaxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, MinCameraBoundary.y, MaxCameraBoundary.y);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * FollowSpeed);
    }

    public IEnumerator Zoom(GameObject ZoomBoundary) {
        MinCameraBoundary = (Vector2)Variables.Object(ZoomBoundary).Get("MinZoomedCameraBoundary");
        MaxCameraBoundary = (Vector2)Variables.Object(ZoomBoundary).Get("MaxZoomedCameraBoundary");

        float ZoomAmount = (float)Variables.Object(ZoomBoundary).Get("ZoomAmount");
        float ZoomDuration = (float)Variables.Object(ZoomBoundary).Get("ZoomDuration");

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize-ZoomAmount, ZoomDuration);
        yield return StartCoroutine(MonoScope);
    }

    public IEnumerator CancelZoom(GameObject ZoomBoundary) {
        MinCameraBoundary = InitMinCameraBoundary;
        MaxCameraBoundary = InitMaxCameraBoundary;

        float CancelDuration = (float)Variables.Object(ZoomBoundary).Get("ZoomDuration");

        if (MonoScope != null) StopCoroutine(MonoScope);
        MonoScope = Scope(DefaultZoomSize, CancelDuration);
        yield return StartCoroutine(MonoScope);
    }

    private IEnumerator Scope(float Lens, float Duration) {
        float InitOrthoSize = Camera.main.orthographicSize;
        for (float t = 0; t <= Duration; t += Time.deltaTime) {
            Camera.main.orthographicSize = Mathf.Lerp(InitOrthoSize, Lens, Mathf.Sin(0.5f*Mathf.PI * t/Duration));
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