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
	public const string TAG = "[ARNetworkHub]";
	private static ARNetworkHub sharedInstance = null;
	public static ARNetworkHub Instance {
		get {
			return sharedInstance;
		}
	}


	[SerializeField] private ARNetworkManagerHelper bluetoothHelper;

    private string clientID = ""; //uses the MAC address of the bluetooth device. Only contains a value when successfully connected to server.
	private bool isServer = false;
	//private bool hasThrownMessage = false;

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

	public string GetClientID() {
		return this.clientID;
	}

	public void StartScan() {
		AndroidBluetoothMultiplayer.StartDiscovery ();
		ConsoleManager.LogMessage("Started discovery");
	}

    public void RegisterServerEvents() {
        if (NetworkServer.active) {
            NetworkServer.RegisterHandler(ARNetworkMessage.messageType, this.OnServerHandleMessage);
            ConsoleManager.LogMessage("Successfully registered server handler");
        }
        if (NetworkManager.singleton.client != null) {
            NetworkManager.singleton.client.RegisterHandler(ARNetworkMessage.messageType, this.OnHandleClientMessage);
            ConsoleManager.LogMessage("Host " + NetworkManager.singleton.client.ToString() + " has successfully started.");
        }
    }

	public void RegisterClientEvents() {
		if (NetworkManager.singleton.client != null) {
            NetworkManager.singleton.client.RegisterHandler(ARNetworkMessage.messageType, this.OnHandleClientMessage);
            this.clientID = AndroidBluetoothMultiplayer.GetCurrentDevice().Address;
			ConsoleManager.LogMessage ("Client " + this.clientID + " has successfully started.");
		} else {
			ConsoleManager.LogMessage ("Did not do anything. No client found.");
		}
    }

    public void SendPosition(Vector3 position) {
        ARNetworkMessage arMsg = new ARNetworkMessage();
        arMsg.actionType = ARNetworkMessage.ActionType.MOVE;
        arMsg.position = position;
        arMsg.clientID = this.clientID;
        NetworkManager.singleton.client.Send(ARNetworkMessage.messageType, arMsg);
    }

    public void SendPosition(float x, float y, float z) {
        ARNetworkMessage arMsg = new ARNetworkMessage();
        arMsg.actionType = ARNetworkMessage.ActionType.MOVE;
        arMsg.position = new Vector3(x, y, z);
        arMsg.clientID = this.clientID;
        NetworkManager.singleton.client.Send(ARNetworkMessage.messageType, arMsg);
    }

    /// <summary>
    /// Sends a spawn request to the server. The server relays this message to all clients.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void RequestSpawnObject(float x, float y, float z) {
        ARNetworkMessage arMsg = new ARNetworkMessage();
        arMsg.actionType = ARNetworkMessage.ActionType.SPAWN_OBJECT;
        arMsg.position = new Vector3(x, y, z);
        arMsg.clientID = this.clientID;
        NetworkManager.singleton.client.Send(ARNetworkMessage.messageType, arMsg);
    }

    /// <summary>
    /// Sends a spawn request to the server. The server relays this message to all clients.
    /// </summary>
    public void RequestSpawnObject() {
        ARNetworkMessage arMsg = new ARNetworkMessage();
        arMsg.actionType = ARNetworkMessage.ActionType.SPAWN_OBJECT;
        arMsg.position = Vector3.zero;
        arMsg.clientID = this.clientID;
        NetworkManager.singleton.client.Send(ARNetworkMessage.messageType, arMsg);
    }

	public void SendDummyData() {
        // Send the message with the tap position to the server, so it can send it to other clients
        /*ARNetworkMessage arMsg = new ARNetworkMessage ();
		arMsg.position = new Vector3 (5.0f, 5.0f, 5.0f);
		arMsg.actionType = ARNetworkMessage.ActionType.TEST_DATA;
        arMsg.clientID = this.clientID;
		NetworkManager.singleton.client.Send(ARNetworkMessage.messageType, arMsg);*/
        this.SendPosition(5.0f, 5.0f, 5.0f);
    }

	private void OnHandleClientMessage(NetworkMessage networkMsg) {
		ARNetworkMessage arMessage = networkMsg.ReadMessage<ARNetworkMessage> ();
        if (arMessage.actionType != ARNetworkMessage.ActionType.TEST_DATA) {
			ARMessageQueue.Instance.EnqueueMessage (arMessage.clientID, arMessage.actionType, arMessage.position);
			EventBroadcaster.Instance.PostEvent (EventNames.ARBluetoothEvents.ON_RECEIVED_MESSAGE);
		}
        else {
			ConsoleManager.LogMessage ("[CLIENT] Will not enqueue message. Action type is " + arMessage.actionType);
		}
	}

	/// <summary>
	/// The server receives a message from its clients (possibly from itself). This function sends the message to all clients (except the sender).
	/// </summary>
	/// <param name="networkMsg">Network message.</param>
	private void OnServerHandleMessage(NetworkMessage networkMsg) {
		//ConsoleManager.LogMessage ("[SERVER] Received message from " + networkMsg.conn.address + " with message: " + networkMsg.ReadMessage<ARMessage> ().position);

		ARNetworkMessage arMessage = networkMsg.ReadMessage<ARNetworkMessage> ();

		for (int i = 0; i < NetworkServer.connections.Count; i++) {
			NetworkConnection connection = NetworkServer.connections [i];

			if (connection != null && connection != networkMsg.conn) {
				connection.Send (ARNetworkMessage.messageType, arMessage);
				ConsoleManager.LogMessage ("[NON-LOCAL] Sending message " + arMessage + " to " +connection.address);
			}
		}

		//this.hasThrownMessage = true;

		for (int i = 0; i < NetworkServer.localConnections.Count; i++) {
			NetworkConnection connection = NetworkServer.localConnections [i];

			if (connection != null && connection != networkMsg.conn) {
				connection.Send (ARNetworkMessage.messageType, arMessage);
				//this.hasThrownMessage = true;
				ConsoleManager.LogMessage ("[LOCAL] Sending message " + arMessage + " to " +connection.address);
			}
		}

		//this.hasThrownMessage = false;
	}
}
