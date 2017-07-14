﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSteering : MonoBehaviour
{

    public Transform head;
    public Transform handTracker;
    public Transform hand;
    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;
    public Camera camera;


    TestTask tTask;
    // Use this for initialization
    void Start()
    {
        tTask = GetComponent<TestTask>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented


        Debug.DrawRay(hand.transform.position, handTracker.transform.forward, Color.red);

        if (Input.GetKey(tTask.getForwardButton()))
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = handTracker.transform.forward * speed * Time.deltaTime;
            this.transform.position += desiredMove;
            RaycastHit hit;
            //target.SetActive(true);
            //Debug.DrawRay(head.transform.position, head.transform.forward,Color.red);
            if (Physics.Raycast(head.position, hand.transform.forward, out hit, raySize))
            {
                //target.transform.position = hit.transform.position;

            }
            else
            {
                //target.transform.position = head.transform.TransformPoint(new Vector3(0, 0, raySize));

            }
        }
        else
        {
            // target.SetActive(false);
        }
    }
}
