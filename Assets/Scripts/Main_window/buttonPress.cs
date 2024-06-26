using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonPress : MonoBehaviour
{
    public RectTransform topUI;  // 상단 UI
    public RectTransform bottomUI;  // 하단 UI
    public float moveSpeed = 10000000000000f;  // 움직임 속도
    private bool move = false;
    public int changeScene = 0;
    public TextMeshProUGUI text;  // TextMeshPro 텍스트 요소
    public float fadeDuration = 1.0f;  // 희미해지는 시간
    public float scaleDuration = 1.0f;
    private float minAlpha = 0.2f;  // 최소 알파 값
    private float maxAlpha = 1.0f;  // 최대 알파 값
    public SceneFader SceneFader;

    private void Start()
    {
        SceneFader = FindObjectOfType<SceneFader>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            move = true;
        }
        if (move)
        {
            StartCoroutine(FadeOutAndScaleText());
        }
        if (changeScene == 1)
        {
            SceneFader.LoadScene("cutScene1");
        }
        if (!move)
        {
            // Mathf.PingPong을 사용하여 알파 값 계산
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time / fadeDuration, 1.0f));
            Color textColor = text.color;
            textColor.a = alpha;
            text.color = textColor;
        }
    }
    private IEnumerator FadeOutAndScaleText()
    {
        float elapsedTime = 0f;
        Color originalColor = text.color;
        Vector3 originalScale = text.rectTransform.localScale;
        Vector3 targetScale = originalScale * 2f; // 1.5배 커지도록 설정

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // 알파 값 감소
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            float scale = Mathf.Lerp(1, 2f, elapsedTime / scaleDuration); // 크기 증가
            text.rectTransform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }
        changeScene = 1;
    }
}
