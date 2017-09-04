using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogSteering : MonoBehaviour
{

    public Transform head;
    public Transform handTracker;
    public Transform leftHand;
    public Transform rightHand;

    public float analogButtonY = 1.0f;
    public float analogButtonX = 0.0f;
    public float speed = 3.0f;

    Transform hand;
    Vector3 initialrighttHandRotation = new Vector3(0, 8.995001f, 0);
    Vector3 initialLeftHandRotation = new Vector3(0, -8.995001f, 0);


    public float raySize = 3.0f;
    Camera camera;
    public float laserWidth;
    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;
    LineRenderer lRenderer;
    float circleSize = 3;
    public Transform referenceJoint;
    float lastCircleSpeed = 0.0f;

    float getSpeed(Vector2 localPosition)
    {
        float circleRadius = circleSize / 2;
        float xN = localPosition.x / circleRadius;
        float yN = localPosition.y / circleRadius;


        //Debug.Log("xN = (" + xN + "," + yN + ")");
        
        float result = (float)Mathf.Sqrt(xN * xN + yN * yN);
        if (xN < 0.0f || yN < 0.0f)
            result = 0.0f;



        return result;
    }

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        tTask = GetComponent<TestTask>();
        hand = rightHand;
        lRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float circleSpeed = 0.0f;
        analogButtonY = Input.GetAxis("Vertical");
        if(referenceJoint)
        {
            Vector3 transformedPoint = this.transform.InverseTransformPoint(referenceJoint.transform.position);
            circleSpeed = getSpeed(new Vector2(transformedPoint.x, transformedPoint.z));
        }

        if (tTask)
        {
            if (tTask.rightHanded)
            {
                hand = rightHand;
                leftHand.transform.localEulerAngles = initialLeftHandRotation;
            }
            else
            {
                hand = leftHand;
                rightHand.transform.localEulerAngles = initialrighttHandRotation;
            }


            if (tTask.rightHanded == true)
                hand.transform.rotation = Quaternion.LookRotation(handTracker.transform.up, handTracker.transform.forward);
            else if (tTask.rightHanded == false)
                hand.transform.rotation = Quaternion.LookRotation(-handTracker.transform.up, handTracker.transform.forward);

        }


        Vector3 dir = handTracker.transform.forward;
        Debug.DrawRay(hand.transform.position, dir, Color.red);

        if (lRenderer != null)
        {
            lRenderer.enabled = true;
            //hand.transform.eulerAngles = new Vector3(handTracker.transform.eulerAngles.x, handTracker.transform.eulerAngles.z, handTracker.transform.eulerAngles.y);

            //hand.transform.up = -handTracker.transform.forward;

            //hand.transform.localEulerAngles = new Vector3(hand.transform.localEulerAngles.x, hand.transform.localEulerAngles.y, hand.transform.localEulerAngles.z);
			analogButtonY = Input.GetAxis("Vertical");
			if (analogButtonY > 1.0f)
				analogButtonY = 1.0f;
			else if (analogButtonY < 0.002f)
				analogButtonY = 0.0f;
			

            Vector3 endPosition = hand.transform.position + speed * dir *analogButtonY;
            lRenderer.SetPosition(0, hand.transform.position);
            lRenderer.SetPosition(1, endPosition);
            lRenderer.startWidth = laserWidth;
            lRenderer.endWidth = laserWidth;
        }
		float triggerButton = Input.GetAxis ("Z Axis");
		if (triggerButton > 0.002f)
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = dir * speed * Time.deltaTime * analogButtonY;
            this.transform.position += desiredMove;
            tTask.setSpeed(speed * analogButtonY);
            tTask.setNavigationState(true, circleSpeed, lastCircleSpeed);
            
        }
        else
        {
            tTask.setSpeed(0.0f);
            // target.SetActive(false);
        }
        lastCircleSpeed = circleSpeed;
    }
}
