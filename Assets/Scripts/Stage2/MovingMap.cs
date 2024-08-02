using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingMap : MonoBehaviour
{
    public string playerTag = "Player";
    public SceneController sceneController;
    public LetterboxController letterboxController;
    public PlayerController playerController;
    public GameObject[] entrance;

    private void Start() {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            sceneController.FadeIn();
            sceneController.FadeOut();
        }
    }
}
