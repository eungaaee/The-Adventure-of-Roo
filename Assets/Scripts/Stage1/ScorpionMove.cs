using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class ScorpionMove : MonoBehaviour {
    Rigidbody2D rigid;
    public int moveSpeed;
    private int d = -1;
    SpriteRenderer spriteRenderer;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (moveSpeed < 0) spriteRenderer.flipX = true;
    }

    void FixedUpdate() {
        Debug.DrawRay(rigid.position, Vector2.right*d, new Color(0, 1, 0));
        Debug.DrawRay(new Vector2(rigid.position.x+d*0.5f, rigid.position.y), Vector2.down, new Color(0, 1, 0));
        if (Physics2D.Raycast(rigid.position, Vector2.right*d, 0.5f, LayerMask.GetMask("Floor"))
            || Physics2D.Raycast(new Vector2(rigid.position.x+d*0.5f, rigid.position.y), Vector2.down, 1, LayerMask.GetMask("Floor")).collider == null) {
            d *= -1;
            spriteRenderer.flipX = d==1;
        }
        rigid.velocity = new Vector2(moveSpeed*d, rigid.velocity.y);
    }
}