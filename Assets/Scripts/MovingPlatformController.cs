using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour {
    [SerializeField] private float MoveTime = 3f;
    [SerializeField] private Vector3[] wayPoints;
    [SerializeField] private bool IsStyx = false;
    private bool styxMoved = false;
    private int wpLen;

    private void Awake() {
        wpLen = wayPoints.Length;
    }

    private void Start() {
        if (IsStyx) transform.position = wayPoints[0];
        else StartCoroutine(nameof(Loop));
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (IsStyx && !styxMoved && col.gameObject.CompareTag("Player")) {
            styxMoved = true;
            StartCoroutine(Move(wayPoints[0], wayPoints[1]));
        }
    }

    private IEnumerator Loop() {
        int curIdx = 0;
        while (true) {
            yield return StartCoroutine(Move(transform.position, wayPoints[curIdx]));
            curIdx++;
            if (curIdx == wpLen) curIdx = 0;
        }
    }

    private IEnumerator Move(Vector3 start, Vector3 end) {
        for (float t = 0; t < MoveTime; t += Time.deltaTime) {
            transform.position = Vector3.Lerp(start, end, t/MoveTime);
            yield return null;
        }
    }
}
