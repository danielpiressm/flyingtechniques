using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutColliderInEverybody : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform t = this.transform.GetChild(i);
            if (t.GetComponent<MeshCollider>())
            {
                t.GetComponent<MeshCollider>().convex = true;
                t.GetComponent<MeshCollider>().inflateMesh = true;
                t.GetComponent<MeshCollider>().isTrigger = true;

                if (!t.GetComponent<CollisionTrigger>())
                {
                    t.gameObject.AddComponent<CollisionTrigger>();
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
       
	}
}
