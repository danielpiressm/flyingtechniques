using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;


public class Tracker : MonoBehaviour
{

	private Dictionary<string, PointCloudSimple> _clouds;
    private Dictionary<string, GameObject> _cloudGameObjects;


    void Awake ()
	{
        Debug.Log("Hello Tracker");
		_clouds = new Dictionary<string, PointCloudSimple> ();
        _cloudGameObjects = new Dictionary<string, GameObject>();
        _loadConfig ();

        UdpClient udp = new UdpClient();
        string message = AvatarMessage.createRequestMessage(1, TrackerProperties.Instance.listenPort);
        byte[] data = Encoding.UTF8.GetBytes(message);
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, TrackerProperties.Instance.trackerPort);
        Debug.Log("Sent request to port" + TrackerProperties.Instance.trackerPort + " with content " + message); 
        udp.Send(data, data.Length, remoteEndPoint);
    }
    

	internal void setNewCloud (CloudMessage cloud)
	{
        //if (!_clouds.ContainsKey (cloud.KinectId)) {
        //	Vector3 position = new Vector3 (Mathf.Ceil (_clouds.Count / 2.0f) * (_clouds.Count % 2 == 0 ? -1.0f : 1.0f), 1, 0);
        //          //criar nova cloud
        //          _clouds[cloud.KinectId] = new PointCloudSimple ();
        //}
        int step = cloud.headerSize+3; // TMA: Size in bytes of heading: "CloudMessage" + L0 + 2 * L1. Check the UDPListener.cs from the Client.
        string[] pdu = cloud.message.Split(MessageSeparators.L1);
       

        string KinectId = pdu[0]; 
        uint  id = uint.Parse(pdu[1]);
        step += pdu[0].Length + pdu[1].Length;

        if (_clouds.ContainsKey(KinectId))
        {
            if (pdu[2] == "") {
                _clouds[KinectId].setToView();
            }
            else
                _clouds[KinectId].setPoints(cloud.receivedBytes,step,id,cloud.receivedBytes.Length);
        }
    }

    //FOR TCP
    internal void setNewCloud(string KinectID, byte[] data,int size, uint id)
    {
     
         // tirar o id da mensagem que é um int
        if (_clouds.ContainsKey(KinectID))
        {
            _clouds[KinectID].setPoints(data, 0, id,size);
            _clouds[KinectID].setToView();
        }
    }

    private void _loadConfig ()
	{
		string filePath = Application.dataPath + "/" + TrackerProperties.Instance.configFilename;

		string port = ConfigProperties.load (filePath, "udp.listenport");
		if (port != "") {
			TrackerProperties.Instance.listenPort = int.Parse (port);
		}
		resetListening ();

        port = ConfigProperties.load(filePath, "udp.trackerport");
        if (port != "")
        {
            TrackerProperties.Instance.trackerPort = int.Parse(port);
        }
    }
    
	public void resetListening ()
	{
		gameObject.GetComponent<UdpListener> ().udpRestart ();
	}

	public void hideAllClouds ()
	{
		foreach (PointCloudSimple s in _clouds.Values) {
			s.hideFromView ();
		}
		UdpClient udp = new UdpClient ();
		string message = CloudMessage.createRequestMessage (2,Network.player.ipAddress, TrackerProperties.Instance.listenPort); 
		byte[] data = Encoding.UTF8.GetBytes(message);
		IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, TrackerProperties.Instance.listenPort + 1);
		udp.Send(data, data.Length, remoteEndPoint);
	}

	public void broadCastCloudRequests (bool continuous)
	{
		UdpClient udp = new UdpClient ();
		string message = CloudMessage.createRequestMessage (continuous ? 1 : 0, Network.player.ipAddress, TrackerProperties.Instance.listenPort); 
		byte[] data = Encoding.UTF8.GetBytes (message);
		IPEndPoint remoteEndPoint = new IPEndPoint (IPAddress.Broadcast, TrackerProperties.Instance.listenPort + 1);
		udp.Send (data, data.Length, remoteEndPoint);
	}
    
    public void processAvatarMessage(AvatarMessage av)
    {
        foreach (string s in av.calibrations)
        {
            string[] chunks = s.Split(';');
            string id = chunks[0];
            float px = float.Parse(chunks[1]);
            float py = float.Parse(chunks[2]);
            float pz = float.Parse(chunks[3]);
            float rx = float.Parse(chunks[4]);
            float ry = float.Parse(chunks[5]);
            float rz = float.Parse(chunks[6]);
            float rw = float.Parse(chunks[7]);

            GameObject cloudobj = new GameObject(id);
            cloudobj.transform.localPosition = new Vector3(px,py,pz);
            cloudobj.transform.localRotation = new Quaternion(rx,ry,rz,rw);
            cloudobj.transform.localScale = new Vector3(-1, 1, 1);
            cloudobj.AddComponent<PointCloudSimple>();
            PointCloudSimple cloud = cloudobj.GetComponent<PointCloudSimple>();
            _clouds.Add(id, cloud);
            _cloudGameObjects.Add(id, cloudobj);

        }
        Camera.main.GetComponent<MouseOrbitImproved>().target = _cloudGameObjects.First().Value.transform;
    }
}
