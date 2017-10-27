using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePerRing  {

    private float timeIdle = 0;
    private float timeIdleTotal = 0;
    private float timeWalking = 0;
    private float timeWalkingAndFlying = 0;
    private float timeFlying = 0;
    private float timeFlyingTotal = 0;

	public void Add(NavigationState state, float time)
    {
        if(state == NavigationState.Flying)
        {
            timeFlying += time;
            timeFlyingTotal += time;
        }
        else if(state == NavigationState.WalkingAndFlying)
        {
            timeWalkingAndFlying += time;
            timeFlyingTotal += time;
        }
        else if(state == NavigationState.Walking)
        {
            timeWalking += time;
            timeIdleTotal += time;
        }
        else if(state == NavigationState.Idle)
        {
            timeIdle += time;
            timeIdleTotal += time;
        }
    }

    

    public string getFormattedString()
    {
        string header = "ring,timeIdle,timeFlying,timeWalking,timeWalkingAndFlying," +
                        "timeFlyingTotal,TimeIdleTotal\n";
        string str = timeIdle + "," + timeFlying + "," + timeWalking + "," + timeWalkingAndFlying + "," +
                     timeFlyingTotal + "," + timeIdleTotal + "\n";
        return str;

    }


}
