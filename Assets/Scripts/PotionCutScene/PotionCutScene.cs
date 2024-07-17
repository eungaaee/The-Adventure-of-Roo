using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCutScene : MonoBehaviour {
    void Start() {

    }

    void Update() {

    }

    public void EndCutScene() {
        StartCoroutine(GameObject.Find("SceneControlObject").GetComponent<SceneController>().UnloadScene("PotionCutScene"));
    }
}
