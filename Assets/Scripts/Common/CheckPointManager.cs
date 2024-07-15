using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPointManager : MonoBehaviour {
    private bool InBoundary = false;
    private float PressDuration = 0f;
    private bool IsSaved = false;
    private bool InitIsLetterboxOn;

    private SpriteRenderer GlowRenderer;
    private LetterboxController Letterbox;
    private PlayerController Player;

    private void Awake() {
        GlowRenderer = transform.Find("Glow").GetComponent<SpriteRenderer>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        Player = GameObject.Find("Roo").GetComponent<PlayerController>();

        GlowRenderer.color = new Color(0, 0.9607843137254902f, 1, 0.2f);
    }

    void Update() {
        if (!IsSaved & InBoundary) {
            if (Input.GetKey(KeyCode.Return)) {
                PressDuration += Time.deltaTime;
                if (PressDuration > 2f) {
                    StartCoroutine(Glow());
                    StartCoroutine(Save());
                }
            } else if (PressDuration > 0) {
                PressDuration -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (!IsSaved & col.gameObject.CompareTag("Player")) {
            InBoundary = true;
            InitIsLetterboxOn = Letterbox.IsLetterboxOn;
            if (!InitIsLetterboxOn) StartCoroutine(Letterbox.LetterboxOn());
            StartCoroutine(Letterbox.SetLetterboxText("[Enter] 길게 눌러서 저장하기", 1));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (!IsSaved & col.gameObject.CompareTag("Player")) {
            InBoundary = false;
            if (!InitIsLetterboxOn) StartCoroutine(Letterbox.LetterboxOff());
            StartCoroutine(Letterbox.ClearLetterboxText(1));
        }
    }

    private IEnumerator Glow() {
        float Duration = 1.5f;
        for (int i = 0; i < 2; i++) {
            for (float t = 0; t < Duration; t += Time.deltaTime) yield return GlowRenderer.color = new Color(0, 0.9607843137254902f, 1, Mathf.Lerp(0.2f, 1, t/Duration));
            for (float t = 0; t < Duration; t += Time.deltaTime) yield return GlowRenderer.color = new Color(0, 0.9607843137254902f, 1, Mathf.Lerp(1, 0.2f, t/Duration));
        }
        for (float t = 0; t < 1f; t += Time.deltaTime) yield return GlowRenderer.color = new Color(0, 0.9607843137254902f, 1, Mathf.Lerp(0.2f, 1, t/1f));
    }

    private IEnumerator Save() {
        IsSaved = true;
        Player.InitPos = transform.position;
        // Player.ResetStats();
        
        yield return StartCoroutine(Letterbox.ClearLetterboxText(1));
        yield return StartCoroutine(Letterbox.SetLetterboxText("진행도 저장 완료", 0));
        yield return new WaitForSecondsRealtime(3f);
        yield return StartCoroutine(Letterbox.ClearLetterboxText(0));
    }
}