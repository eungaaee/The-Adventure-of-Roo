using UnityEngine;
using TMPro;
using System.Threading;
using System.Collections;

public class Timer : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
    [SerializeField] private LogueController Logue;
    [SerializeField] private TextMeshProUGUI TimerText;

    [SerializeField] private float remainingTime = 90f;
    private float initRemainingTime;
    private bool isRunning = false;
    private bool isTimeout = false;
    public bool triggered = false;

    private void Start() {
        initRemainingTime = remainingTime;
    }

    private void Update() {
        if (isRunning) {
            remainingTime -= Time.deltaTime;

            if (remainingTime < 0) {
                remainingTime = 0;
                StartCoroutine(TimeoutReset());
            }

            UpdateTimerText(remainingTime);
        }

        if (triggered & isRunning & !isTimeout & Player.IsReset) {
            isRunning = false;
            Player.SwitchControllable(false);
            HideTimer();
            StartCoroutine(StartTimer());
        }
    }

    private void UpdateTimerText(float time) {
        int seconds = Mathf.FloorToInt(time);
        int milliseconds = Mathf.FloorToInt(time%1 * 100);

        TimerText.text = $"{seconds:00}:{milliseconds:00}";
    }

    public void ResetTimer() {
        HideTimer();
        remainingTime = initRemainingTime;
        isRunning = false;
        UpdateTimerText(remainingTime);
    }

    public void ShowTimer() { TimerText.enabled = true; }

    public void HideTimer() { TimerText.enabled = false; }

    public IEnumerator StartTimer() {
        ResetTimer();

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.DialogueBoxOn());
        yield return StartCoroutine(Logue.SetDialogue("Roo", "?! 역시 해독제가..."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.SetDialogue("Roo", "빨리 마을로 돌아가야겠어"));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Logue.ClearDialogue());
        yield return StartCoroutine(Logue.DialogueBoxOff());
        yield return StartCoroutine(Letterbox.SetBottomText("해독제의 효능이 떨어지기 시작했다."));
        ShowTimer();

        yield return new WaitForSeconds(3);
        yield return StartCoroutine(Letterbox.SetBottomText("Go!"));

        Player.SwitchControllable(true);
        isRunning = true;

        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomText());
    }

    public IEnumerator TimeoutReset() {
        isRunning = false;
        isTimeout = true;
        if (!Letterbox.IsLetterboxOn) Letterbox.LetterboxOn(200);

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Letterbox.SetBottomText("해독제의 효능이 모두 떨어졌다...."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Letterbox.ClearBottomText());
        HideTimer();

        yield return new WaitForSeconds(1.5f);
        Player.SwitchControllable(false);
        Player.Life = 1;
        StartCoroutine(Player.Damaged(Player.transform.position));
        while (true) {
            if (Player.IsReset) break;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(StartTimer());
        isTimeout = false;
    }
}
