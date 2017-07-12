using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSVPrinter : MonoBehaviour {

    [SerializeField]
    private string fileName ="tracker_info.csv"; 

    private string pathDirectory;

    public GameObject optitrackGO;

    public GameObject trackerRightHandGO;

    private string logStr = "";

    private bool reportCompleted = false;

    private int countStrings = 0;

    // Use this for initialization
    void Start () {
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

    void InitializeReport()
    {
        logStr += String.Join(",", new string[]
        {
            "OptitrackTimeStamp",
            "OptitrackPosX",
            "OptitrackPosY",
            "OptitrackPosZ",
            "OptitrackRotX",
            "OptitrackRotY",
            "OptitrackRotZ",
            "TrackerTimeStamp",
            "TrackerPosX",
            "TrackerPosY",
            "TrackerPosZ",
            "TrackerRotX",
            "TrackerRotY",
            "TrackerRotZ\n"
        });


    }


    void CompleteReport()
    {
        System.IO.File.AppendAllText(pathDirectory + fileName, logStr);
        reportCompleted = true;
    }

    void UpdateReport()
    {
        float time = Time.realtimeSinceStartup;
        
        logStr += String.Join(",", new string[]
        {
            time.ToString(),
            optitrackGO.transform.position.x.ToString(),
            optitrackGO.transform.position.y.ToString(),
            optitrackGO.transform.position.z.ToString(),
            optitrackGO.transform.eulerAngles.x.ToString(),
            optitrackGO.transform.eulerAngles.y.ToString(),
            optitrackGO.transform.eulerAngles.z.ToString(),
            time.ToString(),
            trackerRightHandGO.transform.position.x.ToString(),
            trackerRightHandGO.transform.position.y.ToString(),
            trackerRightHandGO.transform.position.z.ToString(),
            trackerRightHandGO.transform.eulerAngles.x.ToString(),
            trackerRightHandGO.transform.eulerAngles.y.ToString(),
            trackerRightHandGO.transform.eulerAngles.z.ToString()
        });
        countStrings++;
        if(countStrings > 20)
        {
            System.IO.File.AppendAllText(pathDirectory + fileName, logStr);
            logStr = "";
            countStrings = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.A))
        {
            CompleteReport();
        }

        if (!reportCompleted)
            UpdateReport();
        	
	}
}
