using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNGo : MonoBehaviour {

    public enum StateS
    {
        Init,
        Lock,
        Drag,
        Go
    };


    public Transform head;
    public Transform hand;
    public Transform arm;
    public Transform forearm;

    public float speed = 3.0f;
    public GameObject target;
    public float raySize = 3.0f;
    public float noHitDistance = 10.0f;
    public float armAngle = 0;

    public float lastArmAngle = 0.0f;
    public StateS currentState;
    public float maxAngle = 100.0f;//in degrees

    public bool lockF = false;
   
    // Use this for initialization
    void Start () {
        currentState = StateS.Init;	
	}

    float ClampAngle(float angle)
    {
        if (angle < 0)
            angle += 360;
        else if (angle > 360)
            angle -= 360;

        if (Mathf.Abs(90 - angle) < Mathf.Abs(270 - angle))
        {
            if (angle > 89)
                angle = 89;
        }
        else
        {
            if (angle < 271)
                angle = 271;
        }

        return angle;
    }

    float normalizeAngle(float angle)
    {
        if (angle > maxAngle)
            return 1.0f;
        else
            return angle / maxAngle;
        //return (angle > maxAngle ? maxAngle : angle / maxAngle); 
    }

    

    // Update is called once per frame
    void Update () {
        //TODO: Implement drag & go
        //Also: Point to the place & fix the direction & use a button
        //Gaze oriented
        Vector3 dir = head.position - hand.position;

        Debug.DrawRay(head.transform.position, -dir, Color.red);

        float angle = Vector3.Angle(hand.transform.position - forearm.transform.position, forearm.transform.position - arm.transform.position);
        float currentArmAngle = normalizeAngle(angle);

        currentArmAngle = armAngle;
        // transform.position += m_go_step * (m_last_mouse_position.y - Input.mousePosition.y);

        // m_last_mouse_position = Input.mousePosition;
        //
        Debug.DrawRay(head.position, -dir, Color.yellow);
        //if (Input.GetKey(KeyCode.Space))
        //{
        Vector3 desiredMove = new Vector3(0,0,0);
        float hitDistance = 0.0f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!lockF)
            {
                desiredMove = -dir;
                lockF = true;
                RaycastHit hit2;
                target.GetComponent<MeshRenderer>().enabled = true;
                
                if (Physics.Raycast(head.position, -dir, out hit2, noHitDistance))
                {
                    target.transform.position = hit2.point;
                    hitDistance = (target.transform.position - head.position).normalized.magnitude;
                }
                else
                {
                    target.transform.position = head.transform.TransformPoint(new Vector3(0, 0, noHitDistance));
                    hitDistance = noHitDistance;
                }

                desiredMove = target.transform.position - head.position;
            }
            else
            {
                lockF = false;
                target.GetComponent<MeshRenderer>().enabled = false;
            }
                
        }
        
        if(lockF)
        {
            hitDistance = (target.transform.position - head.position).magnitude;
            Debug.DrawLine(head.position, target.transform.position, Color.blue);
            float delta = lastArmAngle - currentArmAngle;

            this.transform.position += ( -(target.transform.position - head.position) * delta * hitDistance);

            // m_go_step = (hit.point - transform.position) / m_mouse_start_position.y;

            lastArmAngle = currentArmAngle;
        }    
        else
        {
            //do nothing (for now)
        }
        


            /*desiredMove = -dir ;

            //this.transform.position += ( desiredMove * delta);
            lastArmAngle = currentArmAngle;
            RaycastHit hit;
            if (Physics.Raycast(head.position, -dir, out hit,noHitDistance))
            {
                Debug.DrawLine(head.position, hit.transform.position, Color.blue);
                target.transform.position = hit.point;//.position;
                hitDistance = (hit.point - head.position).normalized.magnitude;

                this.transform.position += (desiredMove * delta * hitDistance);
                //this.transform.position += (desiredMove * new Vector3(hit.point.x *delta, hit.point.y *delta, hit.point.z *delta));
            }
            else
            {

                target.transform.position = (head).transform.TransformPoint(new Vector3(0, 0, noHitDistance));
                Debug.DrawLine(head.position, target.transform.position, Color.blue);
                //Debug.Log("ELSE");
                //target.transform.position = head.transform.TransformPoint(new Vector3(0, 0, noHitDistance));
                //this.transform.position += (desiredMove * delta * noHitDistance);
                //  target.transform.position = head.transform.TransformPoint(new Vector3(0, 0, raySize));

            }*/
        //}
       /* else
        {
            //target.SetActive(false);
        }*/
	}
}
