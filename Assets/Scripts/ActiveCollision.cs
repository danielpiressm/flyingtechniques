using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCollision  {

    public string colliderName;
    public string jointName;
    public float timeInit;
    public bool first;

    public int collisionCount = 0;//martelada para coisas


    public ActiveCollision(string jointName)
    {
        this.jointName = jointName;
    }

    public ActiveCollision(string jointName,float timeInit,bool first)
    {
        this.jointName = jointName;
        this.timeInit = timeInit;
        this.first = first;
    }

    public ActiveCollision(string jointName, float timeInit)
    {
        this.jointName = jointName;
        this.timeInit = timeInit;
    }





}
