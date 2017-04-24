﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndGo : MonoBehaviour {

    public Transform head;
    public Transform hand;
    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented



		if(Input.GetKey(KeyCode.Space))
        {
            Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = head.transform.forward * speed * Time.deltaTime;
            this.transform.Translate(desiredMove);
            RaycastHit hit;
            target.SetActive(true);
            Debug.DrawRay(this.transform.position, this.transform.forward,Color.red);
            if (Physics.Raycast(head.position, -dir, out hit, raySize))
            {
                target.transform.position = hit.transform.position;
                
            }
            else
            {
                target.transform.position = head.transform.TransformPoint(new Vector3(0, 0, raySize));
               
            }
        }
        else
        {
            target.SetActive(false);
        }
	}
}
