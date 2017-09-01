using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinectClient;

public class WIPSteering : MonoBehaviour {

    WalkingInPlace wip;
    public Transform head;
    public Transform handTracker;
    public Transform leftHand;
    public Transform rightHand;

    Transform hand;
    Vector3 initialrighttHandRotation = new Vector3(0, 8.995001f, 0);
    Vector3 initialLeftHandRotation = new Vector3(0, -8.995001f, 0);


    public float speed = 3.0f;
    public float raySize = 3.0f;
    Camera camera;
    public float laserWidth;
    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;
    LineRenderer lRenderer;



    // Use this for initialization
    void Start () {
        wip = new WalkingInPlace("log.csv");
        camera = Camera.main;
        tTask = GetComponent<TestTask>();
        hand = rightHand;
        lRenderer = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        Human h = this.GetComponent<TrackerClientRobot>().trackedHuman;
        float rightKnee = 0.0f;
        float leftKnee = 0.0f;
        if (h)
        {
            if(h.body)
            {
                rightKnee = h.body.Joints[BodyJointType.rightKnee];
                leftKnee = h.body.Joints[BodyJointType.leftKnee];
            }
        }

        float speedWIP = wip.updateDaniel(rightKnee, leftKnee);
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


        Vector3 dir = handTracker.transform.forward;
        Debug.DrawRay(hand.transform.position, dir, Color.red);

        if (lRenderer != null)
        {
            lRenderer.enabled = true;
            //hand.transform.eulerAngles = new Vector3(handTracker.transform.eulerAngles.x, handTracker.transform.eulerAngles.z, handTracker.transform.eulerAngles.y);

            //hand.transform.up = -handTracker.transform.forward;

            //hand.transform.localEulerAngles = new Vector3(hand.transform.localEulerAngles.x, hand.transform.localEulerAngles.y, hand.transform.localEulerAngles.z);

            Vector3 endPosition = hand.transform.position + 3.0f * dir;
            lRenderer.SetPosition(0, hand.transform.position);
            lRenderer.SetPosition(1, endPosition);
            lRenderer.startWidth = laserWidth;
            lRenderer.endWidth = laserWidth;
        }
        
        if (Input.GetAxis("Z Axis") > 0.002f)
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = dir * speed * Time.deltaTime *speedWIP;
            this.transform.position += desiredMove;

        }
        else
        {
            // target.SetActive(false);
        }
    }
}
