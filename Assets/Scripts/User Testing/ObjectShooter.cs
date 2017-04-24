using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShooter : MonoBehaviour {

    bool shot1 = false;
    bool shot2 = false;
    bool shot3 = false;
    bool shot4 = false;
    public GameObject ball = null;
    public int increment = 2;
    public GameObject cannon = null;
    // Use this for initialization
    float elapsedTime = -3;
	void Start () {
        Debug.Log("Object shooter online!");
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;
        if(!shot1 && elapsedTime > increment)
        {
            GameObject s = Instantiate(ball, cannon.transform.position, Quaternion.identity);
            s.transform.parent = this.transform.parent.transform.parent;
            s.name = "ballOne";
            float currentTime = Time.realtimeSinceStartup;
            this.transform.parent.gameObject.SendMessageUpwards("logBallThrow", currentTime);
            s.GetComponent<Rigidbody>().AddForce(new Vector3(0.1f,1.05f,1.0f) * 35);
            shot1 = true;
            Debug.Log("Shooting ball one");
        }
        if (!shot2 && elapsedTime > 2*increment)
        {
            GameObject s = Instantiate(ball, cannon.transform.position, Quaternion.identity);
            s.transform.parent = this.transform.parent.transform.parent;
            s.name = "ballTwo";
            float currentTime = Time.realtimeSinceStartup;
            this.transform.parent.gameObject.SendMessageUpwards("logBallThrow", currentTime);
            s.GetComponent<Rigidbody>().AddForce(new Vector3(-0.05f, 1.05f, 1.0f) * 35);
            shot2 = true;
            Debug.Log("Shooting ball two");
        }
        if (!shot3 && elapsedTime > 3*increment)
        {
            GameObject s = Instantiate(ball, cannon.transform.position, Quaternion.identity);
            s.transform.parent = this.transform.parent.transform.parent;
            s.name = "ballThree";
            float currentTime = Time.realtimeSinceStartup;
            this.transform.parent.gameObject.SendMessageUpwards("logBallThrow", currentTime);
            s.GetComponent<Rigidbody>().AddForce(new Vector3(0.1f, 1.15f, 1.0f) * 35);
            shot3 = true;
            Debug.Log("Shooting ball three");
        }
        if (!shot4 && elapsedTime > 4*increment)
        {
            GameObject s = Instantiate(ball, cannon.transform.position, Quaternion.identity);
            s.transform.parent = this.transform.parent.transform.parent;
            s.name = "ballFour";
            float currentTime = Time.realtimeSinceStartup;
            this.transform.parent.gameObject.SendMessageUpwards("logBallThrow", currentTime);
            s.GetComponent<Rigidbody>().AddForce(new Vector3(-0.05f, 1.15f, 1.0f) * 35);
            shot4 = true;
            Debug.Log("Shooting ball four");
        }
        if(elapsedTime > 5*increment)
        {
           // shot1 = shot2 = shot3 = shot4 = false;
           // elapsedTime = 0;
            SendMessageUpwards("nextTask", "objectShooter");
        }
    }
}
