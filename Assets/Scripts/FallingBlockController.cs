using System.Collections;
using UnityEngine;

public class FallingBlockController : MonoBehaviour {
    [SerializeField] private float DropSpeed = 5.5f;
    [SerializeField] private bool IsLoop = false;

    [SerializeField] private PlayerController Player;

    private Vector3 InitPos;

    private Rigidbody2D rigid;
    private SpriteRenderer sprRdr;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        InitPos = rigid.position;
    }

    private void Start() {
        if (IsLoop) rigid.velocity = new Vector2(0, -DropSpeed);
        else StartCoroutine(RayCheck());
    }

    private void Update() {
        if (Player.IsReset) ReGenerate();
    }

    private void OnCollisionStay2D(Collision2D col) {
        if (IsLoop) StartCoroutine(ResetPos());
        // else Destroy(gameObject);
        else Hide();
    }

    private void Hide() {
        sprRdr.color = new Color(1, 1, 1, 0);
        rigid.velocity = Vector2.zero;
        rigid.position = InitPos;
    }

    private void ReGenerate() {
        sprRdr.color = new Color(1, 1, 1, 1);
        StartCoroutine(RayCheck());
    }

    private IEnumerator ResetPos() {
        sprRdr.color = new Color(1, 1, 1, 0);
        transform.position = InitPos;
        yield return new WaitForSeconds(1.5f);
        sprRdr.color = new Color(1, 1, 1, 1);
        rigid.velocity = new Vector2(0, -DropSpeed);
    }

    private IEnumerator RayCheck() {
        while (true) {
            Debug.DrawRay(transform.position, Vector2.down * 3, Color.green);
            if (Physics2D.Raycast(transform.position, Vector2.down, 3, LayerMask.GetMask("Player"))) {
                rigid.velocity = new Vector2(0, -DropSpeed);
                yield break;
            }
            yield return null;
        }
    }
}
