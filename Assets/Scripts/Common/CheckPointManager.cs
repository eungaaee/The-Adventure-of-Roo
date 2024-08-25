using System.Collections;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    private bool InBoundary = false;
    private const float PressDuration = 2f;
    public bool IsSaved = false;
    private bool InitIsLetterboxOn;
    private float PressedTime = 0, PressedProportion = 0;

    private SpriteRenderer GlowRenderer;
    private LetterboxController Letterbox;
    private PlayerController Player;

    private void Awake() {
        GlowRenderer = transform.Find("Glow").GetComponent<SpriteRenderer>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        Player = GameObject.Find("Roo").GetComponent<PlayerController>();

        GlowRenderer.color = new Color(0, 0.9607843137254902f, 1, 0.2f);
    }

    private void Update() {
        if (!IsSaved & InBoundary) {
            if (Input.GetKey(KeyCode.Return)) {
                PressedTime += Time.deltaTime;
                if (PressedTime >= PressDuration) {
                    StartCoroutine(Glow());
                    StartCoroutine(Save());
                }
            } else if (PressedTime > 0) PressedTime -= Time.deltaTime;

            PressedProportion = Mathf.Floor(PressedTime/PressDuration*100)/100;
            if (PressedProportion > 0) Letterbox.EditTopText($"[ {PressedProportion*100}% ]");
            else Letterbox.EditTopText("");
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (!IsSaved & col.gameObject.CompareTag("Player")) {
            InBoundary = true;
            InitIsLetterboxOn = Letterbox.IsLetterboxOn;
            if (!InitIsLetterboxOn) Letterbox.LetterboxOn();
            StartCoroutine(Letterbox.SetBottomText("<sprite=0> 길게 눌러서 저장하기"));
            StartCoroutine(Letterbox.SetTopText(""));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (!IsSaved & col.gameObject.CompareTag("Player")) {
            InBoundary = false;
            if (!InitIsLetterboxOn) Letterbox.LetterboxOff();
            StartCoroutine(Letterbox.ClearTopText());
            StartCoroutine(Letterbox.ClearBottomText());
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
        Player.SetDefaultPos(transform.position + new Vector3(0, 0.5f, 0));
        StartCoroutine(Player.ResetCondition());
        yield return StartCoroutine(Letterbox.ClearBottomText());
        yield return StartCoroutine(Letterbox.SetTopText("진행도 저장 완료"));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(Letterbox.ClearTopText());
    }
}