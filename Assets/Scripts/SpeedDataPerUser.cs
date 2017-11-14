using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDataPerUser  {

    List<SpeedData> list;
    List<float> listSpeed;
    private string user;
    private string technique;
    private float timeOfthetask;
    private float timeReference;
    private float accumSpeed;
    private string ring;

	public SpeedDataPerUser(string user, string technique,float speed,float time)
    {
        this.user = user;
        this.technique = technique;
        list = new List<SpeedData>();
        listSpeed = new List<float>();
        SpeedData data = new SpeedData(speed, time,"");
        list.Add(data);
        accumSpeed += speed;

        timeOfthetask = 0;
    }

    public SpeedDataPerUser(float speed, float time,string ring)
    {
        list = new List<SpeedData>();
        timeReference = time;
        SpeedData data = new SpeedData(speed, 0.0f,ring);
        listSpeed = new List<float>();
        list.Add(data);
        timeOfthetask = 0;
        accumSpeed += speed;
        this.ring = ring;
    }

    public void AddPoint(float speed, float time,string ring)
    {
        this.ring = ring;
        float timeAux = time - timeReference;
        list.Add(new SpeedData(speed, timeAux,ring));
        listSpeed.Add(speed);
        accumSpeed += speed;
        timeOfthetask = list[list.Count-1].Time - list[0].Time;
    }

    public int getPointsCount()
    {
        return list.Count;
    }

    public string getSpeedAndTimeNormalized(int index)
    {
        if (index > list.Count)
            return null;
        else
        {
            float timeNormalized = list[index].Time / timeOfthetask;
            return list[index].Ring + ","+ list[index].Speed.ToString() + "," + list[index].Time.ToString() + "," + timeNormalized.ToString() + "\n";
        }
            
    }

    public float GetSpeedMedian()
    {
        float median = 0.0f;
        int size = listSpeed.Count;
        int mid = size / 2;
        List<float> listCopy = new List<float>(listSpeed);
        listCopy.Sort();

        median = (size % 2 != 0) ? (float)listCopy[mid] : ((float)listCopy[mid] + (float)listCopy[mid - 1]) / 2;
        return median;
    }

    public float GetMedian(List<float> list)
    {
        float median = 0;
        int size = list.Count;
        int mid = size / 2;
        List<float> listCopy = new List<float>(list);
        listCopy.Sort();

        median = (size % 2 != 0) ? (float)listCopy[mid] : ((float)listCopy[mid] + (float)listCopy[mid - 1]) / 2;
        return median;
    }

    public float GetSpeedAverage()
    {
        float average = 0.0f;
        average = accumSpeed / getPointsCount();
        return average;
    }

    public float GetIqr()
    {
        List<float> afVal = new List<float>(listSpeed);
        float[] iqr = new float[3];

        int iSize = afVal.Count;
        int iMid = iSize / 2; //this is the mid from a zero based index, eg mid of 7 = 3;

        float fQ1 = 0;
        float fQ2 = 0;
        float fQ3 = 0;

        if (iSize % 2 == 0)
        {
            //================ EVEN NUMBER OF POINTS: =====================
            //even between low and high point
            fQ2 = (afVal[iMid - 1] + afVal[iMid]) / 2;

            int iMidMid = iMid / 2;

            //easy split 
            if (iMid % 2 == 0)
            {
                fQ1 = (afVal[iMidMid - 1] + afVal[iMidMid]) / 2;
                fQ3 = (afVal[iMid + iMidMid - 1] + afVal[iMid + iMidMid]) / 2;
            }
            else
            {
                fQ1 = afVal[iMidMid];
                fQ3 = afVal[iMidMid + iMid];
            }
        }
        else if (iSize == 1)
        {
            //================= special case, sorry ================
            fQ1 = afVal[0];
            fQ2 = afVal[0];
            fQ3 = afVal[0];
        }
        else
        {
            //odd number so the median is just the midpoint in the array.
            fQ2 = afVal[iMid];

            if ((iSize - 1) % 4 == 0)
            {
                //======================(4n-1) POINTS =========================
                int n = (iSize - 1) / 4;
                fQ1 = (afVal[n - 1] * .25f) + (afVal[n] * .75f);
                fQ3 = (afVal[3 * n] * .75f) + (afVal[3 * n + 1] * .25f);
            }
            else if ((iSize - 3) % 4 == 0)
            {
                //======================(4n-3) POINTS =========================
                int n = (iSize - 3) / 4;

                fQ1 = (afVal[n] * .75f) + (afVal[n + 1] * .25f);
                fQ3 = (afVal[3 * n + 1] * .25f) + (afVal[3 * n + 2] * .75f);
            }
        }


        return fQ3 - fQ1;

    }

    public float GetVariance( )
    {
        float variance = 0;
        float mean = GetSpeedAverage();
        int start = 0;
        int end = listSpeed.Count;


        for (int i = start; i < end; i++)
        {
            variance += Mathf.Pow((listSpeed[i] - mean), 2);
        }

        int n = end - start;
        if (start > 0) n -= 1;

        return variance / (n);
    }


}
