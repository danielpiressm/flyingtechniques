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
    float previousSpeed = 0.0f;
    public float raySize = 3.0f;
    public float wipThreshold = 0.12f;
    public float laserWidth;
    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;
    LineRenderer lRenderer;
    float circleSize = 2;
    float lastSpeed = 0;
    public float wipMultiplier = 1;
    GameObject textGO;

    bool started = false;



    float getSpeed(Vector2 localPosition)
    {
        float circleRadius = circleSize / 2;
        float xN = localPosition.x / circleRadius;
        float yN = localPosition.y / circleRadius;


        //Debug.Log("xN = (" + xN + "," + yN + ")");

        float result = (float)Mathf.Sqrt(xN * xN + yN * yN);

        return result;
    }

    // Use this for initialization
    void Start () {
        wip = new WalkingInPlace("log.csv");
        tTask = GetComponent<TestTask>();
        hand = rightHand;
        lRenderer = GetComponent<LineRenderer>();
        Math3d.Init();
        textGO = GameObject.Find("Text");
        GameObject safeCircle = GameObject.Find("safeCircle");
        GameObject warningCircle = GameObject.Find("warningCircle");
        if (safeCircle)
        {
            safeCircle.GetComponent<MeshRenderer>().enabled = false;
        }
        if(warningCircle)
        {
            warningCircle.GetComponent<MeshRenderer>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Z Axis") > 0.002f)
        {
            started = true;
        }

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

        Vector3 transformedPoint = this.transform.InverseTransformPoint(referenceJoint.transform.position);

        float circleSpeed = getSpeed(new Vector2(transformedPoint.x, transformedPoint.z));

        

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
        Vector3 dir = handTracker.transform.forward;
        //Vector3 dir = Camera.main.transform.forward;
        Debug.DrawRay(hand.transform.position, dir, Color.red);


        
       

        float speedWIP = wip.updateDaniel(rightDiff, leftDiff);
        float speedWIP2 = speedWIP;
        float temp = speedWIP * wipMultiplier;
        speedWIP = temp;
        speedWIP = Mathf.Clamp(temp, 0.0f, 1.0f);

        textGO.GetComponent<Text>().text = " SPEED WIP = " + temp + " clamp = " + speedWIP ;
        Camera.main.GetComponentInChildren<TextMesh>().text = " SPEED WIP = " + temp + " \n clamp = " + speedWIP;
       
        if (tTask.started)
        {
            Vector3 desiredMove = dir * speed * Time.deltaTime *speedWIP;
            System.IO.File.AppendAllText(tTask.getPathDirectory() +  "wipDoido.csv" ,  rightDiff + "," + leftDiff + "," + speedWIP + "," + temp +  "," + wipMultiplier + ","+ Time.realtimeSinceStartup + "\n");
            if (wip._gait == KinectClient.GaitState.MOVING)
            {
                this.transform.position += desiredMove;
                if (tTask)
                {
                    tTask.setNavigationState(true, circleSpeed, previousSpeed);
                    tTask.setSpeed(speed * speedWIP);
                }
            }
            else
            {
                if(tTask)
                {
                    tTask.setNavigationState(false, circleSpeed, previousSpeed);
                    tTask.setSpeed(0);
                }
                    
            }
            previousSpeed = circleSpeed;
        }
        
        
    }
}
