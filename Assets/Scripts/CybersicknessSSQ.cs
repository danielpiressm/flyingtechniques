using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CybersicknessSSQ : MonoBehaviour {


    int translate(string str)
    {
        int x = 0;
        if (str.Contains("um"))
        {
            x = 0;
        }
        if( str.Contains("Leve"))
        {
            x = 1;
        }
        if( str.Contains("ado"))
        {
            x = 2;
        }
        if(str.Contains("vero"))
        {
            x = 3;
        }

        return x;

    }


    void doStuff()
    {
        string header = "";
        int counter = 0;
        string str = "";
        string strForSecondFile = "User,Technique,";
        string user = "";
        string technique = "";
        string[] arrayLine = null;
        string line = "";

        string rootPath = "F:\\Dropbox\\doutorado\\papers\\vrstFlying\\output\\2ndTest";
        string filePath = "F:\\Dropbox\\doutorado\\papers\\vrstFlying\\output\\2ndTest\\questionariosCybersickness1.csv";

        System.IO.StreamReader file = new System.IO.StreamReader(filePath);


        while ((line = file.ReadLine()) != null)
        {
            arrayLine = line.Split(',');
            user = arrayLine[0];
            technique = arrayLine[1];

            float q1 = translate(arrayLine[2]);
            float q2 = translate(arrayLine[3]);
            float q3 = translate(arrayLine[4]);
            float q4 = translate(arrayLine[5]);
            float q5 = translate(arrayLine[6]);
            float q6 = translate(arrayLine[7]);
            float q7 = translate(arrayLine[8]);
            float q8 = translate(arrayLine[9]);
            float q9 = translate(arrayLine[10]);
            float q10 = translate(arrayLine[11]);
            float q11 = translate(arrayLine[12]);
            float q12 = translate(arrayLine[13]);
            float q13 = translate(arrayLine[14]);
            float q14 = translate(arrayLine[15]);
            float q15 = translate(arrayLine[16]);
            float q16 = translate(arrayLine[17]);

            float nausea = (q1 + q6 + q7 + q8 + q9 + q15 + q16) * 9.54f;
            float oculomotor = (q1 + q2 + q3 + q4 + q5 + q9 + q11) * 7.58f;
            float disorientation = (q5 + q8 + q10 + q11 + q12 + q13 + q14) * 13.92f;

            





            float ssqScore = nausea + oculomotor + disorientation;


            str += user + "," + technique + "," + ssqScore + "\n";


        }

        System.IO.File.WriteAllText(rootPath + "\\ssqOutput.csv" , str);
    }


    // Use this for initialization
    void Start () {
        doStuff();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
