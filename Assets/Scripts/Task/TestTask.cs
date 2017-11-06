using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum NavigationState
{
    Idle, Walking, Flying, WalkingAndFlying,
    Neutral
};


public class TestTask : MonoBehaviour
{
    public enum Technique
    {
        HandSteering, GazeOriented, VirtualCircle, WalkingInPlace, HandGaze, Neutral, ElevatorGaze, ImagePlane,AnalogSteering
    };

    

    [SerializeField]
    private GameObject[] rings;
    public GameObject[] Rings
    {
        get
        {
            return rings;
        }

        set
        {
            rings = value;
        }
    }
    public int countTriggersExit = 0;

    private float[] magnitudes;
    private Vector3[] ringPositionsWhenCrossed;
    private int[] numberOfPathPointsPerRing;

    private int currentRing;

    private string testReport = "";
    private string testReportPath = "";
    private string testOptimalPath = "";
    private string testCollision = "";
    private float lastRingTime;
    private float lastTime2;
    private float totalTime = 0;

    public bool started = false;

    private Dictionary<string, TimePerRing> dictionaryForDiscriminatedTimes;


    private List<Vector3> optimalDiscretizedPathList;

    [SerializeField]
    private string reportOutputFile = "report.csv";

    [SerializeField]
    private string reportOutputFile2 = "reportDiscriminateTimes.csv";

    [SerializeField]
    private string pathReportOutputFile = "reportPath.csv";

    [SerializeField]
    private string optimalPathOutputFile = "optimalCPath.csv";

    [SerializeField]
    private string collisionFile = "collisionLog.csv";

    private Vector3 lastPos;

    [SerializeField]
    private Texture2D arrow;

    [SerializeField]
    private float threshold = 0.05f;

    private Camera mainCamera;

    //testes
    private int countPointsInPath = 0;

    private bool completed = false;

    private string pathDirectory;

    private List<Vector3> cameraPath;

    public Technique travelTechnique;

    public bool rightHanded;// = true;

    private KeyCode calibrateButton;

    private KeyCode forwardButton;

    private KeyCode upButton;

    private KeyCode downButton;

    public bool training;

    [SerializeField]
    private int collisionCount;

    [SerializeField]
    private float timeCollidingWithStuff;

    private string colliderName = "";

    Dictionary<string, List<ActiveCollision>> activeCollisions;
    List<FinishedCollision> finishedCollisions;
    Dictionary<string,FinishedCollision> finishedCollisionsAux;

    FinishedCollision finishedAux;

    Dictionary<string, ActiveCollision> collisionsPerJoint;

    string dominantHandText = "rightHanded";

    private float ringTolerance = 0.7f;

    private float currrentSpeed = 3.0f;

    public string wipStr ="0";

    private NavigationState currentNavState;
    private float lastTimeIdle;
    private float lastTimeFlying;

    private float finishTime = 0.0f;

    public string getSpeedWIP()
    {
        string str = "";
        if(getCurrentTechnique().Equals(Technique.WalkingInPlace))
        {
            return wipStr.ToString();
        }
        else
        {
            return "NOTWIP";
        }
    }

    public float getRingTolerance()
    {
        return ringTolerance;
    }

    public float getCurrentSpeed()
    {
        return currrentSpeed;
    }

    public void setSpeed(float speed)
    {
        this.currrentSpeed = speed;
    }


    public NavigationState getCurrentNavigationState()
    {
        return currentNavState;
    }


    public void setNavigationState(NavigationState navState)
    {
        currentNavState = navState;
    }

    public void setNavigationState(bool isFlying, float currentCircleSpeed, float lastCircleSpeed)
    {
        float threshold = 0.001f;
        NavigationState navStateTemp = currentNavState;
        float timestamp = Time.realtimeSinceStartup;
        if(isFlying)
        {
            
            currentNavState = NavigationState.Flying;
            lastTimeFlying = 0.0f;
            
        }
        else
        {
            
            currentNavState = NavigationState.Idle;
            lastTimeIdle = 0.0f;
        }

        if(getCurrentRing() < rings.Length && training == false)
        {
            
            dictionaryForDiscriminatedTimes["ring" + getCurrentRing()].Add(currentNavState, timestamp - lastTime2);
            
            
            lastTime2 = timestamp;
        }
        


       // Debug.Log("Current Nav State +" + currentNavState.ToString() + " bla = "+ Mathf.Abs(currentCircleSpeed - lastCircleSpeed));
    }

    public Technique getCurrentTechnique()
    {
        Technique technique = Technique.Neutral;
        GazeOriented gaze = GetComponent<GazeOriented>();
        ElevatorGaze elevator = GetComponent<ElevatorGaze>();
        HandSteering handGaze = GetComponent<HandSteering>();
        PointAndGo imgPlane = GetComponent<PointAndGo>();
        WIPSteering wip = GetComponent<WIPSteering>();
        AnalogSteering analogSteering = GetComponent<AnalogSteering>();
        VirtualCircle virtualCircle = GetComponent<VirtualCircle>();

        if (gaze)
        {
            if (gaze.enabled == true)
            {
                technique = Technique.GazeOriented;
            }
        }
        if (handGaze)
        {
            if (handGaze.enabled == true)
            {
                technique = Technique.HandSteering;
            }
        }
        if (elevator)
        { 
            if (elevator.enabled == true)
            {
                technique = Technique.ElevatorGaze;
            }
        }
        if(wip)
        {
            if(wip.enabled == true)
            {
                technique = Technique.WalkingInPlace;
            }
        }
        if(virtualCircle)
        {
            if(virtualCircle.enabled == true)
            {
                technique = Technique.VirtualCircle;
            }
        }
        if(analogSteering)
        {
            if(analogSteering.enabled == true)
            {
                technique = Technique.AnalogSteering;
            }
        }
        

        return technique;
    }

    public void collisionStarted(string colliderName, string jointName,float time)
    {
        if(activeCollisions.ContainsKey(colliderName))
        {
            if (activeCollisions[colliderName].Count > 0)
            {
                activeCollisions[colliderName].Add(new ActiveCollision(jointName, time, false));
            }
            else
            {
                activeCollisions[colliderName].Add(new ActiveCollision(jointName, time, true));
            }
        }
        else
        {
            activeCollisions[colliderName] = new List<ActiveCollision>();
            activeCollisions[colliderName].Add(new ActiveCollision(jointName, time,true));
            Debug.Log("Empilhando " + jointName + " em " + colliderName + " no tempo : "+ time);

        }
    }

    public void collisionEnded(string colliderName, string jointName, float time)
    {
        if(finishTime>0.0f)
        {
            return;
        }

        if(activeCollisions.ContainsKey(colliderName) && finishTime == 0.0f)
        {
            List<ActiveCollision> actColList = new List<ActiveCollision>(activeCollisions[colliderName]);
            for(int i = 0; i < activeCollisions[colliderName].Count;i++)
            {
                if(activeCollisions[colliderName][i].jointName == jointName)
                {
                    /*if (finishedAux == null)
                        finishedAux = new FinishedCollision("", 0);*/

                    //add to joint collision dictionary
                    if (!collisionsPerJoint.ContainsKey(jointName))
                    {
                        ActiveCollision colAux = new ActiveCollision(jointName);
                        colAux.collisionCount++;
                        colAux.timeInit += (time - activeCollisions[colliderName][i].timeInit);
                        collisionsPerJoint.Add(jointName, colAux);
                    }
                    else
                    {
                        collisionsPerJoint[jointName].timeInit += (time - activeCollisions[colliderName][i].timeInit);
                        collisionsPerJoint[jointName].collisionCount++;
                    }


                    if (activeCollisions[colliderName][i].first)
                    {
                        if (!finishedCollisionsAux.ContainsKey(colliderName))
                        {
                            finishedCollisionsAux.Add(colliderName,new FinishedCollision("", 0));
                        }
                        else if(finishedCollisionsAux[colliderName] == null)
                        {
                            finishedCollisionsAux[colliderName] = new FinishedCollision("", 0);
                        }
                        else
                        {

                        }

                        finishedCollisionsAux[colliderName].startTime = activeCollisions[colliderName][i].timeInit;
                        finishedCollisionsAux[colliderName].finishTime = time;
                        finishedCollisionsAux[colliderName].colliderName = colliderName;


                        activeCollisions[colliderName].Remove(activeCollisions[colliderName][i]);
                        
                    }
                    else
                    {
                        if (!finishedCollisionsAux.ContainsKey(colliderName))
                            finishedCollisionsAux[colliderName] = new FinishedCollision("", 0);
                        finishedCollisionsAux[colliderName].finishTime = time;
                        finishedCollisionsAux[colliderName].colliderName = colliderName;
                        activeCollisions[colliderName].Remove(activeCollisions[colliderName][i]);
                    }
                    

                    if (activeCollisions[colliderName].Count == 0)
                    {
                        finishedCollisions.Add(finishedCollisionsAux[colliderName]);

                        timeCollidingWithStuff += (finishedCollisionsAux[colliderName].finishTime - finishedCollisionsAux[colliderName].startTime);
                        //finishedCollisions.FindIndex()
                        //finishedCollisionsAux[colliderName] = null;
                        finishedCollisionsAux.Remove(colliderName);
                    }
                    
                }
             }
        }
    }


    /*public void incrementCollidedTime(float timeCollided,string colliderName,string jointName)
    {
        //if (colliderName != this.colliderName)
        if(!collisionTimePerJoint.ContainsKey(jointName))
        {
            collisionTimePerJoint.Add(jointName, timeCollided);
        }
        else
        {
            collisionTimePerJoint[jointName] += timeCollided;
        }

            timeCollidingWithStuff += timeCollided;
        Debug.Log("Collision between  " + colliderName + " and " + jointName);

        colliderName = "";    
    }*/

    public KeyCode getCalibrateButton()
    {
        return calibrateButton;
    }

    public KeyCode getForwardButton()
    {
        return forwardButton;
    }

    public KeyCode getUpButton()
    {
        return upButton;
    }

    public KeyCode getDownButton()
    {
        return downButton;
    }

    public void iAmAnActiveCollision(string colliderName, string jointName)
    {
        if(activeCollisions.ContainsKey(jointName))
        {
            List<ActiveCollision> act = activeCollisions[jointName];
            if (act == null)
            {
                act = new List<ActiveCollision>();
                act.Add(new ActiveCollision(colliderName));
            }
            else
            {
                foreach (ActiveCollision activeCol in act)
                {

                }
            }
            
        }
    }

    void Awake()
    {
        if(training)
        {
            FullbodyReport fb = this.GetComponent<FullbodyReport>();
            if (fb)
                fb.enabled = false;
        }
    }
   

    private void Start()
    {
        mainCamera = Camera.main;
        magnitudes = new float[42];
        numberOfPathPointsPerRing = new int[42];
        optimalDiscretizedPathList = new List<Vector3>();
        dictionaryForDiscriminatedTimes = new Dictionary<string, TimePerRing>();

        setSpeed(0);

        cameraPath = new List<Vector3>();

        int i = 1;

        travelTechnique = getCurrentTechnique();
        currentNavState = NavigationState.Idle;

        while (Directory.Exists(Directory.GetCurrentDirectory() + "/user" + i+"_"+travelTechnique.ToString()))
        {
            i++;
        }
        //se nao houver diretorios

        //System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/test");
        
        //System.IO.StreamWriter

        pathDirectory = Directory.GetCurrentDirectory() + "/user" + i + "_" + travelTechnique.ToString();// + "/";
        if(training == false)
        {
            InitializeRings();
            InitializeReport();
            pathDirectory += "/";
            System.IO.Directory.CreateDirectory(pathDirectory);
            System.IO.Directory.CreateDirectory(pathDirectory + "/fullbodyLog/");
        }


        

        forwardButton = KeyCode.PageUp;
        calibrateButton = KeyCode.PageDown;
        upButton = KeyCode.UpArrow;
        downButton = KeyCode.DownArrow;

        activeCollisions = new Dictionary<string, List<ActiveCollision>>();
        finishedCollisions = new List<FinishedCollision>();
        finishedCollisionsAux = new Dictionary<string, FinishedCollision>();
        collisionsPerJoint = new Dictionary<string, ActiveCollision>();
       
        //optimalDiscretizedPathList = new List<Vector3>();
    }

    private void OnGUI()
    {
        if (arrow == null)
            return;

        if (currentRing < rings.Length && !rings[currentRing].GetComponent<Renderer>().isVisible)
            DrawArrow();

        GUI.Label(new Rect(10, 10, 100, 20), dominantHandText);

    }

    public string getPathDirectory()
    {
        return pathDirectory;
    }

    private void DrawArrow()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(rings[currentRing].transform.position);
        if (screenPosition.z < 0)
            screenPosition *= -1;

        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2.0f;
        screenPosition -= screenCenter;
        screenPosition = screenPosition.normalized;

        float angle = Mathf.Atan2(screenPosition.x, screenPosition.y) * Mathf.Rad2Deg;

        Matrix4x4 matrixBackup = GUI.matrix;
        GUIUtility.RotateAroundPivot(angle, new Vector2(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2));
        GUI.DrawTexture(new Rect(mainCamera.pixelWidth / 2 - 100, mainCamera.pixelHeight / 2 - 100, 200, 200), arrow, ScaleMode.ScaleToFit, true);
        GUI.matrix = matrixBackup;
    }



    private void InitializeReport()
    {
        testReport += "Ring,Hit,Time,Error,PosRingX,PosRingY,PosRingZ,RawTime,RingTime,DominantHand,NavigationState,Speed\n";
        testCollision += "Joint,PosX,PosY,PosZ,RotX,RotY,RotZ,ColliderName,PosColliderX,PosColliderY,PosColliderZ,RotColliderX,RotColliderY,RotColliderZ,ErrorX,ErrorY,ErrorZ,Error2X,Error2Y,Error2Z,headPosX,headPosY,headPosZ,cameraPosX,cameraPosY,cameraPosZ,TimeElapsed,TimeStart,TimeFinish\n";

        /*string str = string.Join(",", new string[]
        {
            "#"+collider.gameObject.name,
            //this.Id,
            pos.x.ToString(),
            pos.y.ToString(),
            pos.z.ToString(),
            rot.x.ToString(),
            rot.y.ToString(),
            rot.z.ToString(),
            this.Id,
            this.transform.position.x.ToString(),
            this.transform.position.y.ToString(),
            this.transform.position.z.ToString(),
            this.transform.eulerAngles.x.ToString(),
            this.transform.eulerAngles.y.ToString(),
            this.transform.eulerAngles.z.ToString(),
            vec.x.ToString(),
            vec.y.ToString(),
            vec.z.ToString(),
            vec2.x.ToString(),
            vec2.y.ToString(),
            vec2.z.ToString(),
            headPos.x.ToString(),
            headPos.y.ToString(),
            headPos.z.ToString(),
            Camera.main.transform.position.x.ToString(),
            Camera.main.transform.position.y.ToString(),
            Camera.main.transform.position.z.ToString(),
            triggerTime.ToString(),
            timeWhenCollisionStarted.ToString(),
            currentTime.ToString(),
            "\n"
        });*/


        float initTime = Time.realtimeSinceStartup;
        testReportPath += "Ring,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,magnitude,rotX,rotY,rotZ\n";
        //lastTime = Time.realtimeSinceStartup;
        lastPos = new Vector3(Camera.main.transform.position.x,
                              Camera.main.transform.position.y,
                              Camera.main.transform.position.z);
        testReport += ("-1,True,0,0," + ringPositionsWhenCrossed[0].x + "," + ringPositionsWhenCrossed[0].y + "," + ringPositionsWhenCrossed[0].z + ","+ initTime+ "\n");

    }

    private float Average(float[] array)
    {
        float sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return (sum) / array.Length;
    }

    private void UpdatePathReport()
    {
        Vector3 currentPos = this.transform.position;
        Vector3 currentPosVector = currentPos - lastPos;
        if (currentPosVector.magnitude > threshold)
        {
            lastPos = this.transform.position;
            testReportPath += currentRing + "," + currentPos.x + "," + currentPos.y + "," + currentPos.z + "," + currentPosVector.x + "," + currentPosVector.y + "," +
                              currentPosVector.z + "," + currentPosVector.magnitude + "," + Camera.main.transform.localEulerAngles.x + "," + Camera.main.transform.localEulerAngles.y + "," + Camera.main.transform.localEulerAngles.z + "," + getCurrentNavigationState() + ","+ getCurrentSpeed() +"\n";
            //Debug.Log("MAGNITUDE + " + currentPosVector.magnitude);
            countPointsInPath++;
            cameraPath.Add(new Vector3(currentPos.x, currentPos.y, currentPos.z));

            if (!completed)
            {
                try
                {
                    magnitudes[currentRing] += currentPosVector.magnitude;
                }
                catch (Exception e)
                {
                    Debug.Log("Exception on ring stuff");
                }

            }
            else
            {
                magnitudes[currentRing] += currentPosVector.magnitude;
            }
        }
        else
        {
            //do nothing
        }
    }

    private void UpdateReport(bool hit, float distanceToRingCenter)
    {
        float currentTime = Time.realtimeSinceStartup;
        if (currentRing == 0)
            lastRingTime = currentTime;

            float ringTime = currentTime - lastRingTime;
            lastRingTime = currentTime;
            totalTime += ringTime;

            testReport += string.Join(",", new string[]{
            currentRing.ToString(),
            hit.ToString(),
            ringTime.ToString(),
            distanceToRingCenter.ToString(),
            ringPositionsWhenCrossed[currentRing].x.ToString(),
            ringPositionsWhenCrossed[currentRing].y.ToString(),
            ringPositionsWhenCrossed[currentRing].z.ToString(),
            currentTime.ToString(),
            ringTime.ToString(),
            dominantHandText
        }) + "\n";
    }

    private List<Vector3> discretizePath(Vector3 pointA, Vector3 pointB, int averagePointsPerRing)
    {
        Vector3 aux = pointB - pointA;
        List<Vector3> discretizedPath = new List<Vector3>();
        aux = new Vector3(aux.x / averagePointsPerRing, aux.y / averagePointsPerRing, aux.z / averagePointsPerRing);
        for (int j = 0; j <= averagePointsPerRing; j++)
        {
            optimalDiscretizedPathList.Add(pointA + (aux * j));
        }
        return optimalDiscretizedPathList;
    }

    private List<Vector3> discretizePath(Vector3[] points, int averagePointsPerRing)
    {
        List<Vector3> discretizedPath = new List<Vector3>();
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 aux = points[i + 1] - points[i];
            //discretizedPath.Add(points[i]);
            aux = new Vector3(aux.x / averagePointsPerRing, aux.y / averagePointsPerRing, aux.z / averagePointsPerRing);

            //aux.
            for (int j = 0; j <= averagePointsPerRing; j++)
            {
                // aux = points[i] + (aux * j);
                discretizedPath.Add(points[i] + (aux * j));
                //Debug.Log(discretizedPath[j].ToString());
            }
        }
        return discretizedPath;
    }

    private void CompleteReport()
    {
        testReport += ("Total,,,," + "" + "," + "" + "," + ","+ totalTime + "," + finishTime +"\n");
        System.IO.File.WriteAllText(pathDirectory+ reportOutputFile, testReport);
        System.IO.File.WriteAllText(pathDirectory + pathReportOutputFile, testReportPath);


        testCollision += "!ObjectsCollided,"+finishedCollisions.Count+",TotalTimeCollided(s),"+ timeCollidingWithStuff+"\n";
        foreach(KeyValuePair<string,ActiveCollision> collPerJoint in collisionsPerJoint)
        {
            testCollision += "%JointName," + collPerJoint.Key + "," + "CollisionCount," + collPerJoint.Value.collisionCount + ",TimeCollided," + collPerJoint.Value.timeInit + "\n";
        }

        System.IO.File.WriteAllText(pathDirectory + collisionFile, testCollision); 
        completed = true;
        //Debug.Log("countPointsInPath :" + countPointsInPath + " perRing : " + countPointsInPath / 42.0f);
        List<Vector3> optimalPathDiscretizedList1 = discretizePath(ringPositionsWhenCrossed, countPointsInPath / 42);

        printPathToFile(optimalDiscretizedPathList, optimalPathOutputFile);

        //test this
        System.IO.File.AppendAllText(pathDirectory + reportOutputFile2, "ring,timeIdle,timeFlying,timeWalking,timeWalkingAndFlying," +
                        "timeFlyingTotal,TimeIdleTotal\n");
        foreach(KeyValuePair<string, TimePerRing> valuePair in dictionaryForDiscriminatedTimes)
        {
            System.IO.File.AppendAllText(pathDirectory + reportOutputFile2, valuePair.Key + ","+  valuePair.Value.getFormattedString());
        }

    }

    private void CompletePathReport()
    {
        for (int i = 0; i < 42; i++)
        {
            testReportPath += i + " (TOTAL)," + "," + "," + "," + "," + "," + "," + magnitudes[i] + "," + "," + "," + "\n";
        }
        System.IO.File.WriteAllText(pathDirectory + pathReportOutputFile, testReportPath);
    }

    private void printPathToFile(List<Vector3> listPoints, string nameOfPointArray)
    {
        string str = "";
        str += (nameOfPointArray + "posX," + nameOfPointArray + "posY," + nameOfPointArray + "posZ," + "Ring\n");
        for (int i = 0; i < listPoints.Count; i++)
        {
            Vector3 vec = listPoints[i];
            str += vec.x + "," + vec.y + "," + vec.z + "," + (i / 78) + "\n";//TESTE
        }
        System.IO.File.WriteAllText(pathDirectory+ nameOfPointArray, str);
        Debug.Log("PRINTEI TUDOOO");
    }

    private void InitializeRings()
    {
        currentRing = 0;
        for (int i = 0; i < rings.Length; i++)
        {
            bool activated = (i == currentRing);
            rings[i].SetActive(activated);
            dictionaryForDiscriminatedTimes.Add("ring" + i, new TimePerRing());
        }
        ringPositionsWhenCrossed = new Vector3[rings.Length + 1];
        //first we store the initial position of the guy
        //remember!!! this array has a length of numOfRings+1, so here the variable currentRing here is represented by currentRing+1
        ringPositionsWhenCrossed[0] = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    public void Next(bool hit, float distanceToRingCenter)
    {
        rings[currentRing].SetActive(false);

        try
        {
            ringPositionsWhenCrossed[currentRing] = new Vector3(rings[currentRing].transform.position.x, rings[currentRing].transform.position.y, rings[currentRing].transform.position.z);
            if (currentRing != 0)
            {
                discretizePath(ringPositionsWhenCrossed[currentRing - 1], ringPositionsWhenCrossed[currentRing], 78);
            }
        }
        catch (Exception e)
        {

        }
        
        UpdateReport(hit, distanceToRingCenter);

        currentRing++;

        //UpdatePathReport();
        if (rings.Length > currentRing)
        {
            rings[currentRing].SetActive(true);
            if(currentRing+1 > rings.Length)
            {
                rings[currentRing].transform.LookAt(rings[currentRing + 1].transform, rings[currentRing].transform.up);
            }
        }
            
        else
        {
            finishTime = Time.realtimeSinceStartup;
        }

    }

    public void OnDisable()
    {
        if (training == false)
        {
            CompleteReport();
            CompletePathReport();
        }

    }

    public void Update()
    {
        if (completed == false)
        {
            UpdatePathReport();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rightHanded = !rightHanded;
                if (rightHanded == true)
                {
                    Debug.Log("#############rightHanded#############");
                    dominantHandText = "rightHanded";
                }
                else
                {
                    Debug.Log("############leftHanded##############");
                    dominantHandText = "leftHanded";
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Z Axis") > 0.002f)
        {
            started = true;
        }
    }

    public void serializeCollision(string str)
    {
        testCollision += str;
        System.IO.File.AppendAllText(pathDirectory + collisionFile, str);
    }

    public int getCurrentRing()
    {
        return currentRing;
    }
}
