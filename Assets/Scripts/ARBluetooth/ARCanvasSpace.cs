using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Simple script that allows movement of an object through tap.
/// </summary>
public class ARCanvasSpace : MonoBehaviour {

	[SerializeField] private Camera arCamera;
	[SerializeField] private float moveSpeed = 1.0f;
	[SerializeField] private GameObject moveableObject;

	private bool moving = false;
	private Vector3 destination;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = this.arCamera.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction, Color.red);


			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				this.destination = new Vector3 (hit.point.x, moveableObject.transform.position.y, hit.point.z);

				ARMessage arMsg = new ARMessage ();
				arMsg.destination = this.destination;
				//NetworkServer.SendToAll (ARMessage.messageType, arMsg);
				//ARNetworkHub.Instance.SendMessage(ARMessage.messageType, arMsg);
				ConsoleManager.LogMessage ("Attempting to send destination: " + destination);
				this.moving = true;
			}
		}

		if (this.moving) {
			float step = this.moveSpeed * Time.deltaTime;
			Vector3 origin = this.moveableObject.transform.position;
			this.moveableObject.transform.position = Vector3.MoveTowards (origin, destination, step);

			if(Vector3.Distance(this.moveableObject.transform.position, destination) <= 0.0f) {
				this.moving = false;
			}

		}
	}
}
