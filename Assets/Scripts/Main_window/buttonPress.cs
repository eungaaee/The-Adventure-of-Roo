using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonPress : MonoBehaviour
{
    public float moveSpeed = 10000000000000f;  // ������ �ӵ�
    private bool move = false;
    public int changeScene = 0;
    public TextMeshProUGUI text;  // TextMeshPro �ؽ�Ʈ ���
    public float fadeDuration = 1.0f;  // ��������� �ð�
    public float scaleDuration = 1.0f;
    private float minAlpha = 0.2f;  // �ּ� ���� ��
    private float maxAlpha = 1.0f;  // �ִ� ���� ��
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
            // Mathf.PingPong�� ����Ͽ� ���� �� ���
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
        Vector3 targetScale = originalScale * 2f; // 1.5�� Ŀ������ ����

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // ���� �� ����
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            float scale = Mathf.Lerp(1, 2f, elapsedTime / scaleDuration); // ũ�� ����
            text.rectTransform.localScale = new Vector3(scale, scale, scale);

            yield return null;
        }
        changeScene = 1;
    }
}
