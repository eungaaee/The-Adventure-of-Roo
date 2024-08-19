using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.Unicode;
using static UnityEditor.ShaderData;

public class buttonpress4 : MonoBehaviour 
{
    [SerializeField] private GameObject Player;
    [SerializeField] private LetterboxController Letterbox;
    private Puzzle1 puzzle;
    private Rigidbody2D PlayerRigid;
    private BoxCollider2D buttoncolider;
    [SerializeField] public bool button4pressed = false;

    public void Start() {
        buttoncolider = GetComponent<BoxCollider2D>();
        PlayerRigid = Player.GetComponent<Rigidbody2D>();
        puzzle = GameObject.Find("button").GetComponent<Puzzle1>();
    }

    public void Update() {
        if (buttoncolider.bounds.Contains(PlayerRigid.position) && Input.GetKeyDown(KeyCode.E)&& puzzle.Round != 0) {
            button4pressed = true;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == Player && puzzle.Round != 0) {
            Letterbox.LetterboxOn(100);
            StartCoroutine(Letterbox.SetBottomText("��ư ������[E]"));
        }
    }
    public void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == Player && puzzle.Round != 0) {
            StartCoroutine(Letterbox.ClearBottomText());
            Letterbox.LetterboxOff();
        }
    }
}
