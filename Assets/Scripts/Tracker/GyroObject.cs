using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroObject : MonoBehaviour {

    OSCReceiver oscReceiver;

	// Use this for initialization
	void Start () {
        oscReceiver = (OSCReceiver)FindObjectOfType(typeof(OSCReceiver));
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(oscReceiver.yaw1, oscReceiver.roll1, oscReceiver.pitch1);
	}
}
