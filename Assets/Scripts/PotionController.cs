using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class PotionController : MonoBehaviour {
    private bool InBoundary = false;
    private bool PickedPotion = false;
    public GameObject Potion;
    private Rigidbody2D PotionRigid;
    private CapsuleCollider2D PotionCollider;
    private SpriteRenderer PotionSprRdr;
    public MainCameraController CameraController;

    private void Awake() {
        Potion = transform.GetChild(0).gameObject;
        PotionRigid = Potion.GetComponent<Rigidbody2D>();
        PotionCollider = Potion.GetComponent<CapsuleCollider2D>();
        PotionSprRdr = Potion.GetComponent<SpriteRenderer>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
    }

    private void Update() {
        if (!PickedPotion && InBoundary && Input.GetKeyDown(KeyCode.F)) {
            StartCoroutine(Launcher());
            InBoundary = false;
            PickedPotion = true;
            // 컷씬시작
        }
        if (PotionRigid.velocity.y < 0) PotionCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) {
            InBoundary = true;
            StartCoroutine(CameraController.SetLetterboxText("[F] 물약 꺼내기"));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) InBoundary = false;
        StartCoroutine(CameraController.ClearLetterboxText());
    }

    private IEnumerator Launcher() {
        yield return new WaitForSecondsRealtime(1.5f);
        PotionRigid.simulated = true;
        PotionRigid.AddForce(new Vector2(-2.5f, 5), ForceMode2D.Impulse);
        PotionSprRdr.color = new Color(1, 1, 1, 1);
    }
}
