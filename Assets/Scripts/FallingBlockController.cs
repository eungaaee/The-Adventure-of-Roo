using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (!IsLoop) StartCoroutine(RayCheck());
        else {
            InitPos = transform.position;
            rigid.velocity = new Vector2(0, -DropSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (IsLoop) StartCoroutine(ResetPos());
        else Destroy(gameObject);
    }

    private IEnumerator ResetPos() {
        sprRdr.color = new Color(1, 1, 1, 0);
        transform.position = InitPos;
        rigid.velocity = new Vector2(0, 0);
        yield return new WaitForSecondsRealtime(1.5f);
        sprRdr.color = new Color(1, 1, 1, 1);
        rigid.velocity = new Vector2(0, -DropSpeed);
    }

    private IEnumerator RayCheck() {
        Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));
        if (Physics2D.Raycast(rigid.position, Vector2.down, 4, LayerMask.GetMask("Player"))) {
            rigid.velocity = new Vector2(0, -DropSpeed);
            yield break;
        }
        yield return new WaitForFixedUpdate();
        yield return StartCoroutine(RayCheck());
    }
}