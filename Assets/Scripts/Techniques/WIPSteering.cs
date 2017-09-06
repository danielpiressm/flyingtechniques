using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinectClient;
using UnityEngine.UI;

public class WIPSteering : MonoBehaviour {

    WalkingInPlace wip;
    public Transform head;
    public Transform handTracker;
    public Transform leftHand;
    public Transform rightHand;
    public Transform referenceJoint;

    Transform hand;
    Vector3 initialrighttHandRotation = new Vector3(0, 8.995001f, 0);
    Vector3 initialLeftHandRotation = new Vector3(0, -8.995001f, 0);


    public float speed = 3.0f;
    public float raySize = 3.0f;
    public float wipThreshold = 0.12f;
    Camera camera;
    public float laserWidth;
    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;
    LineRenderer lRenderer;
    float circleSize = 2;
    float lastSpeed = 0;
    public float wipMultiplier = 1;
    Math3d math3d;
    GameObject textGO;
    Vector3 inputAcc;

    float getSpeed(Vector2 localPosition)
    {
        float circleRadius = circleSize / 2;
        float xN = localPosition.x / circleRadius;
        float yN = localPosition.y / circleRadius;


        //Debug.Log("xN = (" + xN + "," + yN + ")");

        float result = (float)Mathf.Sqrt(xN * xN + yN * yN);
        if (xN < 0.0f || yN < 0.0f)
            result = 0.0f;



        return result;
    }

    // Use this for initialization
    void Start () {
        wip = new WalkingInPlace("log.csv");
        camera = Camera.main;
        tTask = GetComponent<TestTask>();
        hand = rightHand;
        lRenderer = GetComponent<LineRenderer>();
        Math3d.Init();
        textGO = GameObject.Find("Text");
    }
	
	// Update is called once per frame
	void Update () {
        Human h = this.GetComponent<TrackerClientRobot>().trackedHuman;
        Vector3 rightKnee = new Vector3();
        Vector3 leftKnee = new Vector3();
        Vector3 rightKneeAvg = new Vector3();
        Vector3 leftKneeAvg = new Vector3();

       

        if (h!=null)
        {
            if(h.body!=null)
            {
                rightKnee = h.body.Joints[BodyJointType.rightKnee];
                leftKnee = h.body.Joints[BodyJointType.leftKnee];
                rightKneeAvg = h.body.Joints[BodyJointType.rightKneeAvg];
                leftKneeAvg = h.body.Joints[BodyJointType.leftKneeAvg];
            }
        }


        float rightDiff = 0f;
        float leftDiff = 0f;
        float rightKnee1 = rightKnee.y;
        float leftKnee1 = leftKnee.y;

        if (leftKnee1 > rightKnee1)
        {
            leftDiff = leftKnee1 - rightKnee1;
        }
        else
        {
            rightDiff = rightKnee1 - leftKnee1;
        }


        float speedWIP = wip.updateDaniel(rightDiff, leftDiff);
        speedWIP = Mathf.Clamp(speedWIP, 0.0f, 1.0f);

        inputAcc = Camera.main.transform.localPosition;

        Vector3 outVec = new Vector3();
        Math3d.LinearAcceleration(out outVec, inputAcc, 10);
        textGO.GetComponent<Text>().text = "SPEEDWIP = " + speedWIP * wipMultiplier;
        // Debug.Log("acc = " + outVec);
        //Debug.Log("$$$$$ WIP WORKING ###### " + " speed=" + speedWIP + "Time = " + Time.realtimeSinceStartup + " GAit = " + wip._gait + "multiplier = "+ speedWIP*wipMultiplier);

        if (wip._gait == KinectClient.GaitState.MOVING)
        {
            speedWIP = speedWIP * wipMultiplier;
            //speedWIP = 1.0f;
        }

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

        //TODO: see more
        //Vector3 dir = handTracker.transform.forward;
        Vector3 dir = Camera.main.transform.forward;
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
            if (wip._gait == KinectClient.GaitState.MOVING)
            {
                if (tTask)
                {
                    tTask.setNavigationState(NavigationState.Flying);
                    tTask.setSpeed(speed * speedWIP);
                }
            }
            else
            {
                if(tTask)
                    tTask.setNavigationState(NavigationState.Idle);
            }
                
            

        }
        else
        {
            if (tTask)
            {
                tTask.setNavigationState(NavigationState.Idle);
                tTask.setSpeed(0.0f);
            }
            // target.SetActive(false);
        }
    }
}
