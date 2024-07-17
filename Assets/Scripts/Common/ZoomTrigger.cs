using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour {
    private Transform PlayerTransform;
    private MainCameraController CameraController;

    private float LeftBoundary, RightBoundary, BottomBoundary, TopBoundary;
    private bool IsEnter = false;
    private static int EnterCount = 0;
    private bool IsZoomed = false, IsCancelled = false;

    private void Awake() {
        PlayerTransform = GameObject.Find("Roo").GetComponent<Transform>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();

        LeftBoundary = transform.position.x - transform.lossyScale.x/2;
        RightBoundary = transform.position.x + transform.lossyScale.x/2;
        BottomBoundary = transform.position.y - transform.lossyScale.y/2;
        TopBoundary = transform.position.y + transform.lossyScale.y/2;
    }

    private void Update() {
        // Debug.Log($"IsEnter: {IsEnter}\nEnterCount: {EnterCount}\nIsZoomed: {IsZoomed}\nIsCancelled: {IsCancelled}");
        if (InBoundary()) {
            if (!IsEnter) {
                IsEnter = true;
                EnterCount++;
                IsCancelled = false;
            }
            if (!IsZoomed && EnterCount == 1) {
                IsZoomed = true;
                CameraController.Letterbox.LetterboxOn();
                CameraController.Zoom(gameObject);
            }
        } else if (!InBoundary()) {
            if (IsEnter) {
                IsEnter = false;
                EnterCount--;
                IsZoomed = false;
            }
            if (!IsCancelled && EnterCount == 0) {
                IsCancelled = true;
                CameraController.Letterbox.LetterboxOff();
                CameraController.CancelZoom(gameObject);
            }
        }
    }

    private bool InBoundary() {
        Vector2 PlayerPos = PlayerTransform.position;
        if (LeftBoundary <=  PlayerPos.x && PlayerPos.x <= RightBoundary && BottomBoundary <= PlayerPos.y && PlayerPos.y <= TopBoundary) return true;
        else return false;
    }
}
