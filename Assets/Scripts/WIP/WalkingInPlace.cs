using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace KinectClient
{

    public enum GaitState
    {
        STATIONARY = 0,
        MOVING = 1
    }

    public enum WiPMode
    {
        NORMAL,
        SWING_UP_VELOCITY_ONLY
    }

    public enum AngularVelocityMode
    {
        LINEAR_CONSTANT,
        LINEAR_VARIABLE
    }

    public enum ComputeAngularVelocityMode
    {
        LINEAR,
        EXPONENTIAL,
        TIME
    }

    public class WalkingInPlace
    {


        public delegate void WalkingInPlaceEventHandler(object sender, WalkingInPlaceEventArgs e);
        public event WalkingInPlaceEventHandler WalkingInPlaceEvent;

        public GaitState _gait;
        public WiPMode wipMode;
        public AngularVelocityMode angularVelocityMode;
        public ComputeAngularVelocityMode computeAngularVelocityMode;

        private WipFoot _rightFoot;
        private WipFoot _leftFoot;



        public float _angularPosition;
        private float _minAngularPositionThreshold;
        private float _maxAngularPositionThreshold;

        private float _positionThresholdAdd;

        public int _numSteps;

        public float _distance;
        private float _footVel;

        private float _userHeight;

        private bool _gaitBackwards;

        private Timeout _timeout;

        private float _maxHeightSpeedThreshold;

        private float _stepInitSpeedThreshold;
        public float StepInitSpeedThreshold
        {
            get { return _stepInitSpeedThreshold; }
            set { _stepInitSpeedThreshold = value; }
        }

        private float _stepMaxHeightPos;
        public float _maxHeightPhasePos;
        private float _lastPhasePos;
        private float _firstPhasePos;


        private long _initPhaseTime;
        private float _initPhaseFootPosition;
        private float _initPhaseFootVelocity;

        private long _endPhaseTime;
        private float _endPhaseFootPosition;
        private float _endPhaseFootVelocity;

        public float _totalStepPhaseTime;
        private float _lastStepPhaseTime;
        private float _lastStepPhaseVelocity;
        private float _phaseTimeEstimated;
        private long _floorTimeInit;
        private long _floorTimeEnd;

        private long _currentTime;

        private string consoleString = "";

        private float _positionThreshold;
        public float PositionThreshold
        {
            get { return _positionThreshold; }
            set { _positionThreshold = value; }
        }

        private float _virtualVelocity;
        public float VirtualVelocity
        {
            get { return _virtualVelocity; }
        }
        private List<float> _virtualVelocityList;
        private float _dispatchVelocity;

        private float _initialVelocity;
        public float InitialVelocity
        {
            get { return _initialVelocity; }
            set { _initialVelocity = value; }
        }

        private float _maxVelocity;
        public float MaxVelocity
        {
            get { return _maxVelocity; }
            set { _maxVelocity = value; }
        }

        private int _veFrameRate;
        public int VeFrameRate
        {
            get { return _veFrameRate; }
            set { _veFrameRate = value; }
        }

        private float _angularOffset;
        public float AngularOffset
        {
            get { return _angularOffset; }
        }

        private float _minRotationThreshold;
        public float MinRotationThreshold
        {
            get { return _minRotationThreshold; }
            set { _minRotationThreshold = value; }
        }

        private float _maxRotationThreshold;
        public float MaxRotationThreshold
        {
            get { return _maxRotationThreshold; }
            set { _maxRotationThreshold = value; }
        }

        private float _constantAngularVelocityValue;
        public float ConstantAngularVelocityValue
        {
            get { return _constantAngularVelocityValue; }
            set { _constantAngularVelocityValue = value; }
        }

        private bool _outputEnabled;
        public bool OutputEnabled
        {
            get { return _outputEnabled; }
            set
            {
                _outputEnabled = value;
                _rightFoot.OutputEnabled = _outputEnabled;
                _leftFoot.OutputEnabled = _outputEnabled;
            }
        }

        private float _m;
        public float M
        {
            get { return _m; }
            set { _m = value; }
        }

        private float _b;
        public float B
        {
            get { return _b; }
            set { _b = value; }
        }


        private Stopwatch _rotationTime;

        private FileWriter velFileWriter;



        public WalkingInPlace(string logfilename)
        {
            velFileWriter = new FileWriter("out.csv");
            velFileWriter.write("user height $ distance $ foot vel $ speed");

            _rotationTime = new Stopwatch();
            _rotationTime.Start();


            _m = 2.83f;
            _b = 10f;


            _veFrameRate = 60;
            _angularOffset = 0f;

            _minRotationThreshold = 0.085f;
            _maxRotationThreshold = 0.90f;

            _userHeight = 0f;

            _initialVelocity = 0.24f;
            _maxVelocity = 3.0f;

            _gait = GaitState.STATIONARY;
            wipMode = WiPMode.NORMAL;

            computeAngularVelocityMode = ComputeAngularVelocityMode.LINEAR;
            //wipMode = WiPMode.SWING_UP_VELOCITY_ONLY;

            angularVelocityMode = AngularVelocityMode.LINEAR_VARIABLE;

            _positionThreshold = 0.035f;
            _rightFoot = new WipFoot(Type.RIGHT);
            _leftFoot = new WipFoot(Type.LEFT);

            _virtualVelocity = 0.0f;
            _virtualVelocityList = new List<float>();
            _dispatchVelocity = 0f;

            _angularPosition = 0f;

            _gaitBackwards = false;

            _resetSteps();

            _timeout = new Timeout(1000);

            _distance = 0.0f;
            _footVel = 0.0f;

            _positionThresholdAdd = 90f;

            _maxHeightSpeedThreshold = 0.10f; //TODO
            _stepInitSpeedThreshold = 0.0f;
            _stepMaxHeightPos = 0.0f;
            _maxHeightPhasePos = 0.0f;
            _lastPhasePos = 0.0f;
            _firstPhasePos = 0.0f;

            _initPhaseTime = 0; ;
            _initPhaseFootPosition = 0.0f;
            _initPhaseFootVelocity = 0.0f;

            _endPhaseTime = 0;
            _endPhaseFootPosition = 0.0f;
            _endPhaseFootVelocity = 0.0f;

            _totalStepPhaseTime = 0.0f;
            _lastStepPhaseTime = 0.0f;
            _lastStepPhaseVelocity = 0.0f;

            _floorTimeInit = 0;
            _floorTimeEnd = 0;

            ConstantAngularVelocityValue = 30.0f;

            _resetSteps();

            OutputEnabled = false;

        }

        private void _resetSteps()
        {
            _numSteps = 0;
            _leftFoot.numSteps = 0;
            _rightFoot.numSteps = 0;
        }

        public long getElapsedTimeMilliseconds()
        {
            return _timeout.getElapsedTimeMilliseconds();
        }

        public long getElapsedTimeSeconds()
        {
            return getElapsedTimeMilliseconds() / 1000;
        }

        private void _changeGaitState()
        {
            if (_rightFoot.state == FootStates.ON_THE_FLOOR
                && _leftFoot.state == FootStates.ON_THE_FLOOR)
            {
                _gait = GaitState.STATIONARY;
                _resetSteps();

                _virtualVelocity = 0.0f;

                _leftFoot.supportedBetweenSteps = false;
                _rightFoot.supportedBetweenSteps = false;

                printDebug("PAROU");

                _virtualVelocityList = new List<float>();

            }
            else if ((_rightFoot.state == FootStates.ON_THE_FLOOR
                 || _leftFoot.state == FootStates.ON_THE_FLOOR)
                && _gait == GaitState.STATIONARY)
            {
                _gait = GaitState.MOVING;
            }

        }

        private void _computeGaiterVelocity_Amplitude()
        {
            if (_totalStepPhaseTime == 0)
            {
                _virtualVelocity = 0.0f;
                return;
            }

            _virtualVelocity = 7 * _maxHeightPhasePos; //TODO: is it PosY?
        }

        private void _computeGaiterVelocity_Stature()
        {
            // Tx = amplitude/estatura
            //_virtualVelocity = 0.12*Tx;
        }

        private void _computeGaiterVelocity()
        {
            //speed =(alturaPe/0,36) * (1,95/estaturaUser) + velPe/1,45

            if (_totalStepPhaseTime == 0)
            {
                //_virtualVelocity = 0.0f;
                return;
            }

            float distance = Math.Abs(_endPhaseFootPosition - _initPhaseFootPosition);
            float footVel = distance / ((float)_totalStepPhaseTime);

            _distance = distance;
            _footVel = footVel;

            if (distance > 0.13f)
            {
                // user height $ distance $ foot vel $ speed
                _virtualVelocity = (distance / 0.36f) * (1.95f / _userHeight) + (footVel / 1.45f);
                velFileWriter.write("" + _userHeight + "$" + distance + "$" + footVel + "$" + _virtualVelocity);

                Console.WriteLine("--< " + _userHeight);

                //_virtualVelocity = footVel * distance / 0.13f;
            }
            else
            {
                _virtualVelocity = footVel;
            }


            if (VirtualVelocity > MaxVelocity)
            {
                _virtualVelocity = MaxVelocity;
            }
        }

        private void _initVerticalStepPhase(WipFoot foot)
        {
            _initPhaseTime = _currentTime;
            _initPhaseFootPosition = foot.Y; // 1000f;
            _initPhaseFootVelocity = foot.dv;
        }

        private void _endVerticalStepPhase(WipFoot foot)
        {
            _endPhaseTime = _currentTime;
            _endPhaseFootPosition = foot.Y; // 1000f;

            _totalStepPhaseTime = ((float)_endPhaseTime - (float)_initPhaseTime) / 1000.0f;

            if (_maxHeightPhasePos < _endPhaseFootPosition)
            {
                _maxHeightPhasePos = _endPhaseFootPosition;
            }

            _endPhaseFootVelocity = (_endPhaseFootPosition - _initPhaseFootPosition) / _totalStepPhaseTime;

        }

        private bool _gaitStopCondition()
        {
            long phaseTimeMs = _getPhaseTime();
            float phaseTime = phaseTimeMs / 1000f;



            if (_rightFoot.state == FootStates.ON_THE_FLOOR
                    && _rightFoot.supportedBetweenSteps
                    && phaseTime > _phaseTimeEstimated
                    && _numSteps != 0)
            {
                printDebug("phaseTime: " + phaseTime + "\t phaseTimeEstimated:" + _phaseTimeEstimated);

                return true;
            }
            else if (_leftFoot.state == FootStates.ON_THE_FLOOR
                     && _leftFoot.supportedBetweenSteps
                     && phaseTime > _phaseTimeEstimated
                     && _numSteps != 0)
            {
                printDebug("phaseTime: " + phaseTime + "\t phaseTimeEstimated: " + _phaseTimeEstimated);

                return true;
            }
            else if ((_leftFoot.state == FootStates.RAISED_UP
                      || _rightFoot.state == FootStates.RAISED_DOWN)
                        && phaseTime > 1f)
            {
                _leftFoot.state = FootStates.ON_THE_FLOOR;
                _rightFoot.state = FootStates.ON_THE_FLOOR;
                return true;
            }
            else if ((_leftFoot.state == FootStates.RAISED_DOWN
                      || _rightFoot.state == FootStates.RAISED_UP)
                        && phaseTime > 1f)
            {
                _leftFoot.state = FootStates.ON_THE_FLOOR;
                _rightFoot.state = FootStates.ON_THE_FLOOR;
                return true;
            }


            return false;
        }

        private void _estimatePhaseTime()
        {
            _phaseTimeEstimated = (-0.29f * (Math.Abs(_endPhaseFootVelocity)) + 0.84f);
            //_phaseTimeEstimated = 1.5f;
        }

        private long _getPhaseTime()
        {
            return _currentTime - _initPhaseTime;
        }


        private bool doDispatch = false;
        private void _detectGaiterWip()
        {

            if (_gait == GaitState.STATIONARY)
            {
                if (consoleString != "stationary")
                {
                    consoleString = "stationary";
                    Console.WriteLine("\n");
                }
                //OnWalkingInPlaceEvent(new WalkingInPlaceEventArgs(0f, 0f));
                //return;
                _virtualVelocity = 0f;
                _angularOffset = 0f;
            }

            // left foot: strike left foot (stance phase)
            if (_leftFoot.state == FootStates.RAISED_DOWN
                && _rightFoot.state == FootStates.ON_THE_FLOOR
                && _leftFoot.Y <= _positionThreshold)
            {
                _endVerticalStepPhase(_leftFoot);
                _estimatePhaseTime();

                _computeGaiterVelocity();
                _numSteps += 1;
                _leftFoot.numSteps += 1;
                _leftFoot.supportedBetweenSteps = true;
                _leftFoot.update(FootTransitionEvents.RESTING_DOWN);
                _timeout.Start();

                _initVerticalStepPhase(_leftFoot);
            }

            // left foot: get max height - swing down
            else if (_leftFoot.state == FootStates.RAISED_UP
                && _rightFoot.state == FootStates.ON_THE_FLOOR
                && _leftFoot.dv < _maxHeightSpeedThreshold)
            {
                _endVerticalStepPhase(_leftFoot);
                _stepMaxHeightPos = _maxHeightPhasePos;

                _computeGaiterVelocity();
                _lastStepPhaseVelocity = _endPhaseFootVelocity;

                if (wipMode != WiPMode.SWING_UP_VELOCITY_ONLY)
                {
                    //XXX _raiseWalkingInPlaceEvent(VirtualVelocity);
                    doDispatch = true;

                }

                _leftFoot.update(FootTransitionEvents.MAX_REACHED);
                _initVerticalStepPhase(_leftFoot);
                _timeout.Start();
            }


            // left foot: start left foot - swing up
            else if (_leftFoot.state == FootStates.ON_THE_FLOOR
                && _rightFoot.state == FootStates.ON_THE_FLOOR
                && (_leftFoot.Y >= _positionThreshold
                    && _leftFoot.dv >= StepInitSpeedThreshold))
            {
                //printDebug("LEFT  " + _numSteps);
                //printDebug("h: " + _leftFoot.Y);
                //printDebug("v: " + _leftFoot.dv + "\n");

                if (_numSteps != 0)
                {
                    _endVerticalStepPhase(_leftFoot);
                    //XXX _raiseWalkingInPlaceEvent(VirtualVelocity);
                    doDispatch = true;
                }
                else
                {
                    //XXX _raiseWalkingInPlaceEvent(_initialVelocity);
                    _virtualVelocity = _initialVelocity;
                    doDispatch = true;
                }

                _leftFoot.update(FootTransitionEvents.ELEVATING);
                _leftFoot.supportedBetweenSteps = false;
                if (_gait == GaitState.STATIONARY)
                {
                    _changeGaitState();
                }

                _initVerticalStepPhase(_leftFoot);
                _timeout.Start();
            }




            // right foot: strike right foot (stance phase)
            else if (_rightFoot.state == FootStates.RAISED_DOWN
                && _leftFoot.state == FootStates.ON_THE_FLOOR
                && _rightFoot.Y <= _positionThreshold)
            {
                _endVerticalStepPhase(_rightFoot);
                _estimatePhaseTime();

                _computeGaiterVelocity();
                _numSteps += 1;
                _rightFoot.numSteps += 1;
                _rightFoot.supportedBetweenSteps = true;
                _rightFoot.update(FootTransitionEvents.RESTING_DOWN);
                _timeout.Start();

                _initVerticalStepPhase(_rightFoot);

            }



            // right foot: get max height - swing down
            else if (_rightFoot.state == FootStates.RAISED_UP
                && _leftFoot.state == FootStates.ON_THE_FLOOR
                && _rightFoot.dv < _maxHeightSpeedThreshold)
            {
                _endVerticalStepPhase(_rightFoot);
                _stepMaxHeightPos = _maxHeightPhasePos;

                _computeGaiterVelocity();
                _lastStepPhaseVelocity = _endPhaseFootVelocity;

                if (wipMode != WiPMode.SWING_UP_VELOCITY_ONLY)
                {
                    //XXX _raiseWalkingInPlaceEvent(VirtualVelocity);
                    doDispatch = true;
                }

                _rightFoot.update(FootTransitionEvents.MAX_REACHED);
                _initVerticalStepPhase(_rightFoot);
                _timeout.Start();
            }



            // right foot: start right foot - swing up
            else if (_rightFoot.state == FootStates.ON_THE_FLOOR
                && _leftFoot.state == FootStates.ON_THE_FLOOR
                && (_rightFoot.Y >= _positionThreshold
                    && _rightFoot.dv >= StepInitSpeedThreshold))
            {
                //printDebug("RIGHT   " + _numSteps);
                //printDebug("h: " + _rightFoot.Y);
                //printDebug("v: " + _rightFoot.dv + "\n");



                if (_numSteps != 0)
                {
                    _endVerticalStepPhase(_rightFoot);
                    //XXX _raiseWalkingInPlaceEvent(VirtualVelocity);
                    doDispatch = true;
                }
                else
                {
                    //XXX _raiseWalkingInPlaceEvent(_initialVelocity);
                    _virtualVelocity = _initialVelocity;
                    doDispatch = true;
                }

                _rightFoot.update(FootTransitionEvents.ELEVATING);
                _rightFoot.supportedBetweenSteps = false;
                if (_gait == GaitState.STATIONARY)
                {
                    _changeGaitState();
                }


                _initVerticalStepPhase(_rightFoot);
                _timeout.Start();
            }

            else if (_leftFoot.state == FootStates.ON_THE_FLOOR
                && _leftFoot.Y < _positionThreshold
                && _rightFoot.state == FootStates.ON_THE_FLOOR
                && _rightFoot.Y < _positionThreshold)
            {
                if (_timeout.isTimeUp())
                {
                    //_changeGaitState();
                }
            }

            if (_gaitStopCondition())
            {
                _changeGaitState();
                return;
            }

            dispatchWipEvent();
        }

        private void dispatchWipEvent()
        {
            float dispatchVelocity = VirtualVelocity;
            if (_gait == GaitState.STATIONARY)
            {
                _angularOffset = 0f;
                dispatchVelocity = 0f;
                _raiseWalkingInPlaceEvent(dispatchVelocity);
            }
            else
            {
                if (dispatchVelocity < _initialVelocity && dispatchVelocity > 0f)
                {
                    dispatchVelocity = _initialVelocity;
                }

                if (doDispatch)
                {
                    //    if (dispatchVelocity < _initialVelocity && dispatchVelocity > 0f) dispatchVelocity = _initialVelocity;



                    if (_virtualVelocityList.Count == 0)
                    {
                        dispatchVelocity = VirtualVelocity;
                        _virtualVelocityList.Add(dispatchVelocity);
                    }
                    else
                    {
                        dispatchVelocity = VirtualVelocity;



                        foreach (float f in _virtualVelocityList)
                        {
                            dispatchVelocity += f;
                        }
                        dispatchVelocity = dispatchVelocity / (_virtualVelocityList.Count + 1);

                        if (_virtualVelocityList.Count == 2)
                        {
                            _virtualVelocityList.RemoveAt(0);
                        }
                        _virtualVelocityList.Add(dispatchVelocity);
                    }
                    doDispatch = false;
                    _dispatchVelocity = dispatchVelocity;
                }
                _raiseWalkingInPlaceEvent(_dispatchVelocity);
            }




        }


        private float _transformAngularPosition_Linear(float angle)
        {
            //return 1.83f * (angle - MinRotationThreshold) + 5.01f;

            M = 57f;
            B = -5f;

            //M = 58.04f;
            //B = -5.31f;

            float angleAbs = Math.Abs(angle);
            float ret = 0f;


            ret = M * (angleAbs) + B;


            if (angle < 0) ret = -ret;


            return ret;
        }

        private float _transformAngularPosition_Exponential(float angle)
        {
            double tmp_angle = angle;
            if (angle < 0) tmp_angle = Math.Abs(angle);
            if (tmp_angle > 0.25) tmp_angle = 0.25;


            double v = Math.Exp(Math.Log(0.05) + (Math.Log(30) - Math.Log(0.05)) * tmp_angle / 0.25);

            if (angle < 0) v = -v;


            return (float)v;
        }

        private float _transformAngularPosition_Time(float angle)
        {
            float elapsed = _rotationTime.ElapsedMilliseconds;
            if (elapsed > 2000f) elapsed = 2000f;



            float M = 0.01f;
            float B = 0.05f;

            float r = M * elapsed + B;

            if (angle < 0) r = -r;
            return r;
        }

        private float _transformAngularPosition_linear_based_on_forward_velocity(float angle)
        {
            float m = 11.98f;
            float b = 0.04f;

            //float tmp_angle = 23.99f * VirtualVelocity + 0.03f;
            float tmp_angle = m * VirtualVelocity + b;
            if (angle < 0) tmp_angle = -tmp_angle;


            return tmp_angle;
        }

        private void _computeAngularOffset()
        {
            if (Math.Abs(_angularPosition) <= MinRotationThreshold)
            {
                _angularOffset = _transformAngularPosition_Exponential(_angularPosition);
                _angularOffset = _angularOffset / _veFrameRate;
            }
            else
            {
                if (computeAngularVelocityMode == ComputeAngularVelocityMode.EXPONENTIAL)
                {
                    //Debug.WriteLine("EXPONENTIAL");
                    _angularOffset = _transformAngularPosition_Exponential(_angularPosition);
                    _angularOffset = _angularOffset / _veFrameRate;
                }
                else if (computeAngularVelocityMode == ComputeAngularVelocityMode.TIME)
                {
                    //Debug.WriteLine("TIME");
                    _angularOffset = _transformAngularPosition_Time(_angularPosition);
                    _angularOffset = _angularOffset / _veFrameRate;
                }
                else
                {
                    //Debug.WriteLine("LINEAR");
                    _angularOffset = _transformAngularPosition_Linear(_angularPosition);
                    _angularOffset = _angularOffset / _veFrameRate;
                }
            }
        }


        private void _setAngularPosition(float angularPosition)
        {
            //if (computeAngularVelocityMode == ComputeAngularVelocityMode.EXPONENTIAL)
            //{
            //    _angularPosition = angularPosition;
            //}
            //else
            //{

            if (computeAngularVelocityMode == ComputeAngularVelocityMode.TIME)
            {
                if (angularPosition < MinRotationThreshold && angularPosition > -MinRotationThreshold)
                {
                    _rotationTime.Reset();
                    Debug.WriteLine("RESTART");
                }
                else
                {
                    Debug.WriteLine("_");
                }
            }
            _angularPosition = angularPosition;
        }

        private void _raiseWalkingInPlaceEvent(float velocity)
        {
            float offset = _angularOffset;

            if (OutputEnabled)
            {
                OnWalkingInPlaceEvent(new WalkingInPlaceEventArgs(velocity, offset));
            }
        }

        public void setUserHeight(float userHeight)
        {
            this._userHeight = userHeight;
        }

        public void update(float rightFoot, float leftFoot)
        {

            try
            {
                _currentTime = getElapsedTimeMilliseconds();

                _rightFoot.setFootPosition(rightFoot, _currentTime, _positionThreshold);
                _leftFoot.setFootPosition(leftFoot, _currentTime, _positionThreshold);



                //_setAngularPosition(newAngularPosition);
                _computeAngularOffset();
                _detectGaiterWip();

                if (_outputEnabled)
                {
                    //velFileWriter.write("" + leftFoot + "$" + rightFoot + "$" + _numSteps + "$" + _virtualVelocity + "$" + (_angularOffset * _veFrameRate));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
            }
        }

        public float updateDaniel(float rightFoot, float leftFoot)
        {

            update(rightFoot, leftFoot);

            return _dispatchVelocity;
        }


        protected virtual void OnWalkingInPlaceEvent(WalkingInPlaceEventArgs e)
        {
            if (WalkingInPlaceEvent != null)
            {
                WalkingInPlaceEvent(this, e);
            }
        }

        private void printDebug(String line)
        {
            if (this.OutputEnabled)
            {
                Debug.WriteLine("[WalkingInPlace.cs] " + line);
            }
        }
    }
}

