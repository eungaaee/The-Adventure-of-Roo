using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMapMove : MonoBehaviour {
    public SceneController sceneController;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            sceneController.FadeOut();
        }
    }
}

