using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public Transform RooTransform;
    private float followSpeed = 2.5f;
    public Vector2 minCameraBoundary, maxCameraBoundary;

    private float ShakeDuration = 0.25f;
    private float ShakeAmount = 0.25f;

    void Awake() {
        RooTransform = GameObject.Find("Roo").GetComponent<Transform>();
    }

    void FixedUpdate() {
        Vector3 targetPos = new Vector3(RooTransform.position.x, RooTransform.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
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