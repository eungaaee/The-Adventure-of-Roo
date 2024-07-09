using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour
{
    public float jumpPower;
    public float maxSpeed;
    public int MaxJump = 2;
    public int JumpCount = 0;
    public int Life = 3;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
 
    void Update() {
        //����
        if (Input.GetButtonDown("Jump") && JumpCount != MaxJump) {
            JumpCount += 1;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }

        //���ӵ� ���ϱ� X
        if (Input.GetButtonUp("Horizontal")) 
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //�ȴ� �������� ���� �ٲٱ�
        if (Input.GetAxisRaw("Horizontal")< 0) {
            spriteRenderer.flipX = true;
        } else if (Input.GetAxisRaw("Horizontal") > 0) {
            spriteRenderer.flipX = false;
        }

        //�ȱ� �ִϸ��̼�
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            animator.SetBool("IsWalking", false);
        else
            animator.SetBool("IsWalking", true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        LimitPlayerArea();
        // Ű�� ���� ������
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) //������ �ִ� �ӷ�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < (-1) * maxSpeed) //���� �ִ� ���
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        //������ ���(�����ɽ�Ʈ, for ����)
        if(rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Floor"));

            if (rayhit.collider != null) {
                if (rayhit.distance < 1f)
                    animator.SetBool("IsJumping", false);
                    JumpCount = 0;
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
}
