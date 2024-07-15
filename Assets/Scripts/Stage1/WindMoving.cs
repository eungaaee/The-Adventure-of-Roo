using UnityEngine;

public class MoveTrigger : MonoBehaviour {
    public Transform[] waypoints; // 경로 포인트들

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController playerAutoMove = other.GetComponent<PlayerController>();
            if (playerAutoMove != null) {
                playerAutoMove.StartMoving(waypoints);
            }
        }
    }
}

