using UnityEngine;
using System.Collections;
using System.IO;

public enum Tasks { Task1, Task2, Task3, Task4, ReachGreenLollipop0thTime, ReachRedLollipop1stTime, ReachRedLollipop2ndTime, ReachGreenLollipop1stTime, ReachGreenLollipop2ndTime, ThrowingObjects, Completed };

public enum BodyLog {  rightFoot, leftFoot, rightHand, leftHand, head, rightShin, leftShin , hip, torso, };

public enum AvatarType { carlFirstPerson, carlThirdPerson, robotFirstPerson, robotThirdPerson, pointCloudFirstPerson, pointCloudThirdPerson };

public class TestTask : MonoBehaviour {

    public AvatarType avatarType;

    public string collisionLogfileName = "collisionLog";
    public string logFileName = "log";
    public string pathLogFileName = "logPath";

    private string pathDirectory = "";

    private string collisionLogStr = "";
    private string logStr = "";
    private string pathStr = "";

    public string separator = ";";

    public Tasks currentTask;

    
    public GameObject objectsTask1; 
    public GameObject objectsTask2;
    public GameObject objectsTask3;
    public GameObject objectTask4;
    public GameObject pirulito;

    Vector3 lastPos;

    Camera _trackedObj;

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

    public bool fullbodyLog = false;

    int currentTrigger = 0;

    float threshold = 0.00000f;

    float lastTimeBetweenTasks = 0.0f;
    float lastTimeBetweenTriggers = 0.0f;
    float lastTimeBetweenCollisions = 0.0f;
    float startTime = 0.0f;
    int countFullBodiesStr = 0;
    private string pathHeaderStr;

    // Use this for initialization
    void Start () {
        //currentTask = Tasks.Task1;
        //uncomment this for the task
        currentTask = Tasks.ReachGreenLollipop0thTime;
        //with avatars, change for torso tracking
        _trackedObj = Camera.main;
        lastPos = _trackedObj.transform.position;
        InitializeReport();
        
        int i = 1;
        
        while(Directory.Exists(Directory.GetCurrentDirectory()+"/user"+i+avatarType.ToString()))
        {
            i++;
        }
        //se nao houver diretorios

        System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/test");
        System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/user"+i+ avatarType.ToString());
        //System.IO.StreamWriter
        pathDirectory = Directory.GetCurrentDirectory() + "/user" + i + avatarType.ToString() + "/";
        if(fullbodyLog)
            InitializeFullbodyReport();


        //Debug.Log("log");
	}
	
	// Update is called once per frame
	void Update () {
        if (currentTask != Tasks.Completed )
        {
            if(fullbodyLog)
            {
                updateFullbodyReport();
            }
            UpdatePathReport();
        }
        
    }

    float getTaskTime(float time)
    {
        if (time - startTime < 0)
            return 0;
        else
            return time - startTime;
    }

    void serializeCollision(string str)
    {
        float currentTime = Time.realtimeSinceStartup;
        //"$" means collision
        logStr += "$"+currentTask.ToString() + "," + (getTaskTime(currentTime - lastTimeBetweenCollisions)) +  ","+ getTaskTime(currentTime) +   "\n";
        //lastTimeBetweenTasks = currentTime;
        collisionLogStr += str + "," + (getTaskTime(currentTime) - getTaskTime(lastTimeBetweenCollisions)) + "," + getTaskTime(currentTime)+ "," + currentTask + "\n";
        Debug.Log("Time : " + currentTime + " startTime : " + startTime + "lastCollision" + lastTimeBetweenCollisions);
        lastTimeBetweenCollisions = currentTime;
    }

    void serializeBallCollision(string str)
    {
        float currentTime = Time.realtimeSinceStartup;
        logStr += currentTask.ToString() + "," + (currentTime - lastTimeBetweenCollisions) + ","+ currentTime + "\n";
        //lastTimeBetweenTasks = currentTime;
        collisionLogStr += str;
        //Debug.Log("&&" + str);
    }

    void logBallThrow(float time)
    {
        lastTimeBetweenCollisions = time;
        Debug.Log("BetweenCollisions : " + time + ","+ Time.realtimeSinceStartup);
    }


    void InitializeReport()
    {
        collisionLogStr = "Joint" + separator + "PosX" + separator + "PosY" + separator + "PosZ" + separator + "RotX" + separator + "RotY" + separator + "RotZ" + separator +
                        "ColliderName" + separator + "PosColliderX" + separator + "PosColliderY" + separator + "PosColliderZ" + separator + "RotColliderX" + separator + "RotColliderY" +
                        separator + "RotColliderZ" + separator + "ErrorX" + separator + "ErrorY" + separator + "ErrorZ" + separator+ "PositionColliderTransformedX"+ separator + "PositionColliderTransformedY"+separator+"PositionColliderTransformedZ"+ separator +
                        "CameraPositionX"+ separator + "CameraPositionY" + separator + "CameraPositionZ" + separator + "CameraRotationX" + separator + "CameraRotationY" + separator + "CameraRotationZ" + separator + 
                        "TimeElapsed"+ separator + "CurrentTime"+ separator + "CurrentTask"+"\n";
        logStr = "TriggerNum" + separator + "TimeElapsed" + separator + "CurrentTime\n";
        pathStr = "Task,Trigger,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,rotX,rotY,rotZ,magnitude\n";
        pathHeaderStr = "Task,Trigger,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,rotX,rotY,rotZ,magnitude,CameraPosX,CameraPosY,CameraPosZ,CameraRotX,CameraRotY,CameraRotZ\n";
    }

    void CompleteReport()
    {

        logStr += "TotalTime" + getTaskTime(Time.realtimeSinceStartup)+"\n";
        System.IO.File.WriteAllText( pathDirectory+"/"+ collisionLogfileName + ".csv",collisionLogStr);
        System.IO.File.WriteAllText( pathDirectory+"/"+logFileName + ".csv", logStr);
        if(fullbodyLog)
        {
            for (int i = 0; i < 9; i++)
            {
                //System.IO.File.WriteAllText(pathDirectory + "/" + bodyStrPath[i] + ".csv", pathHeaderStr);
                //System.IO.File.WriteAllText(pathDirectory + "/" + bodyStrPath[i] + ".csv", pathHeaderStr + bodyStr[i]);
                //System.IO.File.AppendAllText(pathDirectory + "/" + bodyStrPath[i] + ".csv", "TimeTotal," + "0.355");
            }
            
        }
        //else
        {
            System.IO.File.WriteAllText(pathDirectory + "/" + pathLogFileName + ".csv", pathStr);
        }
        
      
    }

    void UpdateReport( )
    {
        float currentTime = Time.realtimeSinceStartup;
        logStr += currentTask.ToString() + "," + (getTaskTime(currentTime) - getTaskTime(lastTimeBetweenTasks))+ ","+ getTaskTime(currentTime)+"\n";
        lastTimeBetweenTasks = currentTime;
        lastTimeBetweenCollisions = currentTime;
        lastTimeBetweenTriggers = currentTime;
    }

    

    public void InitializeFullbodyReport()
    {
        lastBodyPos = new Vector3[9];
        bodyStrPath = new string[9];
        bodyStr = new string[9];
        
        for(int i = 0; i < 5; i++)
        {
            bodyStr[i] = pathHeaderStr;
        }

        if(rightFoot!=null)
        {
            lastBodyPos[0] = rightFoot.position;
            bodyStrPath[0] = "rightFootLog.csv";
            Debug.Log("rightFootLog");
            //System.IO.File.WriteAllText(pathDirectory  + "rightFootLog.csv", "");
            //create a file for each part of the body
        }
        if(leftFoot!=null)
        {
            //log this
            lastBodyPos[1] = leftFoot.position;
            bodyStrPath[1] = "leftFootLog.csv";
            Debug.Log("leftFootLog");
            //System.IO.File.WriteAllText(pathDirectory  + "leftFootLog.csv", "");
        }
        if(rightHand!=null)
        {
            lastBodyPos[2] = rightHand.position;
            bodyStrPath[2] = "rightHandLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "rightHandLog.csv", "");
        }
        if(leftHand!=null)
        {
            lastBodyPos[3] = leftHand.position;
            bodyStrPath[3] = "leftHandLog.csv";
            //System.IO.File.WriteAllText(pathDirectory + "leftHandLog.csv", "");
        }
        if(rightShin!=null)
        {
            lastBodyPos[4] = rightShin.position;
            bodyStrPath[4] = "rightShinLog.csv";
            //System.IO.File.WriteAllText(pathDirectory + "rightShinLog.csv", "");
        }
        if(leftShin!=null)
        {
            lastBodyPos[5] = leftShin.position;
            bodyStrPath[5] = "leftShinLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "leftShinLog.csv", "");
        }
        if(head!=null)
        {
            lastBodyPos[4] = head.position;
            bodyStrPath[4] = "headLog.csv";
            //System.IO.File.WriteAllText(pathDirectory  + "headLog.csv", "");
        }
        

        
    }

    void updateFullbodyReport()
    {
        if(head == null)
        {
            GameObject headObj = new GameObject("head");
            head = headObj.transform;
            headObj.transform.position = Vector3.zero;
        }
        Vector3 currentPosVector = new Vector3();
        if (rightFoot != null)
        {
            currentPosVector = rightFoot.transform.position - lastBodyPos[0];
            if (true )
            {
                lastBodyPos[0] = rightFoot.transform.position;
                bodyStr[0] += string.Join(",", new string[]
                {
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                bodyStr[1] +=  string.Join(",", new string[]
                {
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                bodyStr[(int)BodyLog.rightShin]  += string.Join(",", new string[]
                {
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                    currentTask.ToString(),
                    currentTrigger.ToString(),
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
                    System.IO.File.AppendAllText(pathDirectory + "/"+bodyStrPath[i], bodyStr[i]);
                    //Debug.Log("&&&&&");
                }
                catch(System.Exception ex)
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

    void UpdatePathReport()
    {
        Vector3 currentPos = Camera.main.transform.position;
        Vector3 currentPosVector = currentPos - lastPos;
        if (currentPosVector.magnitude > threshold)
        {
            lastPos = Camera.main.transform.position;
            pathStr += string.Join(",", new string[]
            {
                currentTask.ToString(),
                currentTrigger.ToString(),
                _trackedObj.transform.position.x.ToString(),
                _trackedObj.transform.position.y.ToString(),
                _trackedObj.transform.position.z.ToString(),
                currentPosVector.x.ToString(),
                currentPosVector.y.ToString(),
                currentPosVector.z.ToString(),
                _trackedObj.transform.eulerAngles.x.ToString(),
                _trackedObj.transform.eulerAngles.y.ToString(),
                _trackedObj.transform.eulerAngles.z.ToString(),
                currentPosVector.magnitude.ToString(),
                "\n"

            }); 
                
            //countPointsInPath++;
            //cameraPath.Add(new Vector3(currentPos.x, currentPos.y, currentPos.z));

           
        }
        else
        {
            //do nothing
        }
    }

    

    void SetActiveChildren(GameObject obj, bool state)
    {
        obj.SetActive(state);
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(state);
        }
    }

    void nextTask(string triggerId)
    {
        if(triggerId == "walkTrigger1")
        {
            if(currentTask == Tasks.Task2)
            {
                UpdateReport();
                currentTask = Tasks.ReachGreenLollipop1stTime;
                Debug.Log(currentTask.ToString());
                
                //objectsTask2.SetActive(false);
                //objectsTask3.SetActive(true);
                //Debug.Log("TEST OVER");
                //CompleteReport();
            }
        }

        if(triggerId == "walkTrigger2")
        {
            if(currentTask == Tasks.Task1)
            {
                UpdateReport();
                currentTask = Tasks.ReachRedLollipop1stTime;
                Debug.Log(currentTask.ToString());
            }
            else if(currentTask == Tasks.Task3)
            {
                UpdateReport();
                currentTask = Tasks.ReachRedLollipop2ndTime;
                Debug.Log(currentTask.ToString());

                //currentTask = Tasks.Completed;
                //CompleteReport();
            }
        }
        if(triggerId == "redLollipop")
        {
            
            if(currentTask == Tasks.ReachRedLollipop1stTime)
            {
                UpdateReport();
                currentTask = Tasks.Task2;
                SetActiveChildren(objectsTask1, false);
                SetActiveChildren(objectsTask2, true);
                Debug.Log(currentTask.ToString());
            }
            else if(currentTask == Tasks.ReachRedLollipop2ndTime)
            {
                UpdateReport();
                lastTimeBetweenCollisions = Time.realtimeSinceStartup;
                currentTask = Tasks.ThrowingObjects;
                Debug.Log("Start Throwing Object Task");
                SetActiveChildren(objectsTask3, false);
                SetActiveChildren(objectTask4,true);
                SetActiveChildren(pirulito, false);
                //CompleteReport();
                //throwing objectsToBeImplemented
            }
        }
        else if(triggerId == "greenLollipop")
        {
            if (currentTask == Tasks.ReachGreenLollipop0thTime)
            {
                currentTask = Tasks.Task1;
                SetActiveChildren(objectsTask1, true);
                Debug.Log(currentTask.ToString());
                lastTimeBetweenTasks = Time.realtimeSinceStartup;
                startTime = lastTimeBetweenTasks;
                Debug.Log("startTime = " + startTime);
            }
            if (currentTask == Tasks.ReachGreenLollipop1stTime)
            {
                UpdateReport();
                currentTask = Tasks.Task3;
                SetActiveChildren(objectsTask2, false);
                SetActiveChildren(objectsTask3, true);
            }
        }
        else if( triggerId == "objectShooter")
        {
            if(currentTask == Tasks.ThrowingObjects)
            {
                SetActiveChildren(objectTask4, false);
                CompleteReport();
                currentTask = Tasks.Completed;
            }
        }
    }
}
