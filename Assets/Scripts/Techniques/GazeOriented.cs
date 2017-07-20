using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeOriented : MonoBehaviour {

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    public Transform handTracker;
    Transform hand;
    public float speed = 3.0f;
	Camera camera;
    TestTask tTask;
    Quaternion initialrighttHandRotation;
    Quaternion initialLeftHandRotation;

    // Use this for initialization
    void Start () {
        tTask = GetComponent<TestTask>();
        camera = Camera.main;
        initialrighttHandRotation = rightHand.rotation;
        initialLeftHandRotation = leftHand.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented
        if (tTask)
        {
            if (tTask.rightHanded)
            {
                hand = rightHand;
                leftHand.transform.rotation = Quaternion.identity;
            }
            else
            {
                hand = leftHand;
                rightHand.transform.rotation = Quaternion.identity;
            }

            if (tTask.rightHanded == true)
                hand.transform.rotation = Quaternion.LookRotation(handTracker.transform.up, handTracker.transform.forward);
            else if (tTask.rightHanded == false)
                hand.transform.rotation = Quaternion.LookRotation(-handTracker.transform.up, handTracker.transform.forward);

        }

        Debug.DrawRay(head.transform.position, camera.transform.forward, Color.red);
        

        if (Input.GetKey(tTask.getForwardButton()))
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = camera.transform.forward * speed * Time.deltaTime;
            this.transform.position += desiredMove;
            //target.SetActive(true);
            //Debug.DrawRay(head.transform.position, head.transform.forward,Color.red);
           
        }
        else
        {
           // target.SetActive(false);
        }
	}
}
