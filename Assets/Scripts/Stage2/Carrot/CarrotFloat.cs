using UnityEngine;

public class Floating : MonoBehaviour
{
    public float Bounciness = 0.2f;
    public float Frequency = 1f; 

    private Vector3 posOffset = new Vector3(); 
    private Vector3 tempPos = new Vector3(); 

    void Start()
    {
        posOffset = transform.position;
    }

    void Update()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Bounciness;

        transform.position = tempPos;
    }
}
