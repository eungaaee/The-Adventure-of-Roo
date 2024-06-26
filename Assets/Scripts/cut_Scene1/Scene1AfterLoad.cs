using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1AfterLoad : MonoBehaviour
{

    public SceneFader SceneFader;

    // Start is called before the first frame update
    void Start()
    {
        SceneFader.AfterLoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
