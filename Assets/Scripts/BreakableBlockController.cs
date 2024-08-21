using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BreakableBlockController : MonoBehaviour {
    [SerializeField] private float DdakDdak = 0f;
    [SerializeField] private float ReGenerateInterval = 2f;
    [SerializeField] private bool IsVibrate = false;
    [SerializeField] private bool IsInteractive = false;
    [SerializeField] private int InteractiveTapCount = 10;
    [SerializeField] private bool InteractiveNoti = false;
    private int InitInteractiveTapCount;
    private float Malang = 0;
    private bool DetectKey = false;

    private BoxCollider2D col;
    private SpriteRenderer sprRdr;
    private MainCameraController CameraController;
    private LetterboxController Letterbox;
    private ParticleSystem[] Particles;
    private PlayerController Player;

    private void Awake() {
        col = GetComponent<BoxCollider2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        Particles = GetComponentsInChildren<ParticleSystem>();
        Player = GameObject.Find("Roo").GetComponent<PlayerController>();
        InitInteractiveTapCount = InteractiveTapCount;
    }

    private void Update() {
        if (IsInteractive && DetectKey && Input.GetKeyDown(KeyCode.F)) {
            InteractiveTapCount--;
            Malang += DdakDdak / InitInteractiveTapCount;
        } else if (!IsInteractive && DetectKey && Input.GetKeyDown(KeyCode.Space)) {
            Malang += 1.5f;
        }

        if (Player.IsResetting) ReGenerate();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            DetectKey = true;
            if (IsInteractive && InteractiveNoti)
                StartCoroutine(Letterbox.SetBottomText("<sprite=31> 벽 부수기"));
            Particles[0].Play();
        }
    }

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            if (!IsInteractive) Malang += Time.deltaTime;

            if (IsVibrate) CameraController.Shake(Mathf.Clamp(0.025f*Malang, 0, 0.075f), 0.1f);

            if (Malang >= DdakDdak || InteractiveTapCount <= 0) {
                GetComponent<BoxCollider2D>().enabled = false;
                sprRdr.color = new Color(1, 1, 1, 0);
                Particles[1].Play();
                Invoke(nameof(ReGenerate), ReGenerateInterval);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            DetectKey = false;
            if (IsInteractive && InteractiveNoti)
                StartCoroutine(Letterbox.ClearBottomText());
            Particles[0].Stop();
        }
    }

    private void ReGenerate() {
        GetComponent<BoxCollider2D>().enabled = true;
        sprRdr.color = new Color(1, 1, 1, 1);
        Malang = 0;
        InteractiveTapCount = InitInteractiveTapCount;
    }
}