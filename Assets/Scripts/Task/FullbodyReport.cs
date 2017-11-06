using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyLog { rightFoot, leftFoot, rightHand, leftHand, head, torso,rightShin, leftShin, circle, leftUpLeg, rightUpLeg, rightForearm, leftForearm, rightArm, leftArm };

public class FullbodyReport : MonoBehaviour
{

    public Transform head;
    public Transform torso;
    public Transform rightHand;
    public Transform leftHand;
    public Transform rightForearm;
    public Transform leftForearm;
    public Transform rightArm;
    public Transform leftArm;
    public Transform rightFoot;
    public Transform leftFoot;
    public Transform rightShin; //shin=canela
    public Transform leftShin;
    public Transform leftUpLeg;
    public Transform rightUpLeg;
    public Transform handTracker;
    public Transform circle;

    public BoxCollider rightFootCollider;
    public BoxCollider leftFootCollider;
    public BoxCollider rightHandCollider;
    public BoxCollider leftHandCollider;
    public BoxCollider torsoCollider;


    private Vector3[] lastBodyPos;
    private string[] bodyStr;
    private string[] bodyStrPath;
    private string[] tempStr;

    private int countFullBodiesStr = 0;

    public bool fullbodyLog = false;

    public string pathHeaderStr = "";

    //public int currentRing;
    private TestTask tTask;



    // Use this for initialization
    void Start()
    {
        tTask = this.GetComponent<TestTask>();
        InitializeFullbodyReport();
    }

    // Update is called once per frame
    void Update()
    {
        updateFullbodyReport();
    }

    public void InitializeFullbodyReport()
    {
        //pathHeaderStr = "Ring,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,rotX,rotY,rotZ,magnitude,CameraPosX,CameraPosY,CameraPosZ,CameraRotX,CameraRotY,CameraRotZ,Time,currentNavigationState\n";

        pathHeaderStr = string.Join(",", new string[]
                {
                    "Ring",
                    "currentPosX",
                    "currentPosY",
                    "currentPosZ",
                    "pathElapsedX",
                    "pathElapsedY",
                    "pathElapsedZ",
                    "rotX",
                    "rotY",
                    "rotZ",
                    "magnitude",
                    "CameraPosX",
                    "CameraPosY",
                    "CameraPosZ",
                    "CameraRotX",
                    "CameraRotY",
                    "CameraRotZ",
                    "Time",
                    "NavigationState",
                    "circlePosX",
                    "circlePosY",
                    "circlePosZ",
                    "speed",
                    "colliderSizeX",
                    "colliderSizeY",
                    "colliderSizeZ",
                    "colliderCenterX",
                    "colliderCenterY",
                    "colliderCenterZ",
                    "Speed",
                    "NavigationState",
                    "torsoForwardX",
                    "torsoForwardY",
                    "torsoForwardZ",
                    "wipInfo(speed-rightShin-leftShin)",
                    "\n"

                });

        lastBodyPos = new Vector3[15];
        bodyStrPath = new string[15];
        bodyStr = new string[15];

        for (int i = 0; i < 15; i++)
        {
            bodyStr[i] = pathHeaderStr;
        }

        if (rightFoot != null)
        {
            lastBodyPos[(int)BodyLog.rightFoot] = rightFoot.position;
            bodyStrPath[(int)BodyLog.rightFoot] = "rightFootLog.csv";
            //System.IO.File.WriteAllText(tTask.getPathDirectory() + "/"+ "rightFootLog.csv", "");
            //create a file for each part of the body
        }
        if (leftFoot != null)
        {
            //log this
            lastBodyPos[(int)BodyLog.leftFoot] = leftFoot.position;
            bodyStrPath[(int)BodyLog.leftFoot] = "leftFootLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "leftFootLog.csv", "");
        }
        if (rightHand != null)
        {
            lastBodyPos[(int)BodyLog.rightHand] = rightHand.position;
            bodyStrPath[(int)BodyLog.rightHand] = "rightHandLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "rightHandLog.csv", "");
        }
        if (leftHand != null)
        {
            lastBodyPos[(int)BodyLog.leftHand] = leftHand.position;
            bodyStrPath[(int)BodyLog.leftHand] = "leftHandLog.csv";
            //System.IO.File.WriteAllText(pathDirectory + "leftHandLog.csv", "");
        }
        if (rightShin != null)
        {
            lastBodyPos[(int)BodyLog.rightShin] = rightShin.position;
            bodyStrPath[(int)BodyLog.rightShin] = "rightShinLog.csv";
            //System.IO.File.WriteAllText(pathDirectory + "rightShinLog.csv", "");
        }
        if (leftShin != null)
        {
            lastBodyPos[(int)BodyLog.leftShin] = leftShin.position;
            bodyStrPath[(int)BodyLog.leftShin] = "leftShinLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "leftShinLog.csv", "");
        }
        if (rightUpLeg != null)
        {
            lastBodyPos[(int)BodyLog.rightUpLeg] = rightUpLeg.position;
            bodyStrPath[(int)BodyLog.rightUpLeg] = "rightUpLeg.csv";
            //System.IO.File.WriteAllText(pathDirectory + "leftHandLog.csv", "");
        }
        if (leftUpLeg != null)
        {
            lastBodyPos[(int)BodyLog.leftUpLeg] = leftUpLeg.position;
            bodyStrPath[(int)BodyLog.leftUpLeg] = "leftUpLeg.csv";
            //System.IO.File.WriteAllText(pathDirectory + "rightShinLog.csv", "");
        }

        if (head != null)
        {
            lastBodyPos[(int)BodyLog.head] = head.position;
            bodyStrPath[(int)BodyLog.head] = "headLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "headLog.csv", "");
        }
        /*if(circle != null)
        {
            lastBodyPos[7] = circle.position;
            bodyStrPath[7] = "circle.csv";
        }*/
        if (torso != null)
        {
            lastBodyPos[(int)BodyLog.torso] = torso.position;
            bodyStrPath[(int)BodyLog.torso] = "torso.csv";
        }
        if (rightForearm != null)
        {
            lastBodyPos[(int)BodyLog.rightForearm] = rightForearm.position;
            bodyStrPath[(int)BodyLog.rightForearm] = "rightForearm.csv";
        }
        if (leftForearm != null)
        {
            lastBodyPos[(int)BodyLog.leftForearm] = leftForearm.position;
            bodyStrPath[(int)BodyLog.leftForearm] = "leftForearm.csv";
        }
        if (rightArm != null)
        {
            lastBodyPos[(int)BodyLog.rightArm] = rightArm.position;
            bodyStrPath[(int)BodyLog.rightArm] = "rightArm.csv";
        }
        if (leftArm != null)
        {
            lastBodyPos[(int)BodyLog.leftArm] = leftArm.position;
            bodyStrPath[(int)BodyLog.leftArm] = "leftArm.csv";
        }
    }

    void updateFullbodyReport()
    {
        if (head == null)
        {
            GameObject headObj = new GameObject("head");
            head = headObj.transform;
            headObj.transform.position = Vector3.zero;
        }
        Vector3 currentPosVector = new Vector3();
        Transform currentJoint;
        Vector3 colliderCenter;
        Vector3 colliderSize;

        if (rightFoot != null)
        {
            currentPosVector = rightFoot.transform.position - lastBodyPos[(int) BodyLog.rightFoot];

            colliderCenter = rightFootCollider.center;
            colliderSize = rightFootCollider.size;

            lastBodyPos[(int)BodyLog.rightFoot] = rightFoot.transform.position;
            bodyStr[(int)BodyLog.rightFoot] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    rightFoot.transform.position.x.ToString() + "," +
                    rightFoot.transform.position.y.ToString() + "," +
                    rightFoot.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    rightFoot.transform.eulerAngles.x.ToString() + "," +
                    rightFoot.transform.eulerAngles.y.ToString() + "," +
                    rightFoot.transform.eulerAngles.z.ToString() + "," +
                    rightFoot.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
                    
            //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[0] , bodyStr[0]);


            //create a file for each part of the body
        }
        if (leftFoot != null)
        {
            currentPosVector = leftFoot.transform.position - lastBodyPos[(int) BodyLog.leftFoot];

            colliderCenter = leftFootCollider.center;
            colliderSize = leftFootCollider.size;

            lastBodyPos[(int)BodyLog.leftFoot] = leftFoot.transform.position;
            bodyStr[(int)BodyLog.leftFoot] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    leftFoot.transform.position.x.ToString() + "," +
                    leftFoot.transform.position.y.ToString() + "," +
                    leftFoot.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    leftFoot.transform.eulerAngles.x.ToString() + "," +
                    leftFoot.transform.eulerAngles.y.ToString() + "," +
                    leftFoot.transform.eulerAngles.z.ToString() + "," +
                    leftFoot.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";

            
            //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[1] , bodyStr[1]);

        }
        if (rightHand != null)
        {
            currentPosVector = rightHand.transform.position - lastBodyPos[(int) BodyLog.rightHand];
            //currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.head];

            //Vector3 leftHandAngles = leftHand.eulerAngles;
            Vector3 rightHandAngles = rightHand.eulerAngles;
            colliderCenter = rightHandCollider.center;
            colliderSize = rightHandCollider.size;

            
            if (tTask.rightHanded && handTracker != null)
            {
                    rightHandAngles = handTracker.eulerAngles;
            }

            lastBodyPos[(int)BodyLog.rightHand] = rightHand.transform.position;
            bodyStr[(int)BodyLog.rightHand] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    rightHand.transform.position.x.ToString() + "," +
                    rightHand.transform.position.y.ToString() + "," +
                    rightHand.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    rightHandAngles.x.ToString() + "," +
                    rightHandAngles.y.ToString() + "," +
                    rightHandAngles.z.ToString() + "," +
                    rightHand.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";

            
            //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[2] , bodyStr[2]);

        }
        if (leftHand != null)
        {
            lastBodyPos[(int)BodyLog.leftHand] = leftHand.transform.position;
            currentPosVector = leftHand.transform.position - lastBodyPos[(int)BodyLog.leftHand];

            colliderCenter = leftHandCollider.center;
            colliderSize = leftHandCollider.size;

            Vector3 leftHandAngles = leftHand.eulerAngles;

            
            if (!tTask.rightHanded && handTracker != null)
            {
               leftHandAngles = handTracker.eulerAngles;
            }
            


            lastBodyPos[(int)BodyLog.leftHand] = rightHand.transform.position;
            bodyStr[(int)BodyLog.leftHand] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    leftHand.transform.position.x.ToString() + "," +
                    leftHand.transform.position.y.ToString() + "," +
                    leftHand.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    leftHandAngles.x.ToString() + "," +
                    leftHandAngles.y.ToString() + "," +
                    leftHandAngles.z.ToString() + "," +
                    leftHand.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
            //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[3] , bodyStr[3]);

        }
        if (rightShin != null)
        {
            lastBodyPos[(int)BodyLog.rightShin] = rightShin.transform.position;
            currentPosVector = rightShin.transform.position - lastBodyPos[(int)BodyLog.rightShin];

            colliderCenter = rightShin.GetComponent<BoxCollider>().center;
            colliderSize = rightShin.GetComponent<BoxCollider>().size;

            lastBodyPos[(int)BodyLog.rightShin] = rightShin.transform.position;
            bodyStr[(int)BodyLog.rightShin] += 
                    "ring"+tTask.getCurrentRing().ToString()+","+
                    rightShin.transform.position.x.ToString()+","+
                    rightShin.transform.position.y.ToString()+","+
                    rightShin.transform.position.z.ToString()+","+
                    currentPosVector.x.ToString()+","+
                    currentPosVector.y.ToString()+","+
                    currentPosVector.z.ToString()+","+
                    rightShin.transform.eulerAngles.x.ToString()+","+
                    rightShin.transform.eulerAngles.y.ToString()+","+
                    rightShin.transform.eulerAngles.z.ToString()+","+
                    rightShin.transform.position.magnitude.ToString()+","+
                    head.transform.position.x.ToString()+","+
                    head.transform.position.y.ToString()+","+
                    head.transform.position.z.ToString()+","+
                    Camera.main.transform.eulerAngles.x.ToString()+","+
                    Camera.main.transform.eulerAngles.y.ToString()+","+
                    Camera.main.transform.eulerAngles.z.ToString()+","+
                    Time.realtimeSinceStartup.ToString()+","+
                    tTask.getCurrentNavigationState().ToString()+","+
                    circle.transform.position.x.ToString()+","+
                    circle.transform.position.y.ToString()+","+
                    circle.transform.position.z.ToString()+","+
                    tTask.getCurrentSpeed().ToString()+","+
                    colliderSize.x.ToString()+","+
                    colliderSize.y.ToString()+","+
                    colliderSize.z.ToString()+","+
                    colliderCenter.x.ToString()+","+
                    colliderCenter.y.ToString()+","+
                    colliderCenter.z.ToString()+","+
                    "\n";
            //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[4] , bodyStr[4]);

        }
        if (leftShin != null)
        {
            lastBodyPos[(int)BodyLog.leftShin] = leftShin.transform.position;
            currentPosVector = leftShin.transform.position - lastBodyPos[(int)BodyLog.leftShin];
            colliderSize = leftShin.GetComponent<BoxCollider>().size;
            colliderCenter = leftShin.GetComponent<BoxCollider>().center;
            if (true)
            {
                lastBodyPos[(int)BodyLog.leftShin] = leftShin.transform.position;
                bodyStr[(int)BodyLog.leftShin] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    leftShin.transform.position.x.ToString() + "," +
                    leftShin.transform.position.y.ToString() + "," +
                    leftShin.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    leftShin.transform.eulerAngles.x.ToString() + "," +
                    leftShin.transform.eulerAngles.y.ToString() + "," +
                    leftShin.transform.eulerAngles.z.ToString() + "," +
                    leftShin.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
                //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[5] , bodyStr[5]);
            }
        }
        if (head != null)
        {
            lastBodyPos[(int)BodyLog.head] = head.transform.position;
            currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.head];
            colliderSize = new Vector3(999, 999, 999);
            colliderCenter = new Vector3(999, 999, 999);


            lastBodyPos[(int)BodyLog.head] = head.transform.position;
            bodyStr[(int)BodyLog.head] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    head.transform.eulerAngles.x.ToString() + "," +
                    head.transform.eulerAngles.y.ToString() + "," +
                    head.transform.eulerAngles.z.ToString() + "," +
                    head.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    "\n";
            //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[6] , bodyStr[6]);

        }
        if (circle != null)
        {
            lastBodyPos[(int)BodyLog.circle] = circle.transform.position;
            currentPosVector = torso.transform.position - lastBodyPos[(int)BodyLog.circle];
            colliderSize = new Vector3(999, 999, 999);
            colliderCenter = new Vector3(999, 999, 999);

            lastBodyPos[(int)BodyLog.circle] = head.transform.position;
            bodyStr[(int)BodyLog.circle] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    circle.transform.eulerAngles.x.ToString() + "," +
                    circle.transform.eulerAngles.y.ToString() + "," +
                    circle.transform.eulerAngles.z.ToString() + "," +
                    circle.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
            //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[6] , bodyStr[6]);
        }
        if (torso != null)
        {
            lastBodyPos[(int)BodyLog.torso] = torso.transform.position;
            currentPosVector = torso.transform.position - lastBodyPos[(int)BodyLog.torso];
            colliderCenter = torsoCollider.center;
            colliderSize = torsoCollider.size;

            lastBodyPos[(int)BodyLog.torso] = torso.transform.position;
            bodyStr[(int)BodyLog.torso] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    torso.transform.position.x.ToString() + "," +
                    torso.transform.position.y.ToString() + "," +
                    torso.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    torso.transform.eulerAngles.x.ToString() + "," +
                    torso.transform.eulerAngles.y.ToString() + "," +
                    torso.transform.eulerAngles.z.ToString() + "," +
                    torso.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    torso.transform.forward.x.ToString() + "," +
                    torso.transform.forward.y.ToString() + "," +
                    torso.transform.forward.z.ToString() + "," +
                    tTask.getSpeedWIP() + "," +
                    "\n";
        }
        if (rightForearm != null)
        {
            currentJoint = rightForearm;
            lastBodyPos[(int)BodyLog.rightForearm] = rightForearm.transform.position;
            currentPosVector = rightForearm.transform.position - lastBodyPos[(int)BodyLog.rightForearm];
            colliderCenter = rightForearm.GetComponent<BoxCollider>().center;
            colliderSize = rightForearm.GetComponent<BoxCollider>().size;

            lastBodyPos[(int)BodyLog.rightForearm] = rightForearm.transform.position;
            bodyStr[(int)BodyLog.rightForearm] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    currentJoint.transform.position.x.ToString() + "," +
                    currentJoint.transform.position.y.ToString() + "," +
                    currentJoint.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    currentJoint.transform.eulerAngles.x.ToString() + "," +
                    currentJoint.transform.eulerAngles.y.ToString() + "," +
                    currentJoint.transform.eulerAngles.z.ToString() + "," +
                    torso.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
        }
        if (leftForearm != null)
        {
            currentJoint = leftForearm;
            lastBodyPos[(int)BodyLog.leftForearm] = currentJoint.transform.position;
            currentPosVector = currentJoint.transform.position - lastBodyPos[(int)BodyLog.leftForearm];

            colliderSize = leftForearm.GetComponent<BoxCollider>().size;
            colliderCenter = leftForearm.GetComponent<BoxCollider>().center;

            lastBodyPos[(int)BodyLog.leftForearm] = leftForearm.transform.position;
            bodyStr[(int)BodyLog.leftForearm] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    currentJoint.transform.position.x.ToString() + "," +
                    currentJoint.transform.position.y.ToString() + "," +
                    currentJoint.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    currentJoint.transform.eulerAngles.x.ToString() + "," +
                    currentJoint.transform.eulerAngles.y.ToString() + "," +
                    currentJoint.transform.eulerAngles.z.ToString() + "," +
                    torso.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
        }
            if (rightArm != null)
            {
                currentJoint = rightArm.transform;
                lastBodyPos[(int)BodyLog.rightArm] = currentJoint.transform.position;
                currentPosVector = currentJoint.transform.position - lastBodyPos[(int)BodyLog.rightArm];
                colliderCenter = rightArm.GetComponent<BoxCollider>().center;
                colliderSize = rightArm.GetComponent<BoxCollider>().size;


                lastBodyPos[(int)BodyLog.rightArm] = rightArm.transform.position;
            bodyStr[(int)BodyLog.rightArm] +=
                "ring" + tTask.getCurrentRing().ToString() + "," +
                currentJoint.transform.position.x.ToString() + "," +
                currentJoint.transform.position.y.ToString() + "," +
                currentJoint.transform.position.z.ToString() + "," +
                currentPosVector.x.ToString() + "," +
                currentPosVector.y.ToString() + "," +
                currentPosVector.z.ToString() + "," +
                currentJoint.transform.eulerAngles.x.ToString() + "," +
                currentJoint.transform.eulerAngles.y.ToString() + "," +
                currentJoint.transform.eulerAngles.z.ToString() + "," +
                torso.transform.position.magnitude.ToString() + "," +
                head.transform.position.x.ToString() + "," +
                head.transform.position.y.ToString() + "," +
                head.transform.position.z.ToString() + "," +
                Camera.main.transform.eulerAngles.x.ToString() + "," +
                Camera.main.transform.eulerAngles.y.ToString() + "," +
                Camera.main.transform.eulerAngles.z.ToString() + "," +
                Time.realtimeSinceStartup.ToString() + "," +
                tTask.getCurrentNavigationState().ToString() + "," +
                circle.transform.position.x.ToString() + "," +
                circle.transform.position.y.ToString() + "," +
                circle.transform.position.z.ToString() + "," +
                tTask.getCurrentSpeed().ToString() + "," +
                colliderSize.x.ToString() + "," +
                colliderSize.y.ToString() + "," +
                colliderSize.z.ToString() + "," +
                colliderCenter.x.ToString() + "," +
                colliderCenter.y.ToString() + "," +
                colliderCenter.z.ToString() + "," +
                "\n";
            }
        if (leftArm != null)
        {
            currentJoint = leftArm.transform;
            lastBodyPos[(int)BodyLog.leftArm] = currentJoint.transform.position;
            currentPosVector = currentJoint.transform.position - lastBodyPos[(int)BodyLog.leftArm];
            colliderCenter = rightArm.GetComponent<BoxCollider>().center;
            colliderSize = rightArm.GetComponent<BoxCollider>().size;


            lastBodyPos[(int)BodyLog.leftArm] = rightArm.transform.position;
            bodyStr[(int)BodyLog.leftArm] +=
                    "ring" + tTask.getCurrentRing().ToString() + "," +
                    currentJoint.transform.position.x.ToString() + "," +
                    currentJoint.transform.position.y.ToString() + "," +
                    currentJoint.transform.position.z.ToString() + "," +
                    currentPosVector.x.ToString() + "," +
                    currentPosVector.y.ToString() + "," +
                    currentPosVector.z.ToString() + "," +
                    currentJoint.transform.eulerAngles.x.ToString() + "," +
                    currentJoint.transform.eulerAngles.y.ToString() + "," +
                    currentJoint.transform.eulerAngles.z.ToString() + "," +
                    torso.transform.position.magnitude.ToString() + "," +
                    head.transform.position.x.ToString() + "," +
                    head.transform.position.y.ToString() + "," +
                    head.transform.position.z.ToString() + "," +
                    Camera.main.transform.eulerAngles.x.ToString() + "," +
                    Camera.main.transform.eulerAngles.y.ToString() + "," +
                    Camera.main.transform.eulerAngles.z.ToString() + "," +
                    Time.realtimeSinceStartup.ToString() + "," +
                    tTask.getCurrentNavigationState().ToString() + "," +
                    circle.transform.position.x.ToString() + "," +
                    circle.transform.position.y.ToString() + "," +
                    circle.transform.position.z.ToString() + "," +
                    tTask.getCurrentSpeed().ToString() + "," +
                    colliderSize.x.ToString() + "," +
                    colliderSize.y.ToString() + "," +
                    colliderSize.z.ToString() + "," +
                    colliderCenter.x.ToString() + "," +
                    colliderCenter.y.ToString() + "," +
                    colliderCenter.z.ToString() + "," +
                    "\n";
        }
        if (rightUpLeg != null)
            {
                currentJoint = rightUpLeg.transform;
                lastBodyPos[(int)BodyLog.rightUpLeg] = currentJoint.transform.position;
                currentPosVector = currentJoint.transform.position - lastBodyPos[(int)BodyLog.rightUpLeg];
                colliderSize = rightUpLeg.GetComponent<BoxCollider>().size;
                colliderCenter = rightUpLeg.GetComponent<BoxCollider>().center;

            bodyStr[(int)BodyLog.rightUpLeg] +=
                "ring" + tTask.getCurrentRing().ToString() + "," +
                currentJoint.transform.position.x.ToString() + "," +
                currentJoint.transform.position.y.ToString() + "," +
                currentJoint.transform.position.z.ToString() + "," +
                currentPosVector.x.ToString() + "," +
                currentPosVector.y.ToString() + "," +
                currentPosVector.z.ToString() + "," +
                currentJoint.transform.eulerAngles.x.ToString() + "," +
                currentJoint.transform.eulerAngles.y.ToString() + "," +
                currentJoint.transform.eulerAngles.z.ToString() + "," +
                torso.transform.position.magnitude.ToString() + "," +
                head.transform.position.x.ToString() + "," +
                head.transform.position.y.ToString() + "," +
                head.transform.position.z.ToString() + "," +
                Camera.main.transform.eulerAngles.x.ToString() + "," +
                Camera.main.transform.eulerAngles.y.ToString() + "," +
                Camera.main.transform.eulerAngles.z.ToString() + "," +
                Time.realtimeSinceStartup.ToString() + "," +
                tTask.getCurrentNavigationState().ToString() + "," +
                circle.transform.position.x.ToString() + "," +
                circle.transform.position.y.ToString() + "," +
                circle.transform.position.z.ToString() + "," +
                tTask.getCurrentSpeed().ToString() + "," +
                colliderSize.x.ToString() + "," +
                colliderSize.y.ToString() + "," +
                colliderSize.z.ToString() + "," +
                colliderCenter.x.ToString() + "," +
                colliderCenter.y.ToString() + "," +
                colliderCenter.z.ToString() + "," +
                "\n";
            }
            if (leftUpLeg != null)
            {
                currentJoint = leftUpLeg.transform;
                lastBodyPos[(int)BodyLog.leftUpLeg] = currentJoint.transform.position;
                currentPosVector = currentJoint.transform.position - lastBodyPos[(int)BodyLog.leftUpLeg];
                colliderSize = leftUpLeg.GetComponent<BoxCollider>().size;
                colliderCenter = leftUpLeg.GetComponent<BoxCollider>().center;

            bodyStr[(int)BodyLog.leftUpLeg] +=
                "ring" + tTask.getCurrentRing().ToString() + "," +
                currentJoint.transform.position.x.ToString() + "," +
                currentJoint.transform.position.y.ToString() + "," +
                currentJoint.transform.position.z.ToString() + "," +
                currentPosVector.x.ToString() + "," +
                currentPosVector.y.ToString() + "," +
                currentPosVector.z.ToString() + "," +
                currentJoint.transform.eulerAngles.x.ToString() + "," +
                currentJoint.transform.eulerAngles.y.ToString() + "," +
                currentJoint.transform.eulerAngles.z.ToString() + "," +
                torso.transform.position.magnitude.ToString() + "," +
                head.transform.position.x.ToString() + "," +
                head.transform.position.y.ToString() + "," +
                head.transform.position.z.ToString() + "," +
                Camera.main.transform.eulerAngles.x.ToString() + "," +
                Camera.main.transform.eulerAngles.y.ToString() + "," +
                Camera.main.transform.eulerAngles.z.ToString() + "," +
                Time.realtimeSinceStartup.ToString() + "," +
                tTask.getCurrentNavigationState().ToString() + "," +
                circle.transform.position.x.ToString() + "," +
                circle.transform.position.y.ToString() + "," +
                circle.transform.position.z.ToString() + "," +
                tTask.getCurrentSpeed().ToString() + "," +
                colliderSize.x.ToString() + "," +
                colliderSize.y.ToString() + "," +
                colliderSize.z.ToString() + "," +
                colliderCenter.x.ToString() + "," +
                colliderCenter.y.ToString() + "," +
                colliderCenter.z.ToString() + "," +
                "\n";
            }

            //flush the string into the file
            if (countFullBodiesStr > 1000)
            {
            /*for (int i = 0; i < 15; i++)
            {
                try
                {
                    if (bodyStr[i] != null && bodyStrPath[i] != null)
                    {
                        System.IO.File.AppendAllText(tTask.getPathDirectory() + "/fullbodyLog/" + bodyStrPath[i], bodyStr[i]);
                    }

                    //Debug.Log("&&&&&");
                }
                catch (System.Exception ex)
                {
                    //Debug.LogError("@@@@@@@ : " + bodyStrPath[i]);
                }

                bodyStr[i] = "";
            }*/
            /*if (bodyStr[(int)BodyLog.torso] != null && bodyStrPath[(int)BodyLog.torso] != null)
            {
                System.IO.File.AppendAllText(tTask.getPathDirectory() + "/fullbodyLog/" + bodyStrPath[(int)BodyLog.torso], bodyStr[(int)BodyLog.torso]);
            }*/
            countFullBodiesStr = 0;
            }
            else
                countFullBodiesStr++;
        }



    private void OnDisable()
    {
        for (int i = 0; i < 15; i++)
        {
            try
            {
                if (bodyStr[i] != null && bodyStrPath[i] != null)
                {
                    System.IO.File.AppendAllText(tTask.getPathDirectory() + "/fullbodyLog/" + bodyStrPath[i], bodyStr[i]);
                }

                //Debug.Log("&&&&&");
            }
            catch (System.Exception ex)
            {
                //Debug.LogError("@@@@@@@ : " + bodyStrPath[i]);
            }

            bodyStr[i] = "";
        }
    }



}
