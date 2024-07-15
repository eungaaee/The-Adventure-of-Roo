using UnityEngine;

public class MoveTrigger : MonoBehaviour {
    public Transform[] waypoints; // ��� ����Ʈ��

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerController playerAutoMove = other.GetComponent<PlayerController>();
            if (playerAutoMove != null) {
                playerAutoMove.StartMoving(waypoints);
            }
        }
    }
}

