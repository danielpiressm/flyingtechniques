using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollider : MonoBehaviour
{

    private Camera camera;
    private TestTask testTask;
    private Plane plane;

    // Use this for initialization
    void Start()
    {
        GameObject virtualCircle = GameObject.Find("Virtual_Circle");
        camera = (Camera)virtualCircle.GetComponentInChildren<Camera>();
        testTask = (TestTask)virtualCircle.GetComponent<TestTask>();
    }

    // Update is called once per frame
    void Update()
    {
        plane = new Plane(transform.up, transform.position);
        // has crossed plane
        if (!plane.GetSide(camera.transform.position))
        {
            // Pythagoras
            float distanceToPlane = plane.GetDistanceToPoint(camera.transform.position);
            float distanceToRingCenter = Vector3.Distance(transform.position, camera.transform.position);
            float flatennedDistanceToRingCenter = Mathf.Sqrt(Mathf.Pow(distanceToRingCenter, 2) - Mathf.Pow(distanceToPlane, 2));
            float normalizedDistanceToRingCenter = flatennedDistanceToRingCenter / (transform.lossyScale.x / 2.0f);

            if (flatennedDistanceToRingCenter < (transform.lossyScale.x * 1.1))
            {
                testTask.Next(true, normalizedDistanceToRingCenter);
                //testRunner.Next(true, normalizedDistanceToRingCenter);
            }
            else
            {
                testTask.Next(false, normalizedDistanceToRingCenter);
                //testRunner.Next(false, normalizedDistanceToRingCenter);
            }

        }
    }
}