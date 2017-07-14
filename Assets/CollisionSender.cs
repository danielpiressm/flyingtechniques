using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSender : MonoBehaviour {

    public TestTask tTask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void serializeCollision(string str)
    {
        if(tTask)
        {
            tTask.serializeCollision(str);
        }
        Debug.Log("Estou a debugar");
    }
}
