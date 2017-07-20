using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGaze : MonoBehaviour {

    public Transform head;
    Transform hand;
    public Transform rightHand;
    public Transform leftHand;
    public float speed = 3.0f;
    public Transform handTracker;


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

        Vector3 dir = Vector3.ProjectOnPlane(camera.transform.forward, this.transform.up);
        Debug.DrawRay(head.transform.position, dir, Color.red);

        if(Input.GetKey(tTask.getUpButton()))
        {
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if(Input.GetKey(tTask.getDownButton()))
        {
            this.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        /*if(Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }*/



        if (Input.GetKey(tTask.getForwardButton()))
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
        }
        /*else if(Input.GetKey(KeyCode.PageDown))
        {
            Vector3 desiredMove = -dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
        }*/
    }
}
