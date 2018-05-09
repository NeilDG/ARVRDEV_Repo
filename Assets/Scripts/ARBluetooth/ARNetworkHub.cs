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

	void Awake() {
		sharedInstance = this;
	}

	void OnDestroy() {
		sharedInstance = null;
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
			NetworkServer.RegisterHandler (ARMessage.messageType, this.OnReceivedClientMessage);
			ConsoleManager.LogMessage ("Successfully registered server handler");
		}
			
		if (NetworkManager.singleton.client != null) {
			NetworkManager.singleton.client.RegisterHandler (ARMessage.messageType, this.OnHandleClientMessage);
			ConsoleManager.LogMessage ("Client " + NetworkManager.singleton.client.connection.address + " has successfully started.");
		}

		/*NetworkClient[] clients = NetworkClient.allClients.ToArray ();
		for (int i = 0; i < clients.Length; i++) {
			ConsoleManager.LogMessage ("Client " + clients[i].connection.address + " has successfully started.");
			clients[i].RegisterHandler (ARMessage.messageType, this.OnHandleClientMessage);
		}*/

	}

	/*public void SendMessage(short type, MessageBase message) {
		NetworkClient[] clients = NetworkClient.allClients.ToArray ();
		for (int i = 0; i < clients.Length; i++) {
			NetworkServer.SendToClient (clients[i].connection.connectionId,type, message);
		}
	}*/

	public void SendDummyData() {
		// Send the message with the tap position to the server, so it can send it to other clients

		//ConsoleManager.LogMessage ("Attempting to send dummy data ");
		ARMessage arMsg = new ARMessage ();
		arMsg.destination = new Vector3 (5.0f, 5.0f, 5.0f);
		NetworkManager.singleton.client.Send(ARMessage.messageType, new ARMessage(){destination = new Vector3(5.0f, 5.0f, 5.0f)});
		//NetworkServer.SendToAll (ARMessage.messageType, arMsg);
		//this.SendMessage(ARMessage.messageType, arMsg);
	}

	private void OnHandleClientMessage(NetworkMessage networkMsg) {
		ConsoleManager.LogMessage ("[CLIENT] Received message from " + networkMsg.conn.address + " with message: " + networkMsg.ReadMessage<ARMessage> ().destination);
	}

	/// <summary>
	/// The server receives a message from its clients (possibly from itself). This function sends the message to all clients (except the sender).
	/// </summary>
	/// <param name="networkMsg">Network message.</param>
	private void OnReceivedClientMessage(NetworkMessage networkMsg) {
		//ConsoleManager.LogMessage ("[SERVER] Received message from " + networkMsg.conn.address + " with message: " + networkMsg.ReadMessage<ARMessage> ().destination);

		ARMessage arMessage = networkMsg.ReadMessage<ARMessage> ();

		for (int i = 0; i < NetworkServer.connections.Count; i++) {
			NetworkConnection connection = NetworkServer.connections [i];

			if (connection != null && connection != networkMsg.conn) {
				connection.Send (ARMessage.messageType, arMessage);
				ConsoleManager.LogMessage ("[NON-LOCAL] Sending position " + arMessage.destination + " to " +connection.address);
			}
		}

		for (int i = 0; i < NetworkServer.localConnections.Count; i++) {
			NetworkConnection connection = NetworkServer.connections [i];

			if (connection != null && connection != networkMsg.conn) {
				connection.Send (ARMessage.messageType, arMessage);
				ConsoleManager.LogMessage ("[LOCAL] Sending position " + arMessage.destination + " to " +connection.address);
			}
		}
	}
}
