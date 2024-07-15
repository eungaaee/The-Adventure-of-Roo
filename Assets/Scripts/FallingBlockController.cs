using System.Collections;
using UnityEngine;

public class FallingBlockController : MonoBehaviour {
    public float DropSpeed = 5.5f;
    public bool IsLoop = false;
    Vector3 InitPos;

    Rigidbody2D rigid;
    SpriteRenderer sprRdr;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sprRdr = GetComponent<SpriteRenderer>();
        InitPos = transform.position;
        rigid.velocity = new Vector2(0, -DropSpeed);
        rigid.bodyType = RigidbodyType2D.Static;
        if (!IsLoop) {
            StartCoroutine(CheckAndDestroy());
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (IsLoop) {
            StartCoroutine(ResetPos());
        } else {
            Destroy(gameObject);
        }
    }

    private IEnumerator ResetPos() {
        sprRdr.color = new Color(1, 1, 1, 0);
        transform.position = InitPos;
        rigid.velocity = Vector2.zero;
        yield return new WaitForSecondsRealtime(1.5f);
        sprRdr.color = Color.white;
        rigid.velocity = new Vector2(0, -DropSpeed);
    }

    private IEnumerator CheckAndDestroy() {
        while (true) {
            Debug.DrawRay(transform.position, Vector2.down * 3, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3, LayerMask.GetMask("Player"));
            if (hit.collider != null) {
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.velocity = new Vector2(0, -DropSpeed);
                yield break;
            }
            yield return null;
        }
    }
}
