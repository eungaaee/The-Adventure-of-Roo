using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;

public class NumberPuzzle : MonoBehaviour {
    [SerializeField] private GameObject Roo;
    [SerializeField] private GameObject PeekingRoo;
    [SerializeField] private GameObject SelectGlow;
    private SpriteRenderer PeekingRooRenderer;
    private SpriteRenderer SelectGlowRenderer;

    private TextMeshPro[,] grid = new TextMeshPro[9, 9];
    private int curRow, curColumn, nxtRow, nxtColumn;
    private const float cooldown = 0.2f;

    private void Awake() {
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                grid[i, j] = transform.GetChild(i*9+j).GetComponentInChildren<TextMeshPro>();
            }
        }
        curRow = nxtRow = 8;
        curColumn = nxtColumn = 0;

        PeekingRooRenderer = PeekingRoo.GetComponent<SpriteRenderer>();
        SelectGlowRenderer = SelectGlow.GetComponent<SpriteRenderer>();
    }

    private void Start() { // 스크립트 인스턴스 활성화 이후 첫 프레임을 업데이트하기 전에 실행됨
        Roo.SetActive(false);
        PeekingRoo.SetActive(true);

        StartCoroutine(SelectGlowEffect());
    }

    private void Update() {
        if (Time.time-lastPressedTime > cooldown) {
            KeyInput();
        }
    }

    private float lastPressedTime = -100;
    private float peekingRooOffset = 0.3f;
    private void KeyInput() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            if (nxtRow > 0 && curRow-nxtRow < 1) {
                lastPressedTime = Time.time;
                
                nxtRow--;
                SelectGlow.transform.position = grid[nxtRow, nxtColumn].transform.position;
            }
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            if (nxtRow < 8 && nxtRow-curRow < 1) {
                lastPressedTime = Time.time;

                nxtRow++;
                SelectGlow.transform.position = grid[nxtRow, nxtColumn].transform.position;
            }
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            if (nxtColumn > 0 && curColumn-nxtColumn < 1) {
                lastPressedTime = Time.time;

                nxtColumn--;
                SelectGlow.transform.position = grid[nxtRow, nxtColumn].transform.position;
            }
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            if (nxtColumn < 8 && nxtColumn-curColumn < 1) {
                lastPressedTime = Time.time;

                nxtColumn++;
                SelectGlow.transform.position = grid[nxtRow, nxtColumn].transform.position;
            }
        } else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) {
            lastPressedTime = -100;

            if (curColumn < nxtColumn) {
                peekingRooOffset = -0.3f;
                PeekingRooRenderer.flipX = true;
            } else if (curColumn > nxtColumn) {
                peekingRooOffset = 0.3f;
                PeekingRooRenderer.flipX = false;
            }
            PeekingRoo.transform.position = new Vector3(
                grid[nxtRow, nxtColumn].transform.position.x + peekingRooOffset,
                grid[nxtRow, nxtColumn].transform.position.y + 0.5f);
            
            curRow = nxtRow;
            curColumn = nxtColumn;
        }
    }

    private IEnumerator SelectGlowEffect(float duration = 1) {
        SelectGlow.SetActive(true);
        while (true) {
            for (float t = 0; t < duration; t += Time.deltaTime) {
                SelectGlowRenderer.color = new Color(1, 1, 1, Mathf.Sin(0.5f*Mathf.PI * 0.1f*t/duration));
                yield return null;
            }
            for (float t = duration; t >= 0; t -= Time.deltaTime) {
                SelectGlowRenderer.color = new Color(1, 1, 1, Mathf.Sin(0.5f*Mathf.PI * 0.1f*t/duration));
                yield return null;
            }
        }
    }
}