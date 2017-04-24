using UnityEngine;
using System.Collections;

public class ExampleTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        //Destroy(other.gameObject);
    }
}
