using UnityEngine;
using UnityEngine.Scripting;

public class MoveTrigger : MonoBehaviour {
    public Transform[] waypoints;
    private PlayerController Player;

    private void Awake() {
        Player = GameObject.Find("Roo").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Player.StartMoving(waypoints);
        }
    }
}