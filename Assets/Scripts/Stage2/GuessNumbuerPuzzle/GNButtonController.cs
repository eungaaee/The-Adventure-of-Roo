using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class GNButtonController : MonoBehaviour {
    [SerializeField] private bool isIncreaseButton;

    [SerializeField] private GNPuzzle PuzzleScript;
    [SerializeField] private Sprite NormalSprite, ClickedSprite;
    private SpriteRenderer sprRdr;

    private const int INF = 0x3f3f3f3f;
    private float cooldown = 2;
    private float lastPressed;

    private void Awake() {
        sprRdr = GetComponent<SpriteRenderer>();    
    }

    private void Update() {
        if (Time.time - lastPressed > cooldown) {
            lastPressed = Time.time;
            if (isIncreaseButton) PuzzleScript.IncreasePlayerAnswer();
            else PuzzleScript.DecreasePlayerAnswer();
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            lastPressed = Time.time;
            sprRdr.sprite = ClickedSprite;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            lastPressed = INF;
            sprRdr.sprite = NormalSprite;
        }
    }
}
