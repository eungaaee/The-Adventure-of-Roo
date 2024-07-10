/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    float elapsedTime = 0;
    void Start() {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = Vector2.Lerp(transform.position, )
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 2) {
            Debug.Log(Camera.main.transform.position);
            elapsedTime = 0;
        }
    }
}
 */