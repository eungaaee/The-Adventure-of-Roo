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
    private LetterboxController Letterbox;
    private GameObject CheckPointStone;
    private GameObject HierarchyPotion;
    private SpriteRenderer PlayerSpriteRenderer;
    private PlayerController PlayerC;
    private Timer timer;
    private SceneController SceneController;

    private void Awake() {
        Letterbox = GameObject.Find("Letterbox").GetComponent<LetterboxController>();
        CheckPointStone = GameObject.Find("CheckPoint");
        PlayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();
        SceneController = GameObject.Find("SceneControlObject").GetComponent<SceneController>();
    }

    private void Update() {
        if (!PickedPotion && InBoundary && Input.GetKeyDown(KeyCode.F)) {
            PickedPotion = true;
            InBoundary = false;

            StartCoroutine(Letterbox.ClearBottomLetterboxText());
            CheckPointStone.GetComponent<BoxCollider2D>().enabled = true;
            StartCoroutine(SwitchCorruptedObjects());

            StartCoroutine(GetComponent<PotionCutscene>().StartCutscene());
        }
        if (PickedPotion & isPotionEnabled) {
            if (PlayerSpriteRenderer.flipX) HierarchyPotion.transform.localPosition = new Vector3(0.25f, -0.1f, 1);
            else HierarchyPotion.transform.localPosition = new Vector3(-0.25f, -0.1f, 1);
        }
        if(PickedPotion & isPotionEnabled) {
            ResetPotion();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player") && !PickedPotion) {
            InBoundary = true;
            StartCoroutine(Letterbox.SetBottomLetterboxText("[F]로 해독제 꺼내기"));
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

    public void ResetPotion() {
        PlayerC = FindObjectOfType<PlayerController>();
        timer = FindObjectOfType<Timer>();
        if (PlayerC.Life == 0) {
            timer.ResetTimer();
        }
        if (timer.remainingTime == 0) {
            StartCoroutine(TimeoutReset());
        }
    }
    public IEnumerator TimeoutReset() {
        timer.HideTimer();
        timer.ResetTimer();
        PlayerC.rigid.velocity = Vector2.down;
        PlayerC.SwitchControllable(false);
        Letterbox.LetterboxOn(200);
        yield return new WaitForSeconds(1);
        StartCoroutine(Letterbox.SetBottomLetterboxText("해독제의 효능이 모두 떨어졌다...."));
        yield return new WaitForSeconds(1);
        StartCoroutine(Letterbox.ClearBottomLetterboxText());
        Letterbox.LetterboxOff();
        PlayerC.Life = 1;
        StartCoroutine(PlayerC.Damaged(PlayerC.transform.position));
    }
}
