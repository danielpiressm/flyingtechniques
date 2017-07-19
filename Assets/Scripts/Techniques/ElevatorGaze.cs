using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGaze : MonoBehaviour {

    public Transform head;
    public Transform hand;
    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;
    public Camera camera;
    TestTask tTask;

    // Use this for initialization
    void Start () {
        tTask = GetComponent<TestTask>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update () {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented

        Vector3 dir = Vector3.ProjectOnPlane(camera.transform.forward, this.transform.up);


        Debug.DrawRay(head.transform.position, dir, Color.red);

        if(Input.GetKey(tTask.getUpButton()))
        {
            this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if(Input.GetKey(tTask.getDownButton()))
        {
            this.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }
        /*if(Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }*/



        if (Input.GetKey(tTask.getForwardButton()))
        {
            //Vector3 dir =  head.position - hand.position;
            Vector3 desiredMove = dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
        }
        /*else if(Input.GetKey(KeyCode.PageDown))
        {
            Vector3 desiredMove = -dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
        }*/
    }
}
