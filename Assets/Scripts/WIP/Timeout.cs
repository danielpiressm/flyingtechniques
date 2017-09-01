using System;
using System.Diagnostics;

namespace KinectClient
{
    public class Timeout
    {
        private long _startTime;
        public long timeout;
        private Stopwatch _stopwatch;

        public Timeout(long timeoutMiliseconds)
        {
            _startTime = 0;
            timeout = timeoutMiliseconds;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public long getElapsedTimeMilliseconds()
        {
            return _stopwatch.ElapsedMilliseconds;
        }

        public bool isTimeUp()
        {
            return _stopwatch.ElapsedMilliseconds > (_startTime + timeout);
        }

        public long timeRemaining()
        {
            return (_startTime + timeout) - _stopwatch.ElapsedMilliseconds;
        }

        public void Start()
        {
            _startTime = _stopwatch.ElapsedMilliseconds;
        }
    }
}

