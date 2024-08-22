using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class GNButtonController : MonoBehaviour {
    [SerializeField] private bool isDecreaseButton, isIncreaseButton, isSubmitButton;

    [SerializeField] private GNPuzzle PuzzleScript;
    [SerializeField] private Sprite NormalSprite, ClickedSprite;
    private SpriteRenderer sprRdr;
    private BoxCollider2D buttonCollider, baseCollider;

    private const int INF = 0x3f3f3f3f;
    private float cooldown = 0.25f;
    private float lastPressed = -INF;
    private bool pressing;

    private void Awake() {
        sprRdr = GetComponent<SpriteRenderer>();
        buttonCollider = GetComponent<BoxCollider2D>();
        baseCollider = transform.Find("Base").GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (!PuzzleScript.isChecking && pressing && Time.time - lastPressed > cooldown) {
            lastPressed = Time.time;
            if (isDecreaseButton) PuzzleScript.DecreasePlayerAnswer();
            else if (isIncreaseButton) PuzzleScript.IncreasePlayerAnswer();
            else if (isSubmitButton) PuzzleScript.CorrectCheck();
        }

        if (PuzzleScript.isFinished) {
            ColliderOff();
            GetComponent<GNButtonController>().enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            pressing = true;
            sprRdr.sprite = ClickedSprite;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            pressing = false;
            lastPressed = -INF;
            sprRdr.sprite = NormalSprite;
        }
    }

    private void ColliderOff() {
        buttonCollider.enabled = baseCollider.enabled = false;
    }
}