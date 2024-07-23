using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class PotionController : MonoBehaviour {
    private bool InBoundary = false;
    private bool PickedPotion = false, isPotionEnabled = false;

    [SerializeField] private GameObject Potion;
    [SerializeField] private PlayerController Player;
    [SerializeField] private GameObject[] CorruptedObjects;
    [SerializeField] private Timer timer;

    private LetterboxController Letterbox;
    private GameObject CheckpointStone;
    private GameObject HierarchyPotion;
    private SpriteRenderer PlayerSpriteRenderer;

    private void Awake() {
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        CheckpointStone = GameObject.Find("Checkpoint");
        PlayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (!PickedPotion && InBoundary && Input.GetKeyDown(KeyCode.F)) {
            PickedPotion = true;
            InBoundary = false;

            StartCoroutine(Letterbox.ClearBottomLetterboxText());
            CheckpointStone.GetComponent<BoxCollider2D>().enabled = true;
            StartCoroutine(SwitchCorruptedObjects());

            StartCoroutine(GetComponent<PotionCutscene>().StartCutscene());
        }

        if (PickedPotion & isPotionEnabled) SetPotionPos();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) {
            InBoundary = true;
            StartCoroutine(Letterbox.SetBottomLetterboxText("[F] 해독제 꺼내기"));
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) {
            InBoundary = false;
            StartCoroutine(Letterbox.ClearBottomLetterboxText());
        }
    }

    private IEnumerator SwitchCorruptedObjects() {
        int Length = CorruptedObjects.Length;
        for (int i = 0; i < Length; i++) {
            CorruptedObjects[i].SetActive(!CorruptedObjects[i].activeSelf);
            yield return null;
        }
    }

    public void InstantiatePotion() {
        HierarchyPotion = Instantiate(Potion);
        HierarchyPotion.transform.parent = Player.transform;
        isPotionEnabled = true;
    }

    private void SetPotionPos() {
        if (PlayerSpriteRenderer.flipX) HierarchyPotion.transform.localPosition = new Vector3(0.25f, -0.1f, 1);
        else HierarchyPotion.transform.localPosition = new Vector3(-0.25f, -0.1f, 1);
    }
}
