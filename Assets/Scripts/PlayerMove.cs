using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float maxSpeed;
    public float jumpHeight;
    Rigidbody2D rigid;
    SpriteRenderer sprRdr;
    Animator anim;
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update() {
        // 좌우 방향키 땠을 때 속력 *0.1
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);

        // 캐릭터 방향 반전
        if (Input.GetButton("Horizontal"))
            sprRdr.flipX = rigid.velocity.normalized.x < 0;

        // 좌우 이동 애니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.25)
            anim.SetBool("isRunning", false);
        else
            anim.SetBool("isRunning", true);

        // 점프 상승
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")) {
            rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
    }
    void FixedUpdate() {
        // 좌우 이동
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed)
            rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        // 점프 착지
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        if (rigid.velocity.normalized.y < 0) {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Floor"));
            // 점프 착지가 아닌 그냥 낙하
            if (!anim.GetBool("isJumping"))
                anim.SetBool("isFalling", true);
            else if (rayHit.collider != null && rayHit.distance < 0.5f) {
                // Debug.Log(rayHit.collider.name);
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
            }
        }
    }
}