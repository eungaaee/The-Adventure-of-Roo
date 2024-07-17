using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindTrigger : MonoBehaviour {
    [SerializeField] private bool IsRightToLeft = false;
    private Vector3 StartOfWind, EndOfWind;
    private IEnumerator Sweeper;
    private PlayerController Player;

    private void Awake() {
        Player = GameObject.Find("Roo").GetComponent<PlayerController>();

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
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            if (Sweeper != null) StopCoroutine(Sweeper);
            Sweeper = SweepPlayer();
            StartCoroutine(Sweeper);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            StopCoroutine(Sweeper);
        }
    }

    private IEnumerator SweepPlayer() {
        while (true) {
            Player.GetComponent<Rigidbody2D>().AddForce((EndOfWind-StartOfWind)*3f, ForceMode2D.Force);
            yield return null;
        }
    }
}