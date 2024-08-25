using UnityEngine;

public class WindTrigger : MonoBehaviour {
    [SerializeField] private bool IsRightToLeft = false;
    [SerializeField] private Rigidbody2D PlayerRigid;

    private Vector3 StartOfWind, EndOfWind;
    private const float horizontalForce = 5, verticalForce = 10;
    private Vector2 windForce;
    private bool sweeper;

    private void Awake() {
        StartOfWind = transform.position;
        EndOfWind = transform.position;        
        if (IsRightToLeft) {
            StartOfWind.x += transform.lossyScale.x/2 * Mathf.Cos(transform.rotation.z);
            StartOfWind.y += transform.lossyScale.x/2 * Mathf.Sin(transform.rotation.z) + transform.lossyScale.y;
            EndOfWind.x -= transform.lossyScale.x/2 * Mathf.Cos(transform.rotation.z);
            EndOfWind.y -= transform.lossyScale.x/2 * Mathf.Sin(transform.rotation.z) - transform.lossyScale.y;
        } else {
            StartOfWind.x -= transform.lossyScale.x/2 * Mathf.Cos(transform.rotation.z);
            StartOfWind.y -= transform.lossyScale.x/2 * Mathf.Sin(transform.rotation.z) - transform.lossyScale.y;
            EndOfWind.x += transform.lossyScale.x/2 * Mathf.Cos(transform.rotation.z);
            EndOfWind.y += transform.lossyScale.x/2 * Mathf.Sin(transform.rotation.z) + transform.lossyScale.y;
        }

        windForce.x = (EndOfWind.x-StartOfWind.x) * horizontalForce;
        windForce.y = (EndOfWind.y-StartOfWind.y) * verticalForce;

    }

    private void FixedUpdate() {
        if (sweeper) PlayerRigid.AddForce(windForce, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            sweeper = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            sweeper = false;
        }
    }

    /* private IEnumerator SweepPlayer() {
        while (true) {
            Player.GetComponent<Rigidbody2D>().AddForce((EndOfWind-StartOfWind)*3f, ForceMode2D.Force);
            yield return null;
        }
    } */
}