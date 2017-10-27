using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePerRing  {

    private float timeIdle;
    private float timeIdleTotal;
    private float timeWalking;
    private float timeWalkingAndFlying;
    private float timeFlying;
    private float timeFlyingTotal;

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
