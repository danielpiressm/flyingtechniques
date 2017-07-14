using UnityEngine;
using System.Collections;

public class CollisionTrigger : MonoBehaviour {

    public string Id;
    float lastTime = 0;
    private TestTask tTask;

	// Use this for initialization
	void Start () {
        tTask = GameObject.Find("Virtual_Circle").GetComponent<TestTask>();
        MeshCollider mCollider = this.GetComponent<MeshCollider>();
        if (!mCollider)
        {
            BoxCollider bCollider = this.GetComponent<BoxCollider>();
            bCollider.isTrigger = true;
        }
        else
        {
            mCollider.convex = true;
            mCollider.isTrigger = true;
        }
            
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    string Vec3ToStr(Vector3 vec,string separator)
    {
        return vec.x + separator + vec.y + separator + vec.z;
    }

    

    void OnTriggerEnter(Collider collider)
    {
        /* collisionLogStr = "Joint" + separator + "PosX" + separator + "PosY" + separator + "PosZ" + separator + "RotX" + separator + "RotY" + separator + "RotZ" + separator +
                         "ColliderName" + separator + "PosColliderX" + separator + "PosColliderY" + separator + "PosColliderZ" + separator + "RotColliderX" + separator + "RotColliderZ" +
                         "ErrorX" + separator + "ErrorY" + separator + "ErrorZ" + separator + "TimeElapsed" + "\n";*/

        Transform headTransform = null;
        HeadCameraController head;
        Vector3 headPos;
        Vector3 headRot;

        try
        {
            head = (HeadCameraController)Camera.main.transform.parent.gameObject.GetComponent<HeadCameraController>();
            headTransform = head.headTransform;
        }
        catch (System.Exception ex)
        {

        }

        float currentTime = Time.realtimeSinceStartup;
        float triggerTime = currentTime - lastTime;
        lastTime = currentTime;
        Vector3 pos = new Vector3(collider.transform.position.x, collider.transform.position.y, collider.transform.position.z);
        Vector3 rot = new Vector3(collider.transform.eulerAngles.x, collider.transform.eulerAngles.y, collider.transform.eulerAngles.z);
        Vector3 vec = collider.transform.position - this.transform.position;
        Vector3 vec2 = Camera.main.transform.InverseTransformPoint(collider.transform.position);
        Debug.Log("Collision between  " + this.gameObject.name + " and " + collider.gameObject.name);
        if (collider.gameObject.name == "Plane" || collider.gameObject.name.Contains("ground") || collider.gameObject.name == "triggerObject1" || collider.gameObject.name == "triggerObject2")
            return;


        if (headTransform == null)
        {
            headPos = Vector3.zero;
            headRot = Vector3.zero;
        }
        else
        {
            headPos = headTransform.position;
            headRot = headTransform.eulerAngles;
            vec2 = headTransform.InverseTransformPoint(pos);
        }


        //rotacao da cabeca ou da camera (eu voto pela camera :-))
        string str = string.Join(",", new string[]
        {
            "#"+collider.gameObject.name,
            //this.Id,
            pos.x.ToString(),
            pos.y.ToString(),
            pos.z.ToString(),
            rot.x.ToString(),
            rot.y.ToString(),
            rot.z.ToString(),
            this.Id,
            this.transform.position.x.ToString(),
            this.transform.position.y.ToString(),
            this.transform.position.z.ToString(),
            this.transform.eulerAngles.x.ToString(),
            this.transform.eulerAngles.y.ToString(),
            this.transform.eulerAngles.z.ToString(),
            vec.x.ToString(),
            vec.y.ToString(),
            vec.z.ToString(),
            vec2.x.ToString(),
            vec2.y.ToString(),
            vec2.z.ToString(),
            headPos.x.ToString(),
            headPos.y.ToString(),
            headPos.z.ToString(),
            Camera.main.transform.position.x.ToString(),
            Camera.main.transform.position.y.ToString(),
            Camera.main.transform.position.z.ToString(),
            "\n"
        });
        if(tTask)
            tTask.serializeCollision(str);
        //SendMessageUpwards("serializeCollision", str);
        

    }

    void OnCollisionEnter(Collision collider)
    {
        Transform headTransform = null;
        HeadCameraController head;
        Vector3 headPos;
        Vector3 headRot;

        float currentTime = Time.realtimeSinceStartup;
        float triggerTime = currentTime - lastTime;
        lastTime = currentTime;
        string colliderName = collider.contacts[0].thisCollider.gameObject.name;
        Vector3 pos = new Vector3(collider.transform.position.x, collider.transform.position.y, collider.transform.position.z);
        Vector3 rot = new Vector3(collider.transform.eulerAngles.x, collider.transform.eulerAngles.y, collider.transform.eulerAngles.z);
        Vector3 vec = collider.transform.position - collider.contacts[0].thisCollider.gameObject.transform.position;
        Vector3 vec2 = Camera.main.transform.InverseTransformPoint(pos);

        try
        {
            head = (HeadCameraController) Camera.main.transform.parent.gameObject.GetComponent<HeadCameraController>();
            headTransform = head.headTransform;
        }catch(System.Exception ex)
        {
            
        }

        Transform bounds = collider.contacts[0].thisCollider.gameObject.transform;
        Debug.Log("$$Collision between  " + this.Id + " and " + collider.gameObject.name);
        if(headTransform==null)
        {
            headPos = Vector3.zero;
            headRot = Vector3.zero;
        }
        else
        {
            headPos = headTransform.position;
            headRot = headTransform.eulerAngles;
            vec2 = headTransform.InverseTransformPoint(pos);
        }


        if (collider.gameObject.name == "Plane" || collider.gameObject.name.Contains("ground") || collider.gameObject.name == "triggerObject1" || collider.gameObject.name == "triggerObject2" )
            return;
        /*collisionLogStr = "Joint" + separator + "PosX" + separator + "PosY" + separator + "PosZ" + separator + "RotX" + separator + "RotY" + separator + "RotZ" + separator +
                        "ColliderName" + separator + "PosColliderX" + separator + "PosColliderY" + separator + "PosColliderZ" + separator + "RotColliderX" + separator + "RotColliderY" +
                        separator + "RotColliderZ" + separator + "ErrorX" + separator + "ErrorY" + separator + "ErrorZ" + separator+ "TimeElapsed"+ separator + "CurrentTime"+ separator + "CurrentTask"+"\n";*/
        string str = string.Join(",", new string[]
        {
            collider.gameObject.name,
            //this.Id,
            pos.x.ToString(),
            pos.y.ToString(),
            pos.z.ToString(),
            rot.x.ToString(),
            rot.y.ToString(),
            rot.z.ToString(),
            this.name,
            bounds.position.x.ToString(),
            bounds.position.y.ToString(),
            bounds.position.z.ToString(),
            bounds.eulerAngles.x.ToString(),
            bounds.eulerAngles.y.ToString(),
            bounds.eulerAngles.z.ToString(),
            vec.x.ToString(),
            vec.y.ToString(),
            vec.z.ToString(),
            vec2.x.ToString(),
            vec2.y.ToString(),
            vec2.z.ToString(),
            headPos.x.ToString(),
            headPos.y.ToString(),
            headPos.z.ToString(),
            Camera.main.transform.position.x.ToString(),
            Camera.main.transform.position.y.ToString(),
            Camera.main.transform.position.z.ToString(),
            "\n"
        });
        if (tTask)
            tTask.serializeCollision(str);
        //SendMessageUpwards("serializeCollision", str);

    }

    void OnTriggerStay(Collider collider)
    {
        Vector3 vec = collider.transform.position - this.transform.position;
       // collision.rigidbody.
        //Debug.Log("Contact 1 : " +collision.gameObject.name);
        //Debug.Log("Contact 2 : " + other.ClosestPointOnBounds(other.transform.position).ToString());
    }
}
