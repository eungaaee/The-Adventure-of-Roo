using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockController : MonoBehaviour {
    public Vector3[] wayPoints;
    public int unitPerSecond;
    private int wpLen;
    void Awake() {
        wpLen = wayPoints.Length;
        StartCoroutine(nameof(Loop));
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
        float percent = 0;
        float moveTime = Vector3.Distance(start, end) / unitPerSecond;

        while (percent < 1) {
            percent += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }
    }
}
