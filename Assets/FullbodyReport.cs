using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyLog { rightFoot, leftFoot, rightHand, leftHand, head, rightShin, leftShin, hip, torso, };

public class FullbodyReport : MonoBehaviour {

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    public Transform rightFoot;
    public Transform leftFoot;
    public Transform rightShin; //shin=canela
    public Transform leftShin;

    private Vector3[] lastBodyPos;
    private string[] bodyStr;
    private string[] bodyStrPath;

    private int countFullBodiesStr = 0;

    public bool fullbodyLog = false;

    public string pathHeaderStr = "";

    //public int currentRing;
    private TestTask tTask;

    // Use this for initialization
    void Start () {
        tTask = this.GetComponent<TestTask>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitializeFullbodyReport()
    {
        pathHeaderStr = "Ring,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,rotX,rotY,rotZ,magnitude,CameraPosX,CameraPosY,CameraPosZ,CameraRotX,CameraRotY,CameraRotZ\n";

        lastBodyPos = new Vector3[9];
        bodyStrPath = new string[9];
        bodyStr = new string[9];

        for (int i = 0; i < 5; i++)
        {
            bodyStr[i] = pathHeaderStr;
        }

        if (rightFoot != null)
        {
            lastBodyPos[0] = rightFoot.position;
            bodyStrPath[0] = "rightFootLog.csv";
            Debug.Log("rightFootLog");
            //System.IO.File.WriteAllText(pathDirectory  + "rightFootLog.csv", "");
            //create a file for each part of the body
        }
        if (leftFoot != null)
        {
            //log this
            lastBodyPos[1] = leftFoot.position;
            bodyStrPath[1] = "leftFootLog.csv";
            Debug.Log("leftFootLog");
            //System.IO.File.WriteAllText(pathDirectory  + "leftFootLog.csv", "");
        }
        if (rightHand != null)
        {
            lastBodyPos[2] = rightHand.position;
            bodyStrPath[2] = "rightHandLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "rightHandLog.csv", "");
        }
        if (leftHand != null)
        {
            lastBodyPos[3] = leftHand.position;
            bodyStrPath[3] = "leftHandLog.csv";
            //System.IO.File.WriteAllText(pathDirectory + "leftHandLog.csv", "");
        }
        if (rightShin != null)
        {
            lastBodyPos[4] = rightShin.position;
            bodyStrPath[4] = "rightShinLog.csv";
            //System.IO.File.WriteAllText(pathDirectory + "rightShinLog.csv", "");
        }
        if (leftShin != null)
        {
            lastBodyPos[5] = leftShin.position;
            bodyStrPath[5] = "leftShinLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "leftShinLog.csv", "");
        }
        if (head != null)
        {
            lastBodyPos[4] = head.position;
            bodyStrPath[4] = "headLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "headLog.csv", "");
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
        if (rightFoot != null)
        {
            currentPosVector = rightFoot.transform.position - lastBodyPos[0];
            if (true)
            {
                lastBodyPos[0] = rightFoot.transform.position;
                bodyStr[0] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    rightFoot.transform.position.x.ToString(),
                    rightFoot.transform.position.y.ToString(),
                    rightFoot.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    rightFoot.transform.eulerAngles.x.ToString(),
                    rightFoot.transform.eulerAngles.y.ToString(),
                    rightFoot.transform.eulerAngles.z.ToString(),
                    rightFoot.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[0] , bodyStr[0]);
            }

            //create a file for each part of the body
        }
        if (leftFoot != null)
        {
            currentPosVector = leftFoot.transform.position - lastBodyPos[1];
            if (true)
            {
                //pathHeaderStr = "Task,Trigger,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,rotX,rotY,rotZ,magnitude,CameraPosX,CameraPosY,CameraPosZ,CameraRotX,CameraRotY,CameraRotZ\n";
                lastBodyPos[1] = leftFoot.transform.position;
                bodyStr[1] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    leftFoot.transform.position.x.ToString(),
                    leftFoot.transform.position.y.ToString(),
                    leftFoot.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    leftFoot.transform.eulerAngles.x.ToString(),
                    leftFoot.transform.eulerAngles.y.ToString(),
                    leftFoot.transform.eulerAngles.z.ToString(),
                    leftFoot.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[1] , bodyStr[1]);
            }
        }
        if (rightHand != null)
        {
            currentPosVector = rightHand.transform.position - lastBodyPos[2];
            //currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.head];
            if (true)
            {


                lastBodyPos[2] = rightHand.transform.position;
                bodyStr[(int)BodyLog.rightHand] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    rightHand.transform.position.x.ToString(),
                    rightHand.transform.position.y.ToString(),
                    rightHand.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    rightHand.transform.eulerAngles.x.ToString(),
                    rightHand.transform.eulerAngles.y.ToString(),
                    rightHand.transform.eulerAngles.z.ToString(),
                    rightHand.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[2] , bodyStr[2]);
            }
        }
        if (leftHand != null)
        {
            lastBodyPos[(int)BodyLog.leftHand] = leftHand.transform.position;
            currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.head];
            if (true)
            {
                lastBodyPos[(int)BodyLog.leftHand] = rightHand.transform.position;
                bodyStr[(int)BodyLog.leftHand] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    leftHand.transform.position.x.ToString(),
                    leftHand.transform.position.y.ToString(),
                    leftHand.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    leftHand.transform.eulerAngles.x.ToString(),
                    leftHand.transform.eulerAngles.y.ToString(),
                    leftHand.transform.eulerAngles.z.ToString(),
                    leftHand.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[3] , bodyStr[3]);
            }
        }
        if (rightShin != null)
        {
            lastBodyPos[(int)BodyLog.rightShin] = rightShin.transform.position;
            currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.rightShin];
            if (true)
            {
                lastBodyPos[(int)BodyLog.rightShin] = rightShin.transform.position;
                bodyStr[(int)BodyLog.rightShin] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    rightShin.transform.position.x.ToString(),
                    rightShin.transform.position.y.ToString(),
                    rightShin.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    rightShin.transform.eulerAngles.x.ToString(),
                    rightShin.transform.eulerAngles.y.ToString(),
                    rightShin.transform.eulerAngles.z.ToString(),
                    rightShin.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[4] , bodyStr[4]);
            }
        }
        if (leftShin != null)
        {
            lastBodyPos[(int)BodyLog.leftShin] = leftShin.transform.position;
            currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.leftShin];
            if (true)
            {
                lastBodyPos[(int)BodyLog.leftShin] = leftShin.transform.position;
                bodyStr[(int)BodyLog.leftShin] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    leftShin.transform.position.x.ToString(),
                    leftShin.transform.position.y.ToString(),
                    leftShin.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    leftShin.transform.eulerAngles.x.ToString(),
                    leftShin.transform.eulerAngles.y.ToString(),
                    leftShin.transform.eulerAngles.z.ToString(),
                    leftShin.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory +  bodyStrPath[5] , bodyStr[5]);
            }
        }
        if (head != null)
        {
            lastBodyPos[(int)BodyLog.head] = head.transform.position;
            currentPosVector = head.transform.position - lastBodyPos[(int)BodyLog.head];
            if (true)
            {
                lastBodyPos[(int)BodyLog.head] = head.transform.position;
                bodyStr[(int)BodyLog.head] += string.Join(",", new string[]
                {
                    "ring"+tTask.getCurrentRing().ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    currentPosVector.x.ToString(),
                    currentPosVector.y.ToString(),
                    currentPosVector.z.ToString(),
                    head.transform.eulerAngles.x.ToString(),
                    head.transform.eulerAngles.y.ToString(),
                    head.transform.eulerAngles.z.ToString(),
                    head.transform.position.magnitude.ToString(),
                    head.transform.position.x.ToString(),
                    head.transform.position.y.ToString(),
                    head.transform.position.z.ToString(),
                    Camera.main.transform.eulerAngles.x.ToString(),
                    Camera.main.transform.eulerAngles.y.ToString(),
                    Camera.main.transform.eulerAngles.z.ToString(),
                    "\n"

                });
                //System.IO.File.WriteAllText(pathDirectory + bodyStrPath[6] , bodyStr[6]);
            }
        }
        //flush the string into the file
        if (countFullBodiesStr > 20)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    System.IO.File.AppendAllText(tTask.getPathDirectory() + "/" + bodyStrPath[i], bodyStr[i]);
                    //Debug.Log("&&&&&");
                }
                catch (System.Exception ex)
                {
                    //Debug.LogError("@@@@@@@ : " + bodyStrPath[i]);
                }

                bodyStr[i] = "";
            }
            countFullBodiesStr = 0;
        }
        else
            countFullBodiesStr++;
    }







}
