using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedData  {

    private float speed;
    private float time;
    private string ring;

    public SpeedData(float speed, float time,string ring)
    {
        this.speed = speed;
        this.time = time;
        this.ring = ring;
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

    public string Ring
    {
        get
        {
            return ring;
        }

        set
        {
            ring = value;
        }
    }
}
