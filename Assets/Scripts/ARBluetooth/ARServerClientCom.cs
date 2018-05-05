using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LostPolygon.AndroidBluetoothMultiplayer;

public class ARServerClientCom : NetworkManager {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnStartServer ()
	{
		base.OnStartServer ();
		ConsoleManager.LogMessage ("Server started");

		NetworkServer.RegisterHandler (ARCanvasSpace.ARMessage.messageType, this.OnReceivedClientMessage);
	}

	public override void OnServerReady (NetworkConnection conn)
	{
		base.OnServerReady (conn);
		ConsoleManager.LogMessage ("Server ready: " + conn);
	}

	public override void OnStopServer ()
	{
		base.OnStopServer ();
		ConsoleManager.LogMessage ("Server stopped");
	}

	public override void OnStartClient (NetworkClient client)
	{
		base.OnStartClient (client);
		ConsoleManager.LogMessage ("Client " + client.connection.address + " has successfully started.");

		client.RegisterHandler (ARCanvasSpace.ARMessage.messageType, this.OnHandleClientMessage);
	}

	private void OnHandleClientMessage(NetworkMessage networkMsg) {
		ConsoleManager.LogMessage ("Received message from " + networkMsg.conn.address + " with message: " + networkMsg.ReadMessage<ARCanvasSpace.ARMessage> ().GetDestination ());
	}

	/// <summary>
	/// The server receives a message from its clients (possibly from itself). This function sends the message to all clients (except the sender).
	/// </summary>
	/// <param name="networkMsg">Network message.</param>
	private void OnReceivedClientMessage(NetworkMessage networkMsg) {
		ARCanvasSpace.ARMessage arMessage = networkMsg.ReadMessage<ARCanvasSpace.ARMessage> ();

		for (int i = 0; i < NetworkServer.connections.Count; i++) {
			NetworkConnection connection = NetworkServer.connections [i];

			if (connection != null && connection != networkMsg.conn) {
				connection.Send (ARCanvasSpace.ARMessage.messageType, arMessage);
			}
		}

		for (int i = 0; i < NetworkServer.localConnections.Count; i++) {
			NetworkConnection connection = NetworkServer.connections [i];

			if (connection != null && connection != networkMsg.conn) {
				connection.Send (ARCanvasSpace.ARMessage.messageType, arMessage);
			}
		}
	}
}
