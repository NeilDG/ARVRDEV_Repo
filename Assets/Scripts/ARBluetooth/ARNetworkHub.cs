using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer;
using UnityEngine.Networking;

/// <summary>
/// AR network hub for bluetooth connectivity.
/// 
/// ISSUES: 
/// - Inheriting AndroidBluetoothNetworkManager for implementing own NetworkManager class does not correctly start the client
/// - Using AndroidBluetoothNetworkManager instead of NetworkManager does not seem to pose any issues. 
/// - The network message gets "consumed" once referenced.
/// </summary>
public class ARNetworkHub : MonoBehaviour {

	private static ARNetworkHub sharedInstance = null;
	public static ARNetworkHub Instance {
		get {
			return sharedInstance;
		}
	}


	[SerializeField] private ARNetworkManagerHelper bluetoothHelper;

	private bool isServer = false;
	private bool hasThrownMessage = false;

	void Awake() {
		sharedInstance = this;
		ARMessageQueue.Initialize ();
	}

	void OnDestroy() {
		sharedInstance = null;
		ARMessageQueue.Destroy ();
	}

	public AndroidBluetoothNetworkManagerHelper GetBluetoothHelperInstance() {
		return this.bluetoothHelper;
	}

	public void EnableBluetooth() {
		if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled ()) {
			ConsoleManager.LogMessage ("Bluetooth already enabled.");
		} else {
			if (AndroidBluetoothMultiplayer.RequestEnableBluetooth ()) {
				ConsoleManager.LogMessage ("Attempting to initialize bluetooth");
			}
		}
	}

	public bool IsBluetoothEnabled() {
		return AndroidBluetoothMultiplayer.GetIsBluetoothEnabled ();
	}

	public void StartAsHost() {
		this.isServer = true;
		this.bluetoothHelper.StartHost ();
		ConsoleManager.LogMessage ("Attempting to start as server");
	}

	public void StartAsClient() {
		if (this.isServer) {
			ConsoleManager.LogMessage ("Cannot start as client because the device is set as a host server.");
		} else {
			ConsoleManager.LogMessage ("Attempting to start as client");
			this.bluetoothHelper.SetCustomDeviceBrowser (null);
			this.bluetoothHelper.StartClient ();
		}

	}

	public void StartScan() {
		AndroidBluetoothMultiplayer.StartDiscovery ();
		ConsoleManager.LogMessage("Started discovery");
	}

	public void RegisterNetworkEvents() {
		if (NetworkServer.active) {
			NetworkServer.RegisterHandler (ARNetworkMessage.messageType, this.OnServerHandleMessage);
			ConsoleManager.LogMessage ("Successfully registered server handler");
		}

		if (NetworkManager.singleton.client != null) {
			NetworkManager.singleton.client.RegisterHandler (ARNetworkMessage.messageType, this.OnHandleClientMessage);
			ConsoleManager.LogMessage ("Client " + NetworkManager.singleton.client.ToString() + " has successfully started.");
		} else {
			ConsoleManager.LogMessage ("Did not do anything. No client found.");
		}

	}
	public void SendDummyData() {
		// Send the message with the tap position to the server, so it can send it to other clients
		ARNetworkMessage arMsg = new ARNetworkMessage ();
		arMsg.destination = new Vector3 (5.0f, 5.0f, 5.0f);
		arMsg.actionType = ARNetworkMessage.ActionType.TEST_DATA;
		NetworkManager.singleton.client.Send(ARNetworkMessage.messageType, arMsg);
	}

	private void OnHandleClientMessage(NetworkMessage networkMsg) {
		ARNetworkMessage arMessage = networkMsg.ReadMessage<ARNetworkMessage> ();

		if (arMessage.actionType != ARNetworkMessage.ActionType.TEST_DATA) {
			ARMessageQueue.Instance.EnqueueMessage (arMessage.actionType, arMessage.destination);
			EventBroadcaster.Instance.PostEvent (EventNames.ARBluetoothEvents.ON_RECEIVED_MESSAGE);
		} else {
			ConsoleManager.LogMessage ("[CLIENT] Will not enqueue message. Action type is " + arMessage.actionType);
		}
	}

	/// <summary>
	/// The server receives a message from its clients (possibly from itself). This function sends the message to all clients (except the sender).
	/// </summary>
	/// <param name="networkMsg">Network message.</param>
	private void OnServerHandleMessage(NetworkMessage networkMsg) {
		//ConsoleManager.LogMessage ("[SERVER] Received message from " + networkMsg.conn.address + " with message: " + networkMsg.ReadMessage<ARMessage> ().destination);

		ARNetworkMessage arMessage = networkMsg.ReadMessage<ARNetworkMessage> ();

		for (int i = 0; i < NetworkServer.connections.Count; i++) {
			NetworkConnection connection = NetworkServer.connections [i];

			if (connection != null && connection != networkMsg.conn && !this.hasThrownMessage) {
				connection.Send (ARNetworkMessage.messageType, arMessage);
				this.hasThrownMessage = true;
				ConsoleManager.LogMessage ("[NON-LOCAL] Sending position " + arMessage.destination + " to " +connection.address);
			}
		}

		for (int i = 0; i < NetworkServer.localConnections.Count; i++) {
			NetworkConnection connection = NetworkServer.localConnections [i];

			if (connection != null && connection != networkMsg.conn && !this.hasThrownMessage) {
				connection.Send (ARNetworkMessage.messageType, arMessage);
				this.hasThrownMessage = true;
				ConsoleManager.LogMessage ("[LOCAL] Sending position " + arMessage.destination + " to " +connection.address);
			}
		}

		this.hasThrownMessage = false;
	}
}
