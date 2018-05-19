using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Simple script that allows movement of an object through tap.
/// </summary>
public class ARCanvasSpace : MonoBehaviour {
	public const string TAG = "[ARCanvasSpace]";

	[SerializeField] private Camera arCamera;
	[SerializeField] private ARController player;
	[SerializeField] private ARController opponentCopy;

    private bool firstRun = false;

	private Dictionary<string, ARController> opponents = new Dictionary<string, ARController>();

	// Use this for initialization
	void Start () {
        //set coordinates
        this.opponentCopy.gameObject.SetActive(false);
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
                ARNetworkHub.Instance.SendPosition(destination);
			}
		}
	}

    /// <summary>
    /// Handles initial messaages received upon starting the game. This should contains spawn requests
    /// </summary>
    private void HandleInitialMessages() {
        ARLocalMessage[] messages = ARMessageQueue.Instance.GetAllMessages();

        for(int i = 0; i < messages.Length; i++) {
            if(messages[i].GetActionType() == ARNetworkMessage.ActionType.SPAWN_OBJECT) {
                ConsoleManager.LogMessage(TAG + " Spawning opponent for " + messages[i].GetClientID());
            }
        }

    }

	private void OnReceivedMessage() {
        if(this.firstRun == false) {
            this.player.SetClientID(ARNetworkHub.Instance.GetClientID());
            this.firstRun = true;
        }

		//process message here
		ARLocalMessage localMsg = ARMessageQueue.Instance.GetLatestMessage();
        //ConsoleManager.LogMessage (TAG + " received message with type of " + localMsg.GetActionType ());
        if (localMsg.GetActionType() == ARNetworkMessage.ActionType.SPAWN_OBJECT) {
            ConsoleManager.LogMessage(TAG + " Spawning opponent for " + localMsg.GetClientID());
            ARController opponent = GameObject.Instantiate<ARController>(this.opponentCopy, this.opponentCopy.transform.parent);
            opponent.gameObject.transform.position = this.player.transform.position;
            opponent.gameObject.SetActive(true);
            opponent.SetClientID(localMsg.GetClientID());
            this.opponents.Add(localMsg.GetClientID(), opponent);

        }
        else {
            this.opponents[localMsg.GetClientID()].MoveToDestination(localMsg.GetPosition());
            //this.opponent.MoveToDestination(localMsg.GetPosition());
        } 
	}
}
