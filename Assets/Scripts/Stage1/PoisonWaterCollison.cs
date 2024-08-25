using UnityEngine;

public class PoisonWaterCollison : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Floor") {
            Destroy(gameObject);
        }
    }
}
