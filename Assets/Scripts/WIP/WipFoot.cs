using System;

namespace KinectClient
{

    public enum Type
    {
        RIGHT,
        LEFT
    }

    public enum FootStates
    {
        ON_THE_FLOOR,
        RAISED_UP,
        RAISED_DOWN
    }

    public enum FootTransitionEvents
    {
        RESTING_DOWN,
        ELEVATING,
        LOWERING,
        MAX_REACHED
    }

    public class WipFoot
    {
        public Type type;
        public float Y;
        public float dY;
        public float dv;
        public float da;
        private float lastStepTime;
        private float _lastVelocity;

        public FootStates state;
        public int numSteps;
        public float velocity;
        public bool supportedBetweenSteps;

        public bool OutputEnabled;


        


        public WipFoot(Type type)
        {
            this.type = type;
            Y = 0.0f;
            dY = 0.0f;
            dv = 0.0f;
            lastStepTime = 0;
            _lastVelocity = 0.0f;
            numSteps = 0;
            
            velocity = 0.0f;
            supportedBetweenSteps = false;
            state = FootStates.ON_THE_FLOOR;

            OutputEnabled = false;
        }

        public void setFootPosition(float newPosition, long time, float positionThreshold)
        {
            dY = newPosition - Y;
            if (newPosition <= positionThreshold)
            {
                dY = 0.0f;
            }

            float dt = (((float)time) - lastStepTime) / 1000;
            dv = dY / dt;
            da = dv / dt;
            _lastVelocity = dv;
            Y = newPosition;
            lastStepTime = time;
        }

        public void update(FootTransitionEvents transitionEvent)
        {
            string debug = " " + this.type.ToString() + ": " + state.ToString() + " --> ";


            if (state == FootStates.ON_THE_FLOOR)
            {
                if (transitionEvent == FootTransitionEvents.ELEVATING)
                {
                    state = FootStates.RAISED_UP;
                }
            }
            else if (state == FootStates.RAISED_UP)
            {
                if (transitionEvent == FootTransitionEvents.MAX_REACHED)
                {
                    state = FootStates.RAISED_DOWN;
                }
            }
            else if (state == FootStates.RAISED_DOWN)
            {
                if (transitionEvent == FootTransitionEvents.RESTING_DOWN)
                {
                    state = FootStates.ON_THE_FLOOR;
                }
            }

            printDebug(debug + state.ToString());
        }

        public override string ToString()
        {
            if (state == FootStates.ON_THE_FLOOR)
            {
                return "___";
            }
            if (state == FootStates.RAISED_UP)
            {
                return "_^_";
            }
            else
            {
                return "_v_";
            }
        }

        private void printDebug(String line)
        {
            if (this.OutputEnabled)
            {
                //System.Diagnostics.Debug.WriteLine("[WipFoot.cs] " + line);
            }
        }
    }
}