﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGaze : MonoBehaviour {

    public Transform head;
    public Transform hand;
    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;
    public Camera camera;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented

        Vector3 dir = Vector3.ProjectOnPlane(head.transform.forward, this.transform.up);


        Debug.DrawRay(head.transform.position, Vector3.ProjectOnPlane(head.transform.forward,this.transform.up), Color.red);

        if(Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }



        if (Input.GetKey(KeyCode.Space))
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
