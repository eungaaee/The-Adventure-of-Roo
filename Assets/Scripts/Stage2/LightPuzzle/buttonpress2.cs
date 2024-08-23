using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderData;

public class buttonpress2 : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private LetterboxController Letterbox;

    [SerializeField] private Sprite NormalSprite, ClickedSprite;

    [SerializeField] private Puzzle1 puzzle;
    private Rigidbody2D PlayerRigid;
    private BoxCollider2D buttoncolider;
    private SpriteRenderer sprRdr;

    public bool button2pressed = false;

    public void Start() {
        buttoncolider = GetComponent<BoxCollider2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            sprRdr.sprite = ClickedSprite;
            if (puzzle.Round > 0) button2pressed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            sprRdr.sprite = NormalSprite;
        }
    }

    /* public void Update() {
        if (buttoncolider.bounds.Contains(PlayerRigid.position) && Input.GetKeyDown(KeyCode.E)&& puzzle.Round != 0) {
            button2pressed = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == Player&& puzzle.Round != 0) {
            Letterbox.LetterboxOn(100);
            StartCoroutine(Letterbox.SetBottomText("��ư ������[E]"));
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == Player&& puzzle.Round != 0) {
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
        }
    } */
}
