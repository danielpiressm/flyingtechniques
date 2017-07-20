using UnityEngine;
using UnityEngine.VR;
using System;
using MyTechnic;
using System.Collections.Generic;

public class TrackerClient : MonoBehaviour
{
	// Filter parameters
	private bool isNewFrame;
	private DateTime frameTime;

	// Human
	public float avatarHeight;
	private Human trackedHuman;
	private string trackedHumanId;
	private Dictionary<string, Human> humans;
    public Transform bar;
	// Body transforms and joints

	// Spine
	public Transform spineBase;
	public Transform spineShoulder;

	private PointSmoothing spineBaseJoint;
	private PointSmoothing spineShoulderJoint;

	// Left arm
	public Transform leftShoulder;
	public Transform leftElbow;
	public Transform leftArm;

	private PointSmoothing leftShoulderJoint;
	private PointSmoothing leftElbowJoint;
	private PointSmoothing leftWristJoint;

	// Left leg
	public Transform leftHip;
	public Transform leftKnee;

	private PointSmoothing leftHipJoint;
	private PointSmoothing leftKneeJoint;
	private PointSmoothing leftAnkleJoint;

	// Right arm
	public Transform rightShoulder;
	public Transform rightElbow;
	public Transform rightArm;

	private PointSmoothing rightShoulderJoint;
	private PointSmoothing rightElbowJoint;
	private PointSmoothing rightWristJoint;

	// Right leg
	public Transform rightHip;
	public Transform rightKnee;

	private PointSmoothing rightHipJoint;
	private PointSmoothing rightKneeJoint;
	private PointSmoothing rightAnkleJoint;
    private float bardifference = 0.12f;

	void Start()
	{
		isNewFrame = false;
		frameTime = DateTime.Now;

		trackedHumanId = string.Empty;
		humans = new Dictionary<string, Human>();

		spineBaseJoint = new PointSmoothing();
		spineShoulderJoint = new PointSmoothing();

		leftShoulderJoint = new PointSmoothing();
		leftElbowJoint = new PointSmoothing();
		leftWristJoint = new PointSmoothing();
		leftHipJoint = new PointSmoothing();
		leftKneeJoint = new PointSmoothing();
		leftAnkleJoint = new PointSmoothing();

		rightShoulderJoint = new PointSmoothing();
		rightElbowJoint = new PointSmoothing();
		rightWristJoint = new PointSmoothing();
		rightHipJoint = new PointSmoothing();
		rightKneeJoint = new PointSmoothing();
		rightAnkleJoint = new PointSmoothing();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.PageDown)) // Mouse tap
		{
			string currentHumanId = GetHumanIdWithHandUp();

			if (humans.ContainsKey(currentHumanId)) 
			{
				trackedHumanId = currentHumanId;
				trackedHuman = humans[trackedHumanId];

				AdjustAvatarHeight();
				InputTracking.Recenter();
			}
		}

		if (humans.ContainsKey(trackedHumanId)) 
		{
			trackedHuman = humans[trackedHumanId];
			UpdateAvatarBody();
		}

		// Finally
		CleanDeadHumans();
		isNewFrame = false;
	}

	/// <summary>
	/// Gets the first human identifier with a hand above the head.
	/// </summary>
	private string GetHumanIdWithHandUp()
	{
		foreach (Human h in humans.Values) 
		{
			if (h.body.Joints[BodyJointType.leftHand].y  > h.body.Joints[BodyJointType.head].y ||
				h.body.Joints[BodyJointType.rightHand].y > h.body.Joints[BodyJointType.head].y)
			{
				return h.id;
			}
		}
		return string.Empty;
	}

	/// <summary>
	/// Adjusts avatar's height by calculating the 
	/// ratio between the user and avatar's height.
	/// </summary>
	private void AdjustAvatarHeight()
	{
		float lowerFootY = Mathf.Min(trackedHuman.body.Joints[BodyJointType.rightFoot].y, trackedHuman.body.Joints[BodyJointType.leftFoot].y);

		float userHeight = (trackedHuman.body.Joints[BodyJointType.head].y + 0.1f) - lowerFootY;
		float scaleRatio = userHeight / avatarHeight;
        /*bar.transform.position = new Vector3(bar.transform.position.x, userHeight - bardifference, bar.transform.position.z);
		spineBase.transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);*/
	}

	/// <summary>
	/// Updates the avatar body by filtering body 
	/// joints and applying them through rotations.
	/// </summary>
	private void UpdateAvatarBody()
	{
		ApplyFilterToJoints();
      //  Vector3 headRot = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye).eulerAngles;

		// Spine
		Vector3 spineUp = Utils.GetBoneDirection(spineShoulderJoint.Value, spineBaseJoint.Value);
		Vector3 spineRight = Utils.GetBoneDirection(rightShoulderJoint.Value, leftShoulderJoint.Value);
		Vector3 spineForward = Vector3.Cross(spineRight,spineUp);

		spineBase.position = spineBaseJoint.Value + new Vector3(0.0f, 0.15f, 0.0f);
		spineBase.rotation = Quaternion.LookRotation(spineForward, spineUp);

		// Left Arm
		leftShoulder.rotation = Utils.GetQuaternionFromRightUp(-Utils.GetBoneDirection(leftShoulderJoint.Value, spineShoulderJoint.Value), spineUp);
		leftElbow.rotation = Utils.GetQuaternionFromRightUp(-Utils.GetBoneDirection(leftWristJoint.Value, leftElbowJoint.Value), spineUp);
		leftArm.rotation = Utils.GetQuaternionFromRightUp(-Utils.GetBoneDirection(leftElbowJoint.Value, leftShoulderJoint.Value), spineUp);

		// Left Leg
		leftHip.rotation = Utils.GetQuaternionFromUpRight(-Utils.GetBoneDirection(leftKneeJoint.Value, leftHipJoint.Value), spineRight);
		leftKnee.rotation = Utils.GetQuaternionFromUpRight(-Utils.GetBoneDirection(leftAnkleJoint.Value, leftKneeJoint.Value), spineRight);

		// Right Arm
		rightShoulder.rotation = Utils.GetQuaternionFromRightUp(Utils.GetBoneDirection(rightShoulderJoint.Value, spineShoulderJoint.Value), spineUp);
		rightElbow.rotation = Utils.GetQuaternionFromRightUp(Utils.GetBoneDirection(rightWristJoint.Value, rightElbowJoint.Value), spineUp);
		rightArm.rotation = Utils.GetQuaternionFromRightUp(Utils.GetBoneDirection(rightElbowJoint.Value, rightShoulderJoint.Value), spineUp);

		// Right Leg
		rightHip.rotation = Utils.GetQuaternionFromUpRight(-Utils.GetBoneDirection(rightKneeJoint.Value, rightHipJoint.Value), spineRight);
		rightKnee.rotation = Utils.GetQuaternionFromUpRight(-Utils.GetBoneDirection(rightAnkleJoint.Value, rightKneeJoint.Value), spineRight);
	}

	/// <summary>
	/// Applies the noise filter to joints.
	/// </summary>
	private void ApplyFilterToJoints()
	{
		// Spine
		spineBaseJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.spineBase], isNewFrame, frameTime);
		spineShoulderJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.spineShoulder], isNewFrame, frameTime);

		// Left arm
		leftShoulderJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftShoulder], isNewFrame, frameTime);
		leftElbowJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftElbow], isNewFrame, frameTime);
		leftWristJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftWrist], isNewFrame, frameTime);

		// Left leg
		leftHipJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftHip], isNewFrame, frameTime);
		leftKneeJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftKnee], isNewFrame, frameTime);
		leftAnkleJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftAnkle], isNewFrame, frameTime);

		// Right arm
		rightShoulderJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightShoulder], isNewFrame, frameTime);
		rightElbowJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightElbow], isNewFrame, frameTime);
		rightWristJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightWrist], isNewFrame, frameTime);

		// Right leg
		rightHipJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightHip], isNewFrame, frameTime);
		rightKneeJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightKnee], isNewFrame, frameTime);
		rightAnkleJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightAnkle], isNewFrame, frameTime);
	}

	/// <summary>
	/// Updates frame with new body data if any.
	/// </summary>
	public void SetNewFrame(Body[] bodies)
	{
		isNewFrame = true;
		frameTime = DateTime.Now;

		foreach (Body b in bodies) 
		{
            try
            {  
				string bodyID = b.Properties[BodyPropertiesType.UID];
				if (!humans.ContainsKey(bodyID)) 
				{
					humans.Add(bodyID, new Human());
				}
				humans[bodyID].Update(b);
			} 
			catch (Exception e) 
			{
				Debug.LogError("[TrackerClient] ERROR: " + e.StackTrace);
			}
		}
	}

	void CleanDeadHumans()
	{
		List<Human> deadhumans = new List<Human>();

		foreach (Human h in humans.Values) 
		{
			if (DateTime.Now > h.lastUpdated.AddMilliseconds(1000))
			{
				deadhumans.Add(h);
			}
		}
		foreach (Human h in deadhumans) 
		{
			humans.Remove(h.id);
		}
	}
}