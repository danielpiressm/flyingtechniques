using System.Collections;
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
        Vector3 dir = head.position - hand.position;

        Debug.DrawRay(head.transform.position, -dir, Color.red);

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
