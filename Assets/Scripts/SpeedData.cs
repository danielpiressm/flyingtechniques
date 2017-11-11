using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedData  {

    private float speed;
    private float time;

    public SpeedData(float speed, float time)
    {
        this.speed = speed;
        this.time = time;
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public float Time
    {
        get
        {
            return time;
        }

        set
        {
            time = value;
        }
    }
}
