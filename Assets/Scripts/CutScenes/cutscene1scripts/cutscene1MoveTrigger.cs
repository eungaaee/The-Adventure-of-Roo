using UnityEngine;

public class cutscene1MoveTrigger : MonoBehaviour
{
    public bool CutScene1MoveTriggerBool = false;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            CutScene1MoveTriggerBool = true;
        }
    }
}
