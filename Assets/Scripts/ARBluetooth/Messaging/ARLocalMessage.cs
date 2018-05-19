using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a local message translated from a network message
/// </summary>
public class ARLocalMessage {
    private string clientID;
	private ARNetworkMessage.ActionType actionType;
	private Vector3 position;

	public ARLocalMessage(string clientID, ARNetworkMessage.ActionType actionType, Vector3 position) {
        this.clientID = clientID;
		this.actionType = actionType;
		this.position = position;
	}

	public ARNetworkMessage.ActionType GetActionType() {
		return this.actionType;
	}

	public Vector3 GetPosition() {
		return this.position;
	}

    public string GetClientID() {
        return this.clientID;
    }
}
