using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float jumpForce;
    public float maxSpeed;
    public int MaxJump = 2;
    public int JumpCount = 0;
    public int Life = 3;
    private bool knockbacking = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        animator.SetBool("IsWalking", Mathf.Abs(rigid.velocity.x) > 0.3);

        if (Input.GetButtonUp("Horizontal")) rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        if (Input.GetButtonDown("Jump") && JumpCount != MaxJump && !knockbacking) {
            JumpCount += 1;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
    }

    void FixedUpdate() {
        LimitPlayerArea();
        float h = Input.GetAxisRaw("Horizontal");

        if (h < 0) spriteRenderer.flipX = true;
        else if (h > 0) spriteRenderer.flipX = false;

        rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed) rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed) rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);

        if (rigid.velocity.y <= 0) {
            Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));
            RaycastHit2D hitData = Physics2D.Raycast(rigid.position, Vector2.down, 1f, LayerMask.GetMask("Floor"));
            if (hitData.collider != null) {
                animator.SetBool("IsJumping", false);
                JumpCount = 0;
                if (hitData.transform.CompareTag("verticalBlock"))
                    transform.position = Vector3.MoveTowards(transform.position, hitData.transform.position, Time.deltaTime * 1.5f);
                else if (hitData.transform.CompareTag("horizontalBlock"))
                    transform.position = Vector3.MoveTowards(transform.position, hitData.transform.position, Time.deltaTime * 5f);
            }
        }
    }

    void LimitPlayerArea() {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        if (pos.y < 0f) pos.y = 0f;
        if (pos.y > 1f) pos.y = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "monster" || collision.gameObject.tag == "obstacle") {
            if (!knockbacking) OnDamage(collision.transform.position);
        }
    }
    void OnDamage(Vector2 opponentPos) {
        knockbacking = true;
        Life--;
        gameObject.layer = 10;
        int dirc = transform.position.x < opponentPos.x ? -1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);
        animator.SetBool("IsDamaged", true);
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        GameObject.Find("Main Camera").GetComponent<MainCameraController>().StartCoroutine("Shake");
        Invoke("OffDamage", 1);
    }

    void OffDamage() {
        knockbacking = false;
        animator.SetBool("IsDamaged", false);
        Invoke("Untransparent", 1);
    }

    void Untransparent() {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}