using UnityEngine;
using OptitrackManagement;

/// <summary>
/// Sets a GameObject's position and rotation to a RigidBody
/// tracked via OptiTrack, specified with an ID.
/// </summary>
public class OptiTrackTracking : MonoBehaviour
{
    public int ID;
    public bool useOrientation = true;
    public bool applyCorrection;

    public float lastTrackedTime { get; private set; }

    ~OptiTrackTracking()
    {
        DirectMulticastSocketClient.Close();
    }

    void Start()
    {
        DirectMulticastSocketClient.Start();
    }

    protected virtual void Update()
    {
        ProcessOptiTrackInput(useOrientation);
    }

    public bool ProcessOptiTrackInput(bool useOrientation)
    {
        RigidBody[] rigidBodies = DirectMulticastSocketClient.GetStreemData()._rigidBody;

        for (int i = 0; i < rigidBodies.Length; i++)
        {
            if (rigidBodies[i].ID == ID)
            {
                ApplyOptiTrackTransformToObject(rigidBodies[i], useOrientation);
                lastTrackedTime = Time.time;
                return true;
            }
        }
        return false;
    }
    
    void ApplyOptiTrackTransformToObject(RigidBody rigidBody, bool useOrientation)
    {
        Vector3 pos = rigidBody.pos;
        pos.x *= -1;
        //transform.position = pos;

        if (applyCorrection)
            transform.position += new Vector3(0, 0.1f, 0);
        // Gear VR object should not get its orientation via OptiTrack,
        // but from Gear VR's own sensors
        // Exception: in the Editor, there is no Gear VR sensors, we need the OptiTrack orientation
        if (useOrientation || Application.isEditor)
        {
            transform.rotation = Quaternion.Inverse(rigidBody.ori);
            
        }
            
    }

}
