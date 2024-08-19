using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class buttonpress2 : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private LetterboxController Letterbox;
    private Puzzle1 puzzle;
    private Rigidbody2D PlayerRigid;
    private BoxCollider2D buttoncolider;
    [SerializeField] public bool button2pressed = false;

    public void Start() {
        buttoncolider = GetComponent<BoxCollider2D>();
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
        puzzle = GameObject.Find("button").GetComponent<Puzzle1>();
    }

    public void Update() {
        if (buttoncolider.bounds.Contains(PlayerRigid.position) && Input.GetKeyDown(KeyCode.E)&& puzzle.Round != 0) {
            button2pressed = true;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == Player&& puzzle.Round != 0) {
            Letterbox.LetterboxOn(150);
            StartCoroutine(Letterbox.SetBottomText("버튼 누르기[E]"));
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == Player&& puzzle.Round != 0) {
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
        }
    }
}
