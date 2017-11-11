using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDataPerUser  {

    List<SpeedData> list;
    private string user;
    private string technique;
    private float timeOfthetask;
    private float timeReference;
    private string ring;

	public SpeedDataPerUser(string user, string technique,float speed,float time)
    {
        this.user = user;
        this.technique = technique;
        list = new List<SpeedData>();
        SpeedData data = new SpeedData(speed, time,"");
        list.Add(data);
        timeOfthetask = 0;
    }

    public SpeedDataPerUser(float speed, float time,string ring)
    {
        list = new List<SpeedData>();
        timeReference = time;
        SpeedData data = new SpeedData(speed, 0.0f,ring);
        list.Add(data);
        timeOfthetask = 0;
        this.ring = ring;
    }

    public void AddPoint(float speed, float time,string ring)
    {
        this.ring = ring;
        float timeAux = time - timeReference;
        list.Add(new SpeedData(speed, timeAux,ring));
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

    
}
