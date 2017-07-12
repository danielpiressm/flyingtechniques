using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndGo : MonoBehaviour {

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;

    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;

    TestTask tTask;
    bool rightHanded = true;
    Transform hand;

	// Use this for initialization
	void Start () {
        tTask = GetComponent<TestTask>();
        rightHanded = tTask.rightHanded;
	}
	
	// Update is called once per frame
	void Update () {
        if (tTask.rightHanded)
            hand = rightHand;
        else
            hand = leftHand;
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented
        Vector3 dir = head.position - hand.position;

        Debug.DrawRay(head.transform.position, -dir, Color.red);
        //map another button
		if(Input.GetKey(KeyCode.Space))
        {
            Vector3 desiredMove = -dir * speed * Time.deltaTime;
            this.transform.position += desiredMove;
            RaycastHit hit;
           // target.SetActive(true);
            //Debug.DrawRay(this.transform.position, this.transform.forward,Color.red);
            if (Physics.Raycast(head.position, -dir, out hit, raySize))
            {
                //target.transform.position = hit.transform.position;
                
            }
            else
            {
              //  target.transform.position = head.transform.TransformPoint(new Vector3(0, 0, raySize));
               
            }
        }
        else
        {
            //target.SetActive(false);
        }
	}
}
