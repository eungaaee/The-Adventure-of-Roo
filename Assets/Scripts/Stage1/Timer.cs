using UnityEngine;
using TMPro;
using System.Threading;
using System.Collections;

public class Timer : MonoBehaviour {
    [SerializeField] private PlayerController Player;
    [SerializeField] private LetterboxController Letterbox;
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

            if (remainingTime <= 0) {
                remainingTime = 0;
                StartCoroutine(TimeoutReset());
            }

            UpdateTimerText(remainingTime);
        }
        
        if (triggered & !isTimeout & Player.IsResetting) {
            isRunning = false;
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
        Player.SwitchControllable(false);
        ResetTimer();

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Letterbox.SetBottomLetterboxText("해독제의 효능이 떨어지기 시작했다."));
        ShowTimer();

        yield return new WaitForSeconds(3);
        yield return StartCoroutine(Letterbox.SetBottomLetterboxText("Go!"));

        Player.SwitchControllable(true);
        isRunning = true;

        yield return new WaitForSeconds(2);
        StartCoroutine(Letterbox.ClearBottomLetterboxText());
    }

    public IEnumerator TimeoutReset() {
        isRunning = false;
        isTimeout = true;
        if (!Letterbox.IsLetterboxOn) Letterbox.LetterboxOn(200);

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Letterbox.SetBottomLetterboxText("해독제의 효능이 모두 떨어졌다...."));

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Letterbox.ClearBottomLetterboxText());
        HideTimer();

        yield return new WaitForSeconds(1.5f);
        Player.SwitchControllable(false);
        Player.Life = 1;
        StartCoroutine(Player.Damaged(Player.transform.position));
        while (true) {
            if (Player.IsResetting) break;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(StartTimer());
        isTimeout = false;
    }
}
