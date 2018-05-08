using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ARMessage: MessageBase {
	public const short messageType = 12345;
	public Vector3 destination = Vector3.zero; //must make this public. cannot use getter/setter to modify this variable via network.

	public ARMessage() {

	}
}
