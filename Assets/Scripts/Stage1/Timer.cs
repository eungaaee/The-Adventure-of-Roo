using UnityEngine;
using TMPro;
using System.Threading;

public class Timer : MonoBehaviour {
    public TextMeshProUGUI timerText;
    public float remainingTime = 90f;
    private bool running = false;
    private bool timerstart = false;

    void Start() {
        UpdateTimerText(remainingTime);
        timerText.enabled = false;
    }

    void Update() {
        if (timerstart && running) {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0) {
                remainingTime = 0;
            }
            UpdateTimerText(remainingTime);
        }
    }

    void UpdateTimerText(float time) {
        int seconds = Mathf.FloorToInt(time / 1);
        int milliseconds = Mathf.FloorToInt((time % 1) * 100);

        timerText.text = $"{seconds:00}:{milliseconds:00}";
    }

    public void OnPotionConsumed() {
        timerstart = true;
        running = true;
        ShowTimer();
    }

    public void StopTimer() {
        running = false;
    }

    public void StartTimer() {
        timerstart = true;
        running = true;
        ShowTimer();
    }
    

    public void ResetTimer() {
        remainingTime = 90f;
        timerstart = false;
        UpdateTimerText(remainingTime);
        HideTimer();
    }
    public void ShowTimer() {
        timerText.enabled = true;
    }
    public void HideTimer() {
        timerText.enabled = false;
    }
}
