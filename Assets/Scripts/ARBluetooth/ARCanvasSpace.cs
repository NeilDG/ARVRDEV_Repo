using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Simple script that allows movement of an object through tap.
/// </summary>
public class ARCanvasSpace : MonoBehaviour {
	public const string TAG = "ARCanvasSpace";

	[SerializeField] private Camera arCamera;
	[SerializeField] private ARController player;
	[SerializeField] private ARController opponent;

	private List<ARController> opponents;

	// Use this for initialization
	void Start () {
		//set coordinates
		this.opponent.transform.localPosition = this.player.transform.localPosition;
		EventBroadcaster.Instance.AddObserver (EventNames.ARBluetoothEvents.ON_RECEIVED_MESSAGE, this.OnReceivedMessage);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ARBluetoothEvents.ON_RECEIVED_MESSAGE);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = this.arCamera.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction, Color.red);


			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Vector3 destination = new Vector3 (hit.point.x, this.player.transform.position.y, hit.point.z);

				//move the piece
				this.player.MoveToDestination(destination);

				//send the move message
				ARNetworkMessage arMsg = new ARNetworkMessage ();
				arMsg.destination = destination;
				arMsg.actionType  = ARNetworkMessage.ActionType.MOVE;
				NetworkManager.singleton.client.Send (ARNetworkMessage.messageType, arMsg);
				ConsoleManager.LogMessage ("Attempting to send destination: " + destination);
			}
		}
	}

	private void OnReceivedMessage() {
		//process message here
		ARLocalMessage localMsg = ARMessageQueue.Instance.GetLatestMessage();
		ConsoleManager.LogMessage (TAG + " received message with type of " + localMsg.GetActionType ());

		this.opponent.MoveToDestination (localMsg.GetPosition ());
	}
}
