using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCircle : MonoBehaviour {


    public Vector3 calibratedPos;

    public float circleSize = 2;

    public bool recalibrate = false;

    public GameObject meshCircle;


    public Transform child;

    Vector2 userPosInsideCircle;

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

    // Use this for initialization
    void Start () {
        //calibratedPos = this.transform.position;
        //child = this.transform.GetChild(0).transform;
        calibratedPos = child.position; 
	}

    Vector2 normalize(Vector2 vector)
    {
        float circleRadius = circleSize / 2;
        float xN = vector.x / circleSize;
        float yN = vector.y / circleSize;

        xN =  (xN - 0.5f) * 2;
        yN =  (yN - 0.5f) * 2;



        return new Vector2();
    }

    bool insideCircle(Vector2 localPosition)
    {
        float circleRadius = circleSize / 2;
        float xN = localPosition.x / circleSize;
        float yN = localPosition.y / circleSize;

        xN = localPosition.x;
        yN = localPosition.y;

        //Debug.Log("xN = (" + xN + "," + yN + ")");

        float result = (float)Mathf.Sqrt(xN * xN + yN* yN);
        float squareRadius = circleRadius * circleRadius;
        if (result < squareRadius)
            return true;
        else
            return false;
    }
	
	// Update is called once per frame
	void Update () {

        meshCircle.transform.localScale = new Vector3(circleSize,circleSize,1);
        userPosInsideCircle.Set(child.transform.localPosition.x,
                                 child.transform.localPosition.z);
        //Vector2 normalized = 

        if(insideCircle(userPosInsideCircle))
        {
            //Debug.Log("Inside Circle!");
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

                if (analogButtonY > 1.0f)
                    analogButtonY = 1.0f;

                Vector3 endPosition = hand.transform.position + speed * dir * userPosInsideCircle.y;
                lRenderer.SetPosition(0, hand.transform.position);
                lRenderer.SetPosition(1, endPosition);
                lRenderer.startWidth = laserWidth;
                lRenderer.endWidth = laserWidth;
            }

            if (Input.GetKey(KeyCode.PageUp))
            {
                //Vector3 dir =  head.position - hand.position;
                Vector3 desiredMove = dir * speed * Time.deltaTime * userPosInsideCircle.y; // verificar se essa ultima variavel ta entre 0 e 1
                this.transform.position += desiredMove;

            }
            else
            {
                // target.SetActive(false);
            }
        }
        else
        {
            //Debug.Log("Out of the Circle");

            //Vector3 dir = this.transform.position - child.position;
            //restrict to 4DOF
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            Vector3 dir = head.transform.position - child.position;
            Vector3 desiredMove = (this.transform.position - child.position) * speed * Time.deltaTime;
            this.transform.position += desiredMove;
        }

        if (recalibrate)
        {
            this.transform.position = child.position;
            child.localPosition = new Vector3(0, 0, 0);
            recalibrate = false;
        }



        /*if ((child.position - this.transform.position).magnitude > circleSize)
        {
            Vector3 newCameraPosition = child.localPosition;

            Vector3 desiredMove = child.forward * speed;

            desiredMove = (child.position - this.transform.position) * speed * Time.deltaTime;

            //newCameraPosition.z += (speed) * Time.deltaTime;
            this.transform.Translate(desiredMove);// = newCameraPosition;
            
        }*/
	}
}
