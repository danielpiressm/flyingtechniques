using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour {

    LineRenderer lineRenderer;
    GameObject ringRoot;
    public float sphereSize;
    public Material mat;
    GameObject sphereRoot;
    GameObject[] spheres;// TestTask tTask;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 22;
        ringRoot = GameObject.Find("first__circuit");
        spheres = new GameObject[22];
        sphereRoot = new GameObject("sphereRoot");
        for(int i = 0;i < 22;i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].name = "Sphere " + i;
            spheres[i].transform.parent = sphereRoot.transform;
            //spheres[i].renderer.GetComponent<Material>().
        }
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < ringRoot.transform.childCount;i++)
        {
            lineRenderer.SetPosition(i,ringRoot.transform.GetChild(i).GetChild(0).position);
            spheres[i].transform.position = new Vector3(lineRenderer.GetPosition(i).x, lineRenderer.GetPosition(i).y, lineRenderer.GetPosition(i).z);
            spheres[i].transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            spheres[i].GetComponent<Renderer>().sharedMaterial = mat;
        }
        RingCollider[] rings = ringRoot.GetComponentsInChildren<RingCollider>();
	
	}
}
