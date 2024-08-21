using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GNPuzzle : MonoBehaviour {
    private int playerAnswer = 0;
    
    private void Update() {
        Debug.Log(playerAnswer);
    }

    public void IncreasePlayerAnswer() {
        if (playerAnswer > 999) return;
        playerAnswer++;
    }

    public void DecreasePlayerAnswer() {
        if (playerAnswer < 1) return;
        playerAnswer--;
    }
}
