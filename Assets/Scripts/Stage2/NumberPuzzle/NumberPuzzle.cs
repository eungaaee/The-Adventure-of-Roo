using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPuzzle : MonoBehaviour {
    [SerializeField] private GameObject Roo;
    [SerializeField] private GameObject PeekingRoo;

    private TextMeshPro[,] grid = new TextMeshPro[9, 9];
    private int curX, curY;

    private void Awake() {
        Transform[] cells = GetComponentsInChildren<Transform>();
        int p = 0;
        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                grid[i, j] = cells[p++].GetComponentInChildren<TextMeshPro>();
            }
        }

        curX = 0; curY = 8;
    }

    private void Start() { // 스크립트 인스턴스 활성화 이후 첫 프레임을 업데이트하기 전에 실행됨
        Roo.SetActive(false);
        PeekingRoo.SetActive(true);

    }

    void Update() {

    }
}