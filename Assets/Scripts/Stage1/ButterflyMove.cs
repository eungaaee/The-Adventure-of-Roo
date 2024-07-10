using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ButterflyMove : MonoBehaviour {
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float unitPerSecond = 1;
    [SerializeField]
    private bool isLoop;

    private int wayPointCount;
    private int currentIndex = 0;

    private void Awake() {
        wayPointCount = wayPoints.Length;
        StartCoroutine(nameof(Loop));
    }

    private IEnumerator Loop() {
        var wait = new WaitForSeconds(waitTime);

        while (true) {
            yield return StartCoroutine(MoveATOB(transform.position, wayPoints[currentIndex].position));
            currentIndex++;
            if (currentIndex == wayPointCount) currentIndex = 0;
            yield return wait;
        }
    }
    private IEnumerator MoveATOB(Vector3 start, Vector3 end) {
        float percent = 0;
        float moveTime = Vector3.Distance(start, end) / unitPerSecond; 

        while (percent < 1) {
            percent += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }
    }
    
}
