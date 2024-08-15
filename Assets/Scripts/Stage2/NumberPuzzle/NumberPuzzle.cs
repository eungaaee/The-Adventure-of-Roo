using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPuzzle : MonoBehaviour {
    [SerializeField] private GameObject PeekingRoo;

    private void Start() { // 스크립트 인스턴스 활성화 이후 첫 프레임을 업데이트하기 전에 실행됨
        PeekingRoo.SetActive(true);
    }

    void Update() {
        
    }
}