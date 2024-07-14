using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BreakableBlockController : MonoBehaviour {
    public float DdakDdak = 0f;
    public float ReGenerateInterval = 2f;
    public bool IsVibrate = false;
    public bool IsInteractive = false;
    public int InteractiveTapCount = 10;
    private int InitInteractiveTapCount;
    private float Malang = 0;
    private bool DetectKey = false;

    private BoxCollider2D col;
    private SpriteRenderer sprRdr;
    private MainCameraController CameraController;
    private ParticleSystem[] Particles;

    void Awake() {
        col = GetComponent<BoxCollider2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        Particles = GetComponentsInChildren<ParticleSystem>();
        InitInteractiveTapCount = InteractiveTapCount;
    }

    void Update() {
        if (IsInteractive && DetectKey && Input.GetKeyDown(KeyCode.F)) {
            InteractiveTapCount--;
            Malang += DdakDdak / InitInteractiveTapCount;
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            DetectKey = true;
            Particles[0].Play();
        }
    }

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            if (!IsInteractive) Malang += Time.deltaTime;
            // else if (IsInteractive && DetectKey && Input.GetKeyDown(KeyCode.F)) InteractiveTapCount--;

            if (IsVibrate) StartCoroutine(CameraController.Shake(Mathf.Clamp(0.05f*Malang, 0, 0.1f), 0.1f));
            
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