using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class PotionController : MonoBehaviour {
    private bool InBoundary = false;
    private bool PickedPotion = false;
    public GameObject Potion;
    private Rigidbody2D PotionRigid;
    private CapsuleCollider2D PotionCollider;
    private SpriteRenderer PotionSprRdr;
    private LetterboxController Letterbox;
    private SceneController SceneController;
    private GameObject CheckPointStone;
    public GameObject[] CorruptedObjects;

    private void Awake() {
        Potion = transform.GetChild(0).gameObject;
        PotionRigid = Potion.GetComponent<Rigidbody2D>();
        PotionCollider = Potion.GetComponent<CapsuleCollider2D>();
        PotionSprRdr = Potion.GetComponent<SpriteRenderer>();
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        SceneController = GameObject.Find("SceneControlObject").GetComponent<SceneController>();
        CheckPointStone = GameObject.Find("CheckPoint");
    }

    private void Update() {
        if (!PickedPotion && InBoundary && Input.GetKeyDown(KeyCode.F)) {
            // StartCoroutine(Launcher());
            PickedPotion = true;
            InBoundary = false;
            StartCoroutine(Letterbox.ClearLetterboxText());
            // CheckPointStone.GetComponent<BoxCollider2D>().enabled = true;
            StartCoroutine(SwitchCorruptedObjects());
            StartCoroutine(SceneController.LoadCutScene("PotionCutScene"));
        }
        // if (PotionRigid.velocity.y < 0) PotionCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) {
            InBoundary = true;
            StartCoroutine(Letterbox.SetLetterboxText("[F] 물약 꺼내기", 1));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) {
            InBoundary = false;
            StartCoroutine(Letterbox.ClearLetterboxText(1));
        }
    }

    private IEnumerator Launcher() {
        yield return new WaitForSecondsRealtime(1.5f);
        PotionRigid.simulated = true;
        PotionRigid.AddForce(new Vector2(-2.5f, 5), ForceMode2D.Impulse);
        PotionSprRdr.color = new Color(1, 1, 1, 1);
    }

    private IEnumerator SwitchCorruptedObjects() {
        int Length = CorruptedObjects.Length;
        for (int i = 0; i < Length; i++) {
            CorruptedObjects[i].SetActive(!CorruptedObjects[i].activeSelf);
            yield return null;
        }
    }
}
