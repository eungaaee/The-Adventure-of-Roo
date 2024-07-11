using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePoisonDrop : MonoBehaviour
{
    public GameObject PoisonWaterDrop;
    public static bool IsSpawn = false;
    public float[] Spawnlocation = new float[23];
    public int lotation = 22;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 23; i++) {
            Spawnlocation[i] = -5 + 0.55f * i;
        }
    }

    IEnumerator SpawnDrop() {
        IsSpawn = true;
        Vector2 location = new Vector2(transform.position.x + Spawnlocation[lotation], transform.position.y);
        lotation--;
        if (lotation == 0) lotation = 22;
        Instantiate(PoisonWaterDrop, location, transform.rotation, gameObject.transform);
        yield return new WaitForSeconds(0.1f);
        IsSpawn = false;
    }
    // Update is called once per frame
    void Update()
    {   
        if (!IsSpawn)   StartCoroutine(SpawnDrop());
    }
}
