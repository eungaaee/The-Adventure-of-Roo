using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BreakableBlockController : MonoBehaviour {
    public float DdakDdak = 0f;
    public float ReGenerateInverval = 2f;
    public bool IsVibrate = false;
    private float Malang = 0;

    private BoxCollider2D col;
    private SpriteRenderer sprRdr;
    private MainCameraController CameraController;
    private ParticleSystem particle;

    void Awake() {
        col = GetComponent<BoxCollider2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            Malang += Time.deltaTime;
            if (IsVibrate) {
                StartCoroutine(CameraController.Shake(0.05f*Malang, 0.1f));
            }
            if (Malang >= DdakDdak) {
                GetComponent<BoxCollider2D>().enabled = false;
                sprRdr.color = new Color(1, 1, 1, 0);
                particle.Play();
                Invoke(nameof(breakstop), 0.8f);
                Invoke(nameof(ReGenerate), ReGenerateInverval);
            }
        }
    }
    private void breakstop() {
        particle.Stop();
    }
    private void ReGenerate() {
        GetComponent<BoxCollider2D>().enabled = true;
        sprRdr.color = new Color(1, 1, 1, 1);
        Malang = 0;
    }
}