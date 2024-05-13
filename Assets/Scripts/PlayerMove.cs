using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float maxSpeed;
    Rigidbody2D rigid;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
    }
}