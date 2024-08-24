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

    private AudioSource Audio;
    [SerializeField] private AudioClip PressAudio;

    private SpriteRenderer sprRdr;

    public bool button2pressed = false;

    public void Start() {
        sprRdr = GetComponent<SpriteRenderer>();
        Audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            sprRdr.sprite = ClickedSprite;
            Audio.PlayOneShot(PressAudio);
            
            if (puzzle.Round > 0) button2pressed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            sprRdr.sprite = NormalSprite;
        }
    }
}
