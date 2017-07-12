using UnityEngine;
using System.Collections;

public class OSCReceiver : MonoBehaviour
{
    public GameObject obj;

	public string RemoteIP = "127.0.0.1";
	public int SendToPort = 9000;
	public int ListenerPort = 8000;

	public float yaw1 = 0;
	public float roll1 = 0;
	public float pitch1 = 0;
	public bool buttonB1 = false;

	public float yaw2 = 0;
	public float roll2 = 0;
	public float pitch2 = 0;
	public bool buttonB2 = false;

	private Osc handler;

    public string messageToShow = "";
	
	// Use this for initialization
	void Start ()
	{
		UDPPacketIO udp = (UDPPacketIO)FindObjectOfType (typeof(UDPPacketIO));
		udp.init (RemoteIP, SendToPort, ListenerPort);
		handler = (Osc)FindObjectOfType (typeof(Osc));
		handler.init (udp);
		handler.SetAllMessageHandler (AllMessageHandler);
	}
	
	// Update is called once per frame
	void Update ()
	{
       // obj.transform.eulerAngles = new Vector3(pitch1, yaw1, roll1);

        Vector3 euler1 = new Vector3(-pitch1, yaw1, -roll1);
        Quaternion newQuaternion1 = Quaternion.Euler(euler1);
        obj.transform.rotation = newQuaternion1;

    }

    public void AllMessageHandler (OscMessage oscMessage)
	{
		string msgString = Osc.OscMessageToString (oscMessage);
        messageToShow = msgString;
		string msgAddress = oscMessage.Address;
		//Debug.Log (msgString);

		switch (msgAddress) {
		case "/pie/rotation":
			yaw1 = (float)oscMessage.Values [0];
			roll1 = (float)oscMessage.Values [1];
			pitch1 = (float)oscMessage.Values [2];
			buttonB1 = (int)oscMessage.Values [3] == 1;
			/*yaw2 = (float)oscMessage.Values [4];
			roll2 = (float)oscMessage.Values [5];
			pitch2 = (float)oscMessage.Values [6];
			buttonB2 = (int)oscMessage.Values [7] == 1;*/
			break;
		}
	}
}
