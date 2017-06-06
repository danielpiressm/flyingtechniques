using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    public string triggerId = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void receiveMessage(string triggerId)
    {
        if(triggerId == this.triggerId)
        {
            Debug.Log(this.triggerId + "receivedMessagedSuccessfully");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Contact " + other.contactOffset);
        SendMessageUpwards("nextTask", triggerId);
    }
}
