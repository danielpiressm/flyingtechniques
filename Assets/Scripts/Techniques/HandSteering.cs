using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSteering : MonoBehaviour
{

    public Transform head;
    public Transform handTracker;
    public Transform leftHand;
    public Transform rightHand;

    Transform hand;
    Quaternion initialrighttHandRotation;
    Quaternion initialLeftHandRotation;

    public float speed = 3.0f;
    public float raySize = 3.0f;
    Camera camera;
    public float laserWidth; 
    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;
    LineRenderer lRenderer;



    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        tTask = GetComponent<TestTask>();
        hand = rightHand;
        lRenderer = GetComponent<LineRenderer>();
        initialrighttHandRotation = rightHand.rotation;
        initialLeftHandRotation = leftHand.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented

        if (tTask)
        {
            if (tTask.rightHanded)
            {
                hand = rightHand;
                leftHand.transform.rotation = Quaternion.identity;
            }
            else
            {
                hand = leftHand;
                rightHand.transform.rotation = Quaternion.identity;
            }



            if (tTask.rightHanded == true)
                hand.transform.rotation = Quaternion.LookRotation(handTracker.transform.up, handTracker.transform.forward);
            else if (tTask.rightHanded == false)
                hand.transform.rotation = Quaternion.LookRotation(-handTracker.transform.up, handTracker.transform.forward);

        }


        Vector3 dir = handTracker.transform.forward;
        Debug.DrawRay(hand.transform.position, dir, Color.red);
        
        if (lRenderer !=null)
        {
            lRenderer.enabled = true;
            //hand.transform.eulerAngles = new Vector3(handTracker.transform.eulerAngles.x, handTracker.transform.eulerAngles.z, handTracker.transform.eulerAngles.y);

            //hand.transform.up = -handTracker.transform.forward;
            
            //hand.transform.localEulerAngles = new Vector3(hand.transform.localEulerAngles.x, hand.transform.localEulerAngles.y, hand.transform.localEulerAngles.z);

            Vector3 endPosition = hand.transform.position + 3.0f * dir;
            lRenderer.SetPosition(0, hand.transform.position);
            lRenderer.SetPosition(1, endPosition);
            lRenderer.startWidth = laserWidth;
            lRenderer.endWidth = laserWidth;
        }

        if (Input.GetKey(tTask.getForwardButton()))
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
           
        }
        else
        {
            // target.SetActive(false);
        }
    }
}
