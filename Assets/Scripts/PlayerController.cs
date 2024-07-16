using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float jumpForce;
    public float maxSpeed;
    public int MaxJump = 2;
    public int JumpCount = 0;
    public int Life = 3;
    private int InitLife;
    private bool IsKnockbacking = false;

    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    private bool shouldMove = false;

    public Vector2 MinPlayerBoundary, MaxPlayerBoundary;
    public Vector3 InitPos;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private MainCameraController CameraController;
    //private SceneController SceneController;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        // SceneController = GameObject.Find("SceneControlObject").GetComponent<SceneController>();

        InitLife = Life;
        InitPos = new Vector3(-86, -3.4f, 1);
    }

    void Update() {
        animator.SetBool("IsWalking", Mathf.Abs(rigid.velocity.x) > 0.3);
        if (Input.GetButtonDown("Jump") && JumpCount != MaxJump && !IsKnockbacking) {
            JumpCount += 1;
            rigid.gravityScale = 1.5f;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
        if (!shouldMove) {
            if (Input.GetButtonUp("Horizontal")) rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        if (shouldMove && waypoints.Length > 0) {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath() {
        if (currentWaypointIndex >= waypoints.Length) {
            shouldMove = false;
            rigid.gravityScale = 1.5f;
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f) {
            currentWaypointIndex++;
        }
    }

    public void StartMoving(Transform[] path) {
        waypoints = path;
        currentWaypointIndex = 0;
        shouldMove = true;
        rigid.gravityScale = 0; 
        rigid.velocity = Vector2.zero; 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (JumpCount == 1 && collision.gameObject.CompareTag("Path")) {
            JumpCount = 0;
            rigid.gravityScale = 1.5f;
            StartMoving(waypoints);
        }
    }

    void FixedUpdate() {
        LimitPlayerArea();
        float h = Input.GetAxisRaw("Horizontal");

        if (h < 0) spriteRenderer.flipX = true;
        else if (h > 0) spriteRenderer.flipX = false;

        if (!shouldMove) rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);
        
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

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("monster")|| collision.gameObject.CompareTag("obstacle")) {
            if (!IsKnockbacking) OnDamage(collision.transform.position);
        }
    }

    private void LimitPlayerArea() {
        if (transform.position.x < MinPlayerBoundary.x) transform.position = new Vector3(MinPlayerBoundary.x, transform.position.y, transform.position.z);
        else if (transform.position.x > MaxPlayerBoundary.x) transform.position = new Vector3(MaxPlayerBoundary.x, transform.position.y, transform.position.z);
        else if (transform.position.y < MinPlayerBoundary.y) transform.position = new Vector3(transform.position.x, MinPlayerBoundary.y, transform.position.z);
        else if (transform.position.y > MaxPlayerBoundary.y) transform.position = new Vector3(transform.position.x, MaxPlayerBoundary.y, transform.position.z);
    }

    private void OnDamage(Vector2 opponentPos) {
        IsKnockbacking = true;
        gameObject.layer = 10;
        JumpCount = 0;

        int dirc = transform.position.x < opponentPos.x ? -1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);
        StartCoroutine(CameraController.Shake());
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        Life--;
        if (Life == 0) {
            animator.SetBool("IsDead", true);
            //StartCoroutine(SceneController.FadeOut());
            Life = InitLife;
            animator.SetBool("IsDead", false);
            transform.position = InitPos;
            //StartCoroutine(SceneController.FadeIn());
        }

        animator.SetBool("IsDamaged", true);
        Invoke(nameof(OffDamage), 0.7f);
    }

    private void OffDamage() {
        IsKnockbacking = false;
        animator.SetBool("IsDamaged", false);
        Invoke(nameof(Untransparent), 0.7f);
    }

    private void Untransparent() {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void ResetCondition() {
        Life = InitLife;
        IsKnockbacking = false;
        JumpCount = 0;

        animator.SetBool("IsDamaged", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsWalking", false);
    }
}