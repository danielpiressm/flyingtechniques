using UnityEngine;
using OptitrackManagement;

public class QuickOptiTrackPlace : MonoBehaviour
{
    public int id;
    public string wiimoteButton = "plus";
    public Transform pivot;

    void Update()
    {
        /*if (.GetButton("plus") && InputManager.GetButton("minus"))
        {
            RigidBody[] bodies = DirectMulticastSocketClient.GetStreemData()._rigidBody;

            for (int i = 0; i < bodies.Length; i++)
            {
                if (bodies[i].ID == id)
                {
                    Place(bodies[i]);
                    break;
                }
            }
        }*/
    }

    void Place(RigidBody body)
    {
        Vector3 position = body.pos;
        position.x = -position.x;

        transform.position = position - pivot.localPosition;
    }
}
