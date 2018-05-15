using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Queues all the network messages received from the bluetooth network hub.
/// By: NEILDG
/// </summary>
public class ARMessageQueue {
	public const string TAG = "[ARMessageQueue]";
	private static ARMessageQueue sharedInstance = null;

	public static ARMessageQueue Instance {
		get {
			if (sharedInstance == null) {
				Debug.LogError (TAG + " AR Message Queue not instantiated!");
			}
			return sharedInstance;
		}
	}

	private Queue<ARLocalMessage> messageQueue = new Queue<ARLocalMessage>();
	private int clientCount = 0; //counter for counting clients connected in the scene.

	private ARMessageQueue() {
			
	}

	public static void Initialize() {
		sharedInstance = new ARMessageQueue ();
	}

	public static void Destroy() {
		sharedInstance.messageQueue.Clear ();
		sharedInstance = null;
	}

	/// <summary>
	/// Enqueues a message. The ARMessage is translated to an ARLocalMessage instance to avoid referencing issues
	/// </summary>
	/// <param name="actionType">Action type.</param>
	/// <param name="position">Position.</param>
	public void EnqueueMessage(string clientID, ARNetworkMessage.ActionType actionType, Vector3 position) {
		ARLocalMessage arMsg = new ARLocalMessage (clientID,actionType, position);
		this.messageQueue.Enqueue (arMsg);

		ConsoleManager.LogMessage (TAG + " successfully enqueued message of type " +arMsg.GetActionType());
	}

	/// <summary>
	/// Returns true if there is atleast one message in the queue.
	/// </summary>
	/// <returns><c>true</c>, if messages was containsed, <c>false</c> otherwise.</returns>
	public bool ContainsMessages() {
		return (this.messageQueue.Count > 0);
	}

	/// <summary>
	/// Gets the latest message, assuming there is at least one message in the queue. Also dequeues from the list
	/// </summary>
	/// <returns>The latest message.</returns>
	public ARLocalMessage GetLatestMessage() {
		if (this.ContainsMessages ()) {
			return this.messageQueue.Dequeue ();
		} else {
			Debug.LogError ("[ARMessageQueue] Message queue is empty!");
			return null;
		}
	}

	public ARLocalMessage[] GetLatestMessages(int size) {

		List<ARLocalMessage> arMsgList = new List<ARLocalMessage> ();
		for (int i = 0; i < size; i++) {
			if (this.ContainsMessages ()) {
				arMsgList.Add (this.GetLatestMessage ());
			}
		}

		return arMsgList.ToArray ();
	}
}
