using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class ScorpionMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int moveSpeed;
    public float directionLenght;
    public Vector2 direction;
    SpriteRenderer spriteRenderer;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (moveSpeed > 0) spriteRenderer.flipX = true;
    }

    void FixedUpdate() 
    {
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);

        Vector2 frontvec = new Vector2(rigid.position.x + directionLenght, rigid.position.y);
        Debug.DrawRay(frontvec, direction, new Color(0, 1, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(frontvec, direction, 1, LayerMask.GetMask("Floor"));
        if (rayhit.collider != null) {
            moveSpeed *= -1;
            directionLenght *= -1;
            direction = new Vector2(direction.x * -1, direction.y);
            if (moveSpeed > 0) spriteRenderer.flipX = true;
            else spriteRenderer.flipX = false;
        }
    }
}
