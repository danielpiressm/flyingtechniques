using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishedCollision  {

    public string colliderName;
    public string jointName;
    public float  startTime;
    public float finishTime;
    public float elapsedTime;

    public FinishedCollision(string colliderName, float startTime, float finishTime)
    {
        this.colliderName = colliderName;
        this.startTime = startTime;
        this.finishTime = finishTime;
        this.elapsedTime = finishTime - startTime;
    }

    public FinishedCollision(string colliderName, float startTime)
    {
        this.colliderName = colliderName;
        this.startTime = startTime;
    }

}
