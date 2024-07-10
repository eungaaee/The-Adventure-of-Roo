using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class ScorpionMove : MonoBehaviour {
    Rigidbody2D rigid;
    public int moveSpeed;
    public float directionLenght;
    public Vector2 direction;
    private int d = -1;
    SpriteRenderer spriteRenderer;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (moveSpeed > 0) spriteRenderer.flipX = true;
    }

    void FixedUpdate() {
        /* Vector2 frontvec = new Vector2(rigid.position.x + directionLenght, rigid.position.y);
        Debug.DrawRay(frontvec, direction, new Color(0, 1, 0));
        if (Physics2D.Raycast(frontvec, direction, 1, LayerMask.GetMask("Floor"))) {
            moveSpeed *= -1;
            directionLenght *= -1;
            direction = new Vector2(direction.x * -1, direction.y);
            if (moveSpeed > 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
        }
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y); */
        Debug.DrawRay(rigid.position, Vector2.right*d, new Color(0, 1, 0));
        if (Physics2D.Raycast(rigid.position, Vector2.right*d, 0.5f, LayerMask.GetMask("Floor"))) {
            d *= -1;
            spriteRenderer.flipX = d==1;
        }
        rigid.velocity = new Vector2(moveSpeed*d, rigid.velocity.y);
    }
}