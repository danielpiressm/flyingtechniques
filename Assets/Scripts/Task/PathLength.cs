using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class PathLength : MonoBehaviour {

    Dictionary<string, float> lengthDictionary;

    void doStuff()
    {
        int counter = 0;
        string header = "";
        string line = "";
        string str = "";
        string rootPath = "D:\\Dropbox\\doutorado\\papers\\vrstFlying\\somerandomtest\\";
        string filePath = "D:\\Dropbox\\doutorado\\papers\\vrstFlying\\somerandomtest\\headLog.csv";

        System.IO.StreamReader file = new System.IO.StreamReader(filePath);

        header = file.ReadLine();
        string[] arrayHeader = header.Split(',');
        int indexThatIWant = 0;

        foreach(string field in arrayHeader)
        {
            if(field.Contains("Pos"))
            {
                indexThatIWant = counter;
                break;
            }
            counter++;
        }

        string[] arrayLine = null;
        string user = "";
        string technique = "";
        Vector3 lastVec = new Vector3();
        counter = 0;
        while ((line = file.ReadLine()) != null)
        {
            
            arrayLine =line.Split(',');
            user = arrayLine[0];
            technique = arrayLine[1];
            float x = float.Parse(arrayLine[indexThatIWant]);
            float y = float.Parse(arrayLine[indexThatIWant+1]);
            float z = float.Parse(arrayLine[indexThatIWant+2]);
            
            Vector3 currentPos = new Vector3(x, y, z);
            if (counter == 0)
                lastVec = currentPos;


            Vector3 aux = currentPos - lastVec;

            if(!lengthDictionary.ContainsKey(user+","+technique))
            {
                lengthDictionary.Add(user + "," + technique, aux.magnitude);
            }
            else
            {
                lengthDictionary[user + "," + technique] += aux.magnitude;
            }
            lastVec = currentPos;

            counter++;
            
        }

        string strAux = "User,Technique,Length\n";

        foreach (KeyValuePair<string, float> valuePair in lengthDictionary)
        {
            strAux += valuePair.Key + "," + valuePair.Value.ToString() + "\n";
        }
        System.IO.File.WriteAllText(rootPath + "\\" + "pathLengthAux.csv", strAux);

        file.Close();
        counter++;
    }


	// Use this for initialization
	void Start () {
        lengthDictionary = new Dictionary<string, float>();
        doStuff();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
