using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ARMessage: MessageBase {
	public const short messageType = 12345;

	private Vector3 destination = Vector3.zero;

	public ARMessage() {

	}

	public void SetDestination(Vector3 destination) {
		this.destination = destination;
	}

	public Vector3 GetDestination() {
		return this.destination;
	}
}
