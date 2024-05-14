using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer sprRdr;
    Animator anim;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update() {
        // 캐릭터 좌우 이동
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);

        // 캐릭터 방향 전환
        if (Input.GetButton("Horizontal"))
            // sprRdr.flipX = Input.GetAxisRaw("Horizontal") == -1;
            sprRdr.flipX = rigid.velocity.x < 0;

        // 달리기 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.25)
            anim.SetBool("isRunning", false);
        else
            anim.SetBool("isRunning", true);
    }
    void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed)
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
    }
}