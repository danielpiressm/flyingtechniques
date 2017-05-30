using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TestTask : MonoBehaviour
{

    [SerializeField]
    private GameObject[] rings;

    private float[] magnitudes;
    private Vector3[] ringPositionsWhenCrossed;
    private int[] numberOfPathPointsPerRing;

    private int currentRing;

    private string testReport = "";
    private string testReportPath = "";
    private string testOptimalPath = "";
    private float lastTime;

    private List<Vector3> optimalDiscretizedPathList;

    [SerializeField]
    private string reportOutputFile = "report.csv";

    [SerializeField]
    private string pathReportOutputFile = "reportPath.csv";

    [SerializeField]
    private string optimalPathOutputFile = "optimalPath.csv";

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

    private void Start()
    {
        mainCamera = Camera.main;
        magnitudes = new float[42];
        numberOfPathPointsPerRing = new int[42];
        optimalDiscretizedPathList = new List<Vector3>();
        InitializeRings();
        InitializeReport();

        cameraPath = new List<Vector3>();

        int i = 1;

        while (Directory.Exists(Directory.GetCurrentDirectory() + "/user" + i))
        {
            i++;
        }
        //se nao houver diretorios

        System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/test");
        System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/user" + i);
        //System.IO.StreamWriter
        pathDirectory = Directory.GetCurrentDirectory() + "/user" + i + "/";

    }

    private void OnGUI()
    {
        if (arrow == null)
            return;

        if (currentRing < rings.Length && !rings[currentRing].GetComponent<Renderer>().isVisible)
            DrawArrow();
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
        testReport += "Ring,Hit,Time,Error,PosRingX,PosRingY,PosRingZ\n";
        lastTime = Time.realtimeSinceStartup;
        testReportPath += "Ring,currentPosX,currentPosY,currentPosZ,pathElapsedX,pathElapsedY,pathElapsedZ,magnitude,rotX,rotY,rotZ,navSpeed\n";
        lastTime = Time.realtimeSinceStartup;
        lastPos = new Vector3(Camera.main.transform.position.x,
                              Camera.main.transform.position.y,
                              Camera.main.transform.position.z);
        testReport += ("-1,True,0,0," + ringPositionsWhenCrossed[0].x + "," + ringPositionsWhenCrossed[0].y + "," + ringPositionsWhenCrossed[0].z + "\n");

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
        Vector3 currentPos = Camera.main.transform.position;
        Vector3 currentPosVector = currentPos - lastPos;
        if (currentPosVector.magnitude > threshold)
        {
            lastPos = Camera.main.transform.position;
            testReportPath += currentRing + "," + currentPos.x + "," + currentPos.y + "," + currentPos.z + "," + currentPosVector.x + "," + currentPosVector.y + "," +
                              currentPosVector.z + "," + currentPosVector.magnitude + "," + Camera.main.transform.localEulerAngles.x + "," + Camera.main.transform.localEulerAngles.y + "," + Camera.main.transform.localEulerAngles.z + "\n";
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
        float ringTime = currentTime - lastTime;
        lastTime = currentTime;

        testReport += string.Join(",", new string[]{
            currentRing.ToString(),
            hit.ToString(),
            ringTime.ToString(),
            distanceToRingCenter.ToString(),
            ringPositionsWhenCrossed[currentRing].x.ToString(),
            ringPositionsWhenCrossed[currentRing].y.ToString(),
            ringPositionsWhenCrossed[currentRing].z.ToString()
        }) + "\n";
    }

    private List<Vector3> discretizePath(Vector3 pointA, Vector3 pointB, int averagePointsPerRing)
    {
        Vector3 aux = pointB - pointA;
        //List<Vector3> discretizedPath = new List<Vector3>();
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
        System.IO.File.WriteAllText(pathDirectory+reportOutputFile, testReport);
        System.IO.File.WriteAllText(pathDirectory + pathReportOutputFile, testReportPath);
        completed = true;
        //Debug.Log("countPointsInPath :" + countPointsInPath + " perRing : " + countPointsInPath / 42.0f);
        //List<Vector3> optimalPathDiscretizedList1 = discretizePath(ringPositionsWhenCrossed, countPointsInPath / 42);

        printPathToFile(optimalDiscretizedPathList, "optimalPath");

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
        System.IO.File.WriteAllText(pathDirectory+ nameOfPointArray + ".csv", str);
    }

    private void InitializeRings()
    {
        currentRing = 0;
        for (int i = 0; i < rings.Length; i++)
        {
            bool activated = (i == currentRing);
            rings[i].SetActive(activated);
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
            rings[currentRing].SetActive(true);
        else
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
        }
    }

    public int getCurrentRing()
    {
        return currentRing;
    }
}
