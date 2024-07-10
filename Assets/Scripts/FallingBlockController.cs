using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockController : MonoBehaviour {
    public float DestroyY;
    public float dropSpeed = 5.5f;
    Rigidbody2D rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));
        if (Physics2D.Raycast(rigid.position, Vector2.down, 4, LayerMask.GetMask("Player"))) {
            rigid.velocity = new Vector2(0, -dropSpeed);
        }
        if (transform.position.y <= DestroyY) {
            Destroy(gameObject);
        }
    }
}
