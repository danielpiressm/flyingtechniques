using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class FileReader : MonoBehaviour {

    public Dictionary<string, string> dictionary;
    public Dictionary<string, string> dictionaryHeaders;


    void doStuff()
    {
        dictionary = new Dictionary<string, string>();
        Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
        dictionaryHeaders = new Dictionary<string, string>();


        //dictionary.Add("collisionLog.csv", "");
        //dictionary.Add("report.csv", "");
        //dictionary.Add("headLog.csv", "");
        //dictionary.Add("optimalPath.csv", "");
        //dictionary.Add("leftFootLog.csv", "");
        //dictionary.Add("leftHandLog.csv", "");
        ///dictionary.Add("reportPath.csv", "");
        //dictionary.Add("optimalPath.csv", "");
        dictionary.Add("rightFootLog.csv", "");
        dictionary.Add("rightHandLog.csv", "");
        

        string rootPath = "F:\\Dropbox\\doutorado\\papers\\vrstFlying\\somerandomtest";

        string[] filePaths = Directory.GetFiles(@"F:\Dropbox\doutorado\papers\vrstFlying\userLogs", "*.csv", SearchOption.AllDirectories);
        string[] fileArray = Directory.GetDirectories(@"F:\Dropbox\doutorado\papers\vrstFlying\userLogs");

        foreach (string fileName in filePaths)
        {
            FileInfo fInfo = new FileInfo(fileName);
            
            string userName = fInfo.Directory.Parent.Name;

            try
            {
                if (dictionary.ContainsKey(fInfo.Name))
                {
                    //dictionary[fInfo.Name]++;// = 
                    dictionary[fInfo.Name] += readFile(userName, fileName);
                }


                else
                {
                   // Debug.Log("Devia ter feito isto antes");

                }
            }
            catch (Exception ex)
            {
                Debug.Log("EX = " + fInfo.Name);
            }
            int x = 2;
        }

        Debug.Log("ACABEI");

        foreach (KeyValuePair<string, string> word in dictionary)
        {
            string tmp = "";
            /*if (word.Key != "")
                tmp = word.Value.Substring(0, 30);*/
            Debug.Log(" Word =  " + tmp);
            System.IO.File.WriteAllText(rootPath + "\\" + word.Key, "User,Technique,"+dictionaryHeaders[word.Key]+word.Value);
        }

        
    }


    // Use this for initialization
    void Start () {

        string str = "Task,Teste\n0.0,10.0";
        str = str.Replace("\n", "\nOI");
        Debug.Log(str);
        doStuff();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    string readFile(string name, string filePath)
    {
        int counter = 0;
        string header = "";
        string line = "";
        string str = "";

        System.IO.StreamReader file = new System.IO.StreamReader(filePath);
        FileInfo fInfo = new FileInfo(filePath);
        string dName = fInfo.Directory.Name.Split('_')[1];

        header = file.ReadLine();
        if(!dictionaryHeaders.ContainsKey(fInfo.Name))
        {
            dictionaryHeaders.Add(fInfo.Name, header + "\n");
        }
        //string text = file.ReadToEnd();
        //text += (text.Replace("\n", "\n" + name+","));

        while( (line = file.ReadLine()) != null)
        {
       //     Debug.Log(line);
            str += name + "," + dName + "," + line + "\n";
            counter++;
        }

        file.Close();

        return str;
    }

}
