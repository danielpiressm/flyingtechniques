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

    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;
    public Camera camera;

    string text = "rightHand";
    TestTask tTask;
    bool rightHanded = true;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
        tTask = GetComponent<TestTask>();
        hand = rightHand;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented

        if (tTask.rightHanded)
            hand = rightHand;
        else
            hand = leftHand;


        Vector3 dir = handTracker.transform.forward;
        Debug.DrawRay(hand.transform.position, -dir, Color.red);

        if (Input.GetKey(tTask.getForwardButton()))
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = -dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
           
        }
        else
        {
            // target.SetActive(false);
        }
    }
}
