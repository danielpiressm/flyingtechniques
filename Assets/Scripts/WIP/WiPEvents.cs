using System;

namespace KinectClient
{
    public class WalkingInPlaceEventArgs : EventArgs
    {
        private float _velocity;
        public float Velocity
        {
            get { return _velocity; }
        }

        private float _angularOffset;
        public float AngularOffset
        {
            get { return _angularOffset; }
        }

        public WalkingInPlaceEventArgs(float velocity, float angularOffset)
        {
            _velocity = velocity;
            _angularOffset = angularOffset;
        }
    }

    public class HeadPositionEventArgs : EventArgs
    {

        public string message;

        public HeadPositionEventArgs(string message)
        {
            this.message = message;
        }
    }
}

