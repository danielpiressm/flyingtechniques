using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCircle : MonoBehaviour {


    public Vector3 calibratedPos;

    public float circleSize = 3;

    public bool recalibrate = false;

    public GameObject meshCircle;


    public Transform referenceJoint;

    [SerializeField]
    Vector2 userPosInsideCircle;

    [SerializeField]
    float circleSpeed = 0.0f;


    public Transform head;
    public Transform handTracker;
    public Transform leftHand;
    public Transform rightHand;

    public float speed = 3.0f;

    Transform hand;
    Vector3 initialrighttHandRotation = new Vector3(0, 8.995001f, 0);
    Vector3 initialLeftHandRotation = new Vector3(0, -8.995001f, 0);
    float initialSpineZPos = 0.0f;

    public float raySize = 3.0f;
    Camera camera;
    public float laserWidth;
    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;
    LineRenderer lRenderer;
    GameObject refJointCopy;

    float previousSpeed = 0.0f;
    private bool started = false;

    public float tolerance = 0.25f;
    GameObject rightToe;
    GameObject leftToe;

    GameObject pivotCircle;
    GameObject pivotCircle2;

    // Use this for initialization
    void Start () {

        pivotCircle = new GameObject("pivotCircle");
        pivotCircle2 = new GameObject("pivotCircle2");
        pivotCircle.transform.localScale = Vector3.one;
        calibratedPos = referenceJoint.position;
        tTask = GetComponent<TestTask>();
        camera = Camera.main;
        //tTask = GetComponent<TestTask>();
        hand = rightHand;
        lRenderer = GetComponent<LineRenderer>();
        //this.transform.position = referenceJoint.position;
        refJointCopy = new GameObject("auxRefJoint");
        refJointCopy.transform.parent = referenceJoint.transform.parent;
        refJointCopy.transform.position = referenceJoint.transform.position;
        refJointCopy.transform.rotation = Quaternion.identity;

        initialSpineZPos = referenceJoint.transform.localPosition.z;
        recalibrate = false;

        rightToe = GameObject.Find("mixamorig:RightToe_End");
        leftToe = GameObject.Find("mixamorig:LeftToe_End");

        pivotCircle.transform.parent = GameObject.Find("refCircle").transform;
        pivotCircle.transform.localPosition = new Vector3(0, 0, 0);
        pivotCircle2.transform.parent = pivotCircle.transform;
        pivotCircle2.transform.localPosition = new Vector3(0, 0, -0.3f);
        pivotCircle.transform.localScale = Vector3.one;
        pivotCircle.transform.localScale = Vector3.one;

        


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
        float xN = localPosition.x / circleRadius;
        float yN = localPosition.y / circleRadius;

        //Debug.Log("xN = (" + xN + "," + yN + ")");

        float result = (float)Mathf.Sqrt(xN * xN + yN* yN);
        float squareRadius = circleRadius * circleRadius;
        if (result < 1.08f)
        {
         //   Debug.Log("result true " + result + " squareRadius = "+ squareRadius +  " tolerance = " + tolerance);
            return true;

        }
        else
        {
           // Debug.Log("result false " + result + " tolerance = " + tolerance);
            return false;
        }
            
    }

    float getSpeed(Vector2 localPosition)
    {
        float circleRadius = circleSize / 2;
        float xN = localPosition.x / circleRadius;
        float yN = localPosition.y / circleRadius;
        
        float result = (float)Mathf.Sqrt(xN * xN + yN * yN);
        /*if (xN < 0.0f || yN < 0.0f)
            result = -result;// 0.0f;*/


        return result;
    }

    float getSpeed(Vector2 localPosition,float circleSize)
    {
        float circleRadius = circleSize / 2;
        float xN = localPosition.x / circleRadius;
        float yN = localPosition.y / circleRadius;

        float result = (float)Mathf.Sqrt(xN * xN + yN * yN);
        /*if (xN < 0.0f || yN < 0.0f)
            result = 0.0f;*/

        return result;
    }

    Vector2 getFrontFacingFeetPos()
    {
        Vector2 pos;

        Vector3 transformedRight = meshCircle.transform.parent.transform.InverseTransformPoint(rightToe.transform.position);
        Vector3 transformedLeft = meshCircle.transform.parent.transform.InverseTransformPoint(leftToe.transform.position);

        float distance = transformedRight.z - transformedLeft.z;

        Vector2 rightToeT = new Vector2(transformedRight.x, transformedRight.z);
        Vector2 leftToeT = new Vector2(transformedLeft.x, transformedLeft.z);
        if (distance > 0)
            pos = rightToeT;
        else
            pos = leftToeT;
       // Debug.Log("distance between toes = "+ distance + "RightToe = "+ transformedRight.ToString() + " LeftToe = " + transformedLeft.ToString());
        return pos;
    }

    void recalibrateCircle()
    {
        Vector3 vec3 = new Vector3(0, referenceJoint.transform.localPosition.y,0);
        vec3.Set(0, vec3.y - 0.15f, 0.0f);

        GameObject reffCircle = GameObject.Find("refCircle");
        reffCircle.transform.position = this.transform.TransformPoint(vec3);
        Debug.Log("recalibrate");
        /*recalibrate = true;
        Debug.Log("recalibrating circle");*/
    }


    // Update is called once per frame
    void Update () {

        meshCircle.transform.localScale = new Vector3(circleSize,circleSize,1);
        
        Transform refCircle = meshCircle.transform.parent;
        pivotCircle.transform.rotation = referenceJoint.transform.rotation;

        

        Vector3 cpy = refCircle.transform.TransformPoint(refCircle.transform.localPosition.x, refCircle.transform.localPosition.y, refCircle.transform.localPosition.z - 0.4f);

        Vector3 cpy2 = this.transform.InverseTransformPoint(cpy);
        

        Vector3 transformedPoint = this.transform.InverseTransformPoint(referenceJoint.transform.position);
        Vector3 transformedPoint2 = pivotCircle.transform.InverseTransformPoint(referenceJoint.transform.position);

        Vector3 forwardTransformCircle = refCircle.InverseTransformDirection(referenceJoint.transform.forward);



       //Debug.DrawRay(pivotCircle2.transform.position, pivotCircle2.transform.forward,Color.green);
       

        refJointCopy.transform.position = new Vector3(referenceJoint.transform.position.x, refCircle.transform.position.y, referenceJoint.transform.position.z);
        Vector3 dirBetweenRefJointAndCircle = refJointCopy.transform.position - refCircle.transform.position;



        Vector3 right = Vector3.Cross(dirBetweenRefJointAndCircle, refCircle.up);
        Debug.DrawRay(pivotCircle.transform.position, dirBetweenRefJointAndCircle, Color.cyan);
        Debug.DrawRay(pivotCircle.transform.position, right, Color.yellow);


        float dotProduct = Vector3.Dot(dirBetweenRefJointAndCircle, referenceJoint.forward);
        //Debug.Log("dotProduct = "+dotProduct);
        //Debug.DrawRay(refCircle.transform.position, dirBetweenRefJointAndCircle ,Color.magenta);
        Color color;
        if (dotProduct > 0.0f)
            color = Color.cyan;
        else
            color = Color.green;

        

        userPosInsideCircle.Set(transformedPoint.x,
                                 transformedPoint.z);//Compensation for feet position
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
            circleSpeed = getSpeed(userPosInsideCircle,circleSize);
            if (circleSpeed < 0.3f)
                circleSpeed = 0.0f;

            

         

            if (tTask.started)
            {
                //Vector3 dir =  head.position - hand.position;
                if(dotProduct > -0.01f && insideCircle(userPosInsideCircle))
                {
                    Vector3 desiredMove = dir * speed * Time.deltaTime * circleSpeed;// getSpeed(userPosInsideCircle);// userPosInsideCircle.y; // verificar se essa ultima variavel ta entre 0 e 1
                    this.transform.position += desiredMove;
                    if (tTask)
                    {
                        tTask.setNavigationState(true, circleSpeed, previousSpeed);
                        tTask.setSpeed(speed * circleSpeed);
                    }
                }
                else
                {
                    circleSpeed = -speed;
                    if (tTask)
                    {
                        tTask.setNavigationState(false, circleSpeed, previousSpeed);
                        tTask.setSpeed(0);
                    }
                }
                previousSpeed = circleSpeed;
            }
            


            
            
            if (getSpeed(userPosInsideCircle) > 0.8f)
            {
                refCircle.transform.Find("warningCircle").gameObject.SetActive(true);
                refCircle.transform.Find("warningCircle").gameObject.transform.localScale = new Vector3(circleSize+ tolerance, circleSize + tolerance, 1) ;
            }
            else
            {
                refCircle.transform.Find("warningCircle").gameObject.SetActive(false);
            }
            
        }
        else
        {
            refCircle.transform.Find("warningCircle").gameObject.SetActive(true);
            if (tTask)
            {
                tTask.setNavigationState(false, circleSpeed, previousSpeed);
                tTask.setSpeed(0);
            }

            //Debug.Log("Out of the Circle");

            //Vector3 dir = this.transform.position - child.position;
            //restrict to 4DOF
            /*this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            Vector3 dir = head.transform.position - child.position;
            Vector3 desiredMove = (this.transform.position - child.position) * speed * Time.deltaTime;
            this.transform.position += desiredMove;*/
        }

        if (recalibrate)
        {
            Vector3 refJointPosCopy = referenceJoint.transform.position;
            Quaternion refJointRotCopy = referenceJoint.transform.rotation;
            this.transform.position = new Vector3(referenceJoint.transform.position.x, this.transform.position.y, referenceJoint.transform.position.z);

           



            //refCircle.localPosition = new Vector3(0, 0, 0);
            recalibrate = false;
            Debug.Log("roger roger");
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
