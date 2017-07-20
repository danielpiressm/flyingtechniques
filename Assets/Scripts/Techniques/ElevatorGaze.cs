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
    Vector3 initialrighttHandRotation = new Vector3(0, 8.995001f, 0);
    Vector3 initialLeftHandRotation = new Vector3(0, -8.995001f, 0);

    Camera camera;
    TestTask tTask;
    

    // Use this for initialization
    void Start () {
        tTask = GetComponent<TestTask>();
        camera = Camera.main;
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

        Vector3 dir = Vector3.ProjectOnPlane(camera.transform.forward, this.transform.up);
        Debug.DrawRay(head.transform.position, dir, Color.red);

        if(Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if(Input.GetKey(KeyCode.DownArrow))
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



        if (Input.GetKey(KeyCode.PageUp))
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
