using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public Transform RooTransform;
    public GameObject ZoomBoundary;

    public Vector2 minCameraBoundary, maxCameraBoundary;
    public float FollowSpeed = 2.5f;
    private float ShakeDuration = 0.25f;
    private float ShakeAmount = 0.25f;
    private float ZoomSpeed;
    private float ZoomAmount;
    private float InitZoomSize;
    private Vector2 InitCameraBoundary;

    void Awake() {
        RooTransform = GameObject.Find("Roo").GetComponent<Transform>();
        ZoomBoundary = GameObject.Find("ZoomBoundary");
        InitZoomSize = Camera.main.orthographicSize;
        InitCameraBoundary = maxCameraBoundary;
    }

    void Update() {
        Vector2 ZBPosition = ZoomBoundary.GetComponent<Transform>().position;
        Vector2 ZBScale = ZoomBoundary.GetComponent<Transform>().lossyScale;
        if (transform.position.x >= ZBPosition.x - ZBScale.x/2 
            && transform.position.x <= ZBPosition.x + ZBScale.x/2) ZoomIn();
        else ZoomOut();
    }

    void FixedUpdate() {
        Vector3 targetPos = new Vector3(RooTransform.position.x, RooTransform.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * FollowSpeed);
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

    public void ZoomIn() {
        maxCameraBoundary.y = InitCameraBoundary.y + ((Vector2)Variables.Object(GameObject.Find("ZoomBoundary")).Get("adjustCameraBoundary")).y;

        ZoomAmount = (float)Variables.Object(GameObject.Find("ZoomBoundary")).Get("ZoomAmount");
        ZoomSpeed = (float)Variables.Object(GameObject.Find("ZoomBoundary")).Get("ZoomSpeed");
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, InitZoomSize - ZoomAmount, Time.deltaTime*ZoomSpeed);
    }

    public void ZoomOut() {
        maxCameraBoundary.y = InitCameraBoundary.y;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, InitZoomSize, Time.deltaTime*ZoomSpeed);
    }
}