using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float jumpForce = 8;
    [SerializeField] private float maxSpeed = 5.7f;
    private int MaxJump = 1, JumpCount = 0;
    [SerializeField] private int Life = 6;
    private int InitLife;
    private bool Controllable = true;
    private bool IsDamaging = false;
    public bool IsResetting = false;

    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    private bool shouldMove = false;

    [SerializeField] private Vector2 MinPlayerBoundary, MaxPlayerBoundary;
    private Vector2 InitMinPlayerBoundary, InitMaxPlayerBoundary;
    private Vector3 DefaultPos;
    private float DefaultMaxSpeed;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private MainCameraController CameraController;
    private SceneController SceneController;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        CameraController = GameObject.Find("Main Camera").GetComponent<MainCameraController>();
        SceneController = GameObject.Find("SceneControlObject").GetComponent<SceneController>();

        InitLife = Life;
        DefaultPos = new Vector3(-86, -3.4f, 1);
        DefaultMaxSpeed = maxSpeed;
        InitMinPlayerBoundary = MinPlayerBoundary;
        InitMaxPlayerBoundary = MaxPlayerBoundary;
    }

    private void Update() {
        if (rigid.velocity.x < 0) spriteRenderer.flipX = true;
        else if (rigid.velocity.x > 0) spriteRenderer.flipX = false;
        animator.SetBool("IsWalking", Mathf.Abs(rigid.velocity.x) > 0.3);

        if (Controllable) {
            Jump();
            if (!shouldMove &&  Input.GetButtonUp("Horizontal")) rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
                if (shouldMove && waypoints.Length > 0) {
                MoveAlongPath();
            } 
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
        if (Controllable) Move();
        Land();
        LimitPlayerArea();
        LimitPlayerSpeed();
    }

    void OnCollisionStay2D(Collision2D col) {
        if (!IsDamaging && (col.gameObject.CompareTag("monster") || col.gameObject.CompareTag("obstacle"))) {
            StartCoroutine(Damaged(col.transform.position));
        }
    }

    private void LimitPlayerArea() {
        if (transform.position.x < MinPlayerBoundary.x) transform.position = new Vector3(MinPlayerBoundary.x, transform.position.y, transform.position.z);
        else if (transform.position.x > MaxPlayerBoundary.x) transform.position = new Vector3(MaxPlayerBoundary.x, transform.position.y, transform.position.z);
        else if (transform.position.y < MinPlayerBoundary.y) transform.position = new Vector3(transform.position.x, MinPlayerBoundary.y, transform.position.z);
        else if (transform.position.y > MaxPlayerBoundary.y) transform.position = new Vector3(transform.position.x, MaxPlayerBoundary.y, transform.position.z);
    }

    private void LimitPlayerSpeed() {
        if (rigid.velocity.x > maxSpeed) rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -maxSpeed) rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
    }

    private void Move() {
        float h = Input.GetAxisRaw("Horizontal");
        /* if (!shouldMove)  */rigid.AddForce(Vector2.right*h, ForceMode2D.Impulse);
    }

    private void Jump() {
        if (JumpCount != MaxJump && Input.GetButtonDown("Jump")) {
            JumpCount += 1;
            rigid.gravityScale = 1.5f;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }
    }

    public IEnumerator CutSceneMove(float targetX) {
        int d = rigid.position.x < targetX ? 1 : -1;
        while (true) {
            rigid.AddForce(Vector2.right * d, ForceMode2D.Impulse);
            if ((d == 1 && transform.position.x >= targetX)
            || (d == -1 && transform.position.x <= targetX)) {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
                rigid.position = new Vector3(targetX, transform.position.y, transform.position.z);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    // overload for instantly adjust speed
    public IEnumerator CutSceneMove(float targetX, float speed) {
        SetSpeed(speed);
        int d = rigid.position.x < targetX ? 1 : -1;
        while (true) {
            rigid.AddForce(Vector2.right * d, ForceMode2D.Impulse);
            if ((d == 1 && transform.position.x >= targetX)
            || (d == -1 && transform.position.x <= targetX)) {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
                rigid.position = new Vector3(targetX, transform.position.y, transform.position.z);
                ResetSpeed();
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator CutSceneJump(float force) {
        rigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        animator.SetBool("IsJumping", true);
        while (true) {
            if (Physics2D.Raycast(rigid.position, Vector2.down, 0.6f, LayerMask.GetMask("Floor"))) {
                animator.SetBool("IsJumping", false);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void SetSpeed(float speed) { maxSpeed = speed; }
    public void ResetSpeed() { maxSpeed = DefaultMaxSpeed; }

    public void SwitchControllable(bool _bool) { Controllable = _bool; }

    public void BindToCamera() {
        MinPlayerBoundary.x = CameraController.MinCameraBoundary.x - Camera.main.orthographicSize*Camera.main.aspect;
        MaxPlayerBoundary.x = CameraController.MaxCameraBoundary.x + Camera.main.orthographicSize*Camera.main.aspect;
    }

    public void UnbindToCamera() {
        MinPlayerBoundary = InitMinPlayerBoundary;
        MaxPlayerBoundary = InitMaxPlayerBoundary;
    }

    private void Land() {
        if (rigid.velocity.y <= 0) {
            Debug.DrawRay(rigid.position, Vector2.down*0.6f, new Color(0, 1, 0));
            RaycastHit2D hitData = Physics2D.Raycast(rigid.position, Vector2.down, 0.6f, LayerMask.GetMask("Floor"));
            if (hitData.collider != null) {
                animator.SetBool("IsJumping", false);
                JumpCount = 0;
                /* if (hitData.transform.CompareTag("verticalBlock"))
                    transform.position = Vector3.MoveTowards(transform.position, hitData.transform.position, Time.deltaTime * 1.5f);
                else if (hitData.transform.CompareTag("horizontalBlock"))
                    transform.position = Vector3.MoveTowards(transform.position, hitData.transform.position, Time.deltaTime * 5f); */
            }
        }
    }

    private IEnumerator Damaged(Vector2 opponentPos) {
        yield return IsDamaging = true;
        yield return StartCoroutine(TakeDamage(opponentPos));
        if (Life > 0) {
            yield return StartCoroutine(Transparent(2));
            yield return new WaitForSeconds(0.7f);
            yield return StartCoroutine(OffDamage());
            yield return new WaitForSeconds(0.7f);
            yield return IsDamaging = false;
            yield return StartCoroutine(UnTransparent(2));
        } else {
            animator.SetBool("IsDead", true);
            yield return StartCoroutine(Transparent(0));
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(Revive());
            yield return StartCoroutine(UnTransparent(0));
        }
    }

    private IEnumerator TakeDamage(Vector2 opponentPos) {
        Life--;

        int d = transform.position.x < opponentPos.x ? -1 : 1;
        rigid.AddForce(new Vector2(d, 1) * 3, ForceMode2D.Impulse);

        Controllable = false;
        animator.SetBool("IsDamaged", true);

        CameraController.Shake();

        yield return null;
    }

    private IEnumerator OffDamage() {
        Controllable = true;
        JumpCount = 0;
        animator.SetBool("IsDamaged", false);
        yield return null;
    }

    private IEnumerator Transparent(int PhysicsOrColor = 2) {
        if (PhysicsOrColor == 0 || PhysicsOrColor == 2) gameObject.layer = 10;
        if (PhysicsOrColor == 1 || PhysicsOrColor == 2) spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return null;
    }

    private IEnumerator UnTransparent(int PhysicsOrColor = 2) {
        if (PhysicsOrColor == 0 || PhysicsOrColor == 2) gameObject.layer = 9;
        if (PhysicsOrColor == 1 || PhysicsOrColor == 2) spriteRenderer.color = new Color(1, 1, 1, 1);
        yield return null;
    }

    public IEnumerator ResetCondition() {
        Life = InitLife;
        Controllable = true;
        JumpCount = 0;
        IsDamaging = false;

        animator.SetBool("IsDamaged", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        yield return null;
    }

    private IEnumerator Revive() {
        yield return StartCoroutine(SceneController.FadeOut());
        yield return transform.position = DefaultPos;
        yield return IsResetting = true;
        yield return StartCoroutine(ResetCondition());
        yield return IsResetting = false;
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(SceneController.FadeIn());
    }

    public void SetDefaultPos(Vector3 pos) { DefaultPos = pos; }
}