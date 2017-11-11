using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class Speed : MonoBehaviour {

    void doStuff()
    {
        string header = "";
        int counter = 0;
        string strForFirstFile = "";
        string strForSecondFile = "User,Technique,";
        string user = "";
        string technique = "";
        string[] arrayLine = null;

        string rootPath = "D:\\Dropbox\\doutorado\\papers\\vrstFlying\\output\\2ndTest";
        string filePath = "D:\\Dropbox\\doutorado\\papers\\vrstFlying\\output\\2ndTest\\torso.csv";

        if(File.Exists(rootPath+"\\speedGraph.csv"))
        {
            File.Delete(rootPath + "\\speedGraph.csv");
        }
        if (File.Exists(rootPath + "\\speedComparison.csv"))
        {
            File.Delete(rootPath + "\\speedComparison.csv");
        }

        System.IO.StreamReader file = new System.IO.StreamReader(filePath);

        header = file.ReadLine();
        string[] arrayHeader = header.Split(',');
        int indexSpeed = 0;
        int indexTime = 0;
        Dictionary<string, List<string>> dictionaryFor2ndFile = new Dictionary<string, List<string>>();
        Dictionary<string, SpeedDataPerUser> dictionaryUserSpeed = new Dictionary<string, SpeedDataPerUser>();

        foreach (string field in arrayHeader)
        {
            if(field.Contains("Time"))
            {
                indexTime = counter;

            }
            if (field.Contains("speed"))
            {
                indexSpeed = counter;
                break;
            }
            counter++;
        }
        string line = "";
        string ring = "";
        counter = 0;
        while ((line = file.ReadLine()) != null)
        {
            line = file.ReadLine();
            arrayLine = line.Split(',');
            user = arrayLine[0];
            technique = arrayLine[1];
            ring = arrayLine[2];
            string ringAux = ring;
            float time = float.Parse(arrayLine[indexTime]);
            float speed = float.Parse(arrayLine[indexSpeed]);

            if(ring !="ring0" && ring != "ring33")
            {
                if(dictionaryUserSpeed.ContainsKey(user + ","+technique))
                {
                    SpeedDataPerUser aux = dictionaryUserSpeed[user + "," + technique];
                    aux.AddPoint(speed, time,ring);
                }
                else
                {
                    dictionaryUserSpeed.Add(user + "," + technique, new SpeedDataPerUser(speed, time,ring));
                }
            }
            
            

            counter++;

        }

        Debug.Log("Size of the dictionary root = " + dictionaryFor2ndFile.Count);
        Dictionary<string, int> higherNumberOfPointsPerTechnique = new Dictionary<string, int>();

        string[] arrayAux;
        string strTotal = "User,Technique,Ring,Speed,Time,TimeNormalized\n";
        string strWip = "User,Technique,Ring,Speed,Time,TimeNormalized\n";
        string strVCircle = "User,Technique,Ring,Speed,Time,TimeNormalized\n";
        string strAnalog = "User,Technique,Ring,Speed,Time,TimeNormalized\n";
        foreach (KeyValuePair<string, SpeedDataPerUser> kPair in dictionaryUserSpeed)
        {
            /*
            if(kPair.Key.Contains("Walking"))
            {
                foreach (string velocity in kPair.Value)
                {
                    arrayAux = velocity.Split(',');
                    strWip += kPair.Key + "," + arrayAux[0] + "," + arrayAux[1] + "\n";
                }
                
            }
            if (kPair.Key.Contains("Circle"))
            {
                foreach (string velocity in kPair.Value)
                {
                    arrayAux = velocity.Split(',');
                    strVCircle += kPair.Key + "," + arrayAux[0] + "," + arrayAux[1] + "\n";
                }
            }
            if (kPair.Key.Contains("Analog"))
            {
                foreach (string velocity in kPair.Value)
                {
                    arrayAux = velocity.Split(',');
                    strAnalog += kPair.Key + "," + arrayAux[0] + "," + arrayAux[1] + "\n";
                }
            }*/
            int pointsCount = kPair.Value.getPointsCount();
            string first = kPair.Value.getSpeedAndTimeNormalized(0);
            string last = kPair.Value.getSpeedAndTimeNormalized(pointsCount-1);

            for (int i = 0; i < pointsCount;i++)
            {
                if(kPair.Key.Contains("Analog"))
                    strAnalog += kPair.Key + "," + kPair.Value.getSpeedAndTimeNormalized(i);
                if (kPair.Key.Contains("Walking"))
                    strWip += kPair.Key + "," + kPair.Value.getSpeedAndTimeNormalized(i);
                if (kPair.Key.Contains("Circle"))
                    strVCircle += kPair.Key + "," + kPair.Value.getSpeedAndTimeNormalized(i);

                //strTotal += kPair.Key + "," + kPair.Value.getSpeedAndTimeNormalized(i);
            }
            /*foreach(SpeedDataPerUser velocity in kPair.Value)
            {
                arrayAux = velocity.Split(',');
                strAnalog += kPair.Key + "," + arrayAux[0] + "," + arrayAux[1] + "\n";
            }*/

            
        }
        if(File.Exists(rootPath + "\\speedVCircle.csv"))
            File.Delete(rootPath + "\\speedVCircle.csv");
       
        if (File.Exists(rootPath + "\\speedWip.csv"))
            File.Delete(rootPath + "\\speedWip.csv");

        if (File.Exists(rootPath + "\\speedAnalog.csv"))
            File.Delete(rootPath + "\\speedAnalog.csv");
        

        File.WriteAllText(rootPath + "\\speedVCircle.csv",strVCircle);
        File.WriteAllText(rootPath + "\\speedWip.csv", strWip);
        File.WriteAllText(rootPath + "\\speedAnalog.csv", strAnalog);



        /*foreach (KeyValuePair<string, List<string>> kPair in dictionaryFor2ndFile)
        {
            arrayAux = kPair.Key.Split(',');
            int currentNumberOfPoints = kPair.Value.Count;    
            if (higherNumberOfPointsPerTechnique.Contains(arrayAux[1]))
            {
                if (higherNumberOfPointsPerTechnique[arrayAux[1]] < kPair.Value.Count )
                {
                    higherNumberOfPointsPerTechnique[arrayAux[1]] = kPair.Value.Count;
                }
            }
            else
            {
                higherNumberOfPointsPerTechnique.Add(arrayAux[1], currentNumberOfPoints);
            }

            Debug.Log("Size of dictionary " + kPair.Key + " = " + kPair.Value.Count );
        }*/
        Debug.Log("Size ");
        

    }


    

    
    // Use this for initialization
    void Start () {
        doStuff();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
