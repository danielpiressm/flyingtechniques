using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndGo : MonoBehaviour {

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    public Transform handTracker;

    public float speed = 3.0f;
    
    Camera camera;

    TestTask tTask;
    bool rightHanded = true;
    Transform hand;
    Vector3 initialrighttHandRotation = new Vector3(0, 8.995001f, 0);
    Vector3 initialLeftHandRotation = new Vector3(0, -8.995001f, 0);

    // Use this for initialization
    void Start () {
        tTask = GetComponent<TestTask>();
        rightHanded = tTask.rightHanded;
        camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        if (tTask)
        {
            if (tTask.rightHanded)
            {
                hand = rightHand;
                leftHand.transform.localEulerAngles = initialLeftHandRotation;
            }
            else
            {
                hand = leftHand;
                rightHand.transform.localEulerAngles = initialrighttHandRotation;
            }



            if (tTask.rightHanded == true)
                hand.transform.rotation = Quaternion.LookRotation(handTracker.transform.up, handTracker.transform.forward);
            else if (tTask.rightHanded == false)
                hand.transform.rotation = Quaternion.LookRotation(-handTracker.transform.up, handTracker.transform.forward);

        }

        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented
        Vector3 dir = camera.transform.position - hand.position;
        


        Debug.DrawRay(camera.transform.position, -dir, Color.red);
        //map another button
		if(Input.GetKey(KeyCode.PageUp))
        {
            Vector3 desiredMove = -dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
            RaycastHit hit;
           // target.SetActive(true);
            //Debug.DrawRay(this.transform.position, this.transform.forward,Color.red);
            
        }
        else
        {
            //target.SetActive(false);
        }
	}
}
