using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCircle : MonoBehaviour {

    public float speed = 3.0f;

    public Vector3 calibratedPos;

    public float circleSize = 2;

    public bool recalibrate = false;

    public GameObject meshCircle;

    public Transform child;

    Vector2 userPosInsideCircle;

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
           
        }
        else
        {
            //Debug.Log("Out of the Circle");
            Vector3 desiredMove = (this.transform.position - child.position) * speed * Time.deltaTime;
            this.transform.Translate(desiredMove);
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
