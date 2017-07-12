using UnityEngine;

/// <summary>
/// Locks the VR headset pose to match OptiTrack's pose.
/// This allows the application to use Gear VR's orientation, which is
/// super smooth and pretty, while also getting OptiTrack's global
/// orientation.
/// </summary>
public class LockRotation : MonoBehaviour
{
    public OptiTrackTracking cameraPivot;

    float untrackedTimeout = 0.5f;
    bool appWasPaused = true;

    void Update()
    {
        if ((Application.isMobilePlatform && !Application.isEditor && Input.GetMouseButton(0))
            || appWasPaused
            || Time.time > cameraPivot.lastTrackedTime + untrackedTimeout)
        {
            // Get the OptiTrack pose, including orientation
            if (cameraPivot.ProcessOptiTrackInput(true))
            {
                // Get the side-to-side rotation only, Gear VR itself handles the other rotations
                Vector3 euler = transform.rotation.eulerAngles;
                cameraPivot.transform.rotation = Quaternion.Euler(0, euler.y, 0);

                // Reset the Gear VR's center rotation
                //OVRManager.display.RecenterPose();

                appWasPaused = false;
            }
        }
    }

    void OnApplicationPause()
    {
        appWasPaused = true;
    }
}
