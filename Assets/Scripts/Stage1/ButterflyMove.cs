using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ButterflyMove : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private float waitTime;
    [SerializeField]
    private float unitPerSecond = 1;
    [SerializeField]
    private bool isPlayOnAwake = true;
    [SerializeField]
    private bool isLoop = true;

    private int wayPointCount;
    private int currentIndex = 0;

    private void Awake() {
        wayPointCount = wayPoints.Length;
        if (target == null) target = transform;
        if (isPlayOnAwake == true) Play();
    }

    public void Play() {
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process() {
        var wait = new WaitForSeconds(waitTime);

        while (true) {
            yield return StartCoroutine(MoveATOB(target.position, wayPoints[currentIndex].position));

            if (currentIndex < wayPointCount - 1) {
                currentIndex++;
            } 
            else 
            {
                if (isLoop == true) currentIndex = 0;
                else break;
            }

            yield return wait;
        }
    }
    private IEnumerator MoveATOB(Vector3 start, Vector3 end) {
        float percent = 0;
        float moveTime = Vector3.Distance(start, end) / unitPerSecond; 

        while (percent < 1) {
            percent += Time.deltaTime / moveTime;

            target.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }
    
}
