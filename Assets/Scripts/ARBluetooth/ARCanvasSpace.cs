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
				arMsg.SetDestination (this.destination);
				NetworkManager.singleton.client.Send (ARMessage.messageType, arMsg);
				ConsoleManager.LogMessage ("Attempting to send destination: " + destination);
				/*foreach (NetworkClient client in NetworkClient.allClients) {
					client.Send (ARMessage.messageType, arMsg);
					ConsoleManager.LogMessage ("Attempting to send destination: " + destination+ " to " +client.connection.address);
				}*/
				/*if (NetworkManager.singleton.client != null) {
					NetworkManager.singleton.client.Send (ARMessage.messageType, arMsg);
					ConsoleManager.LogMessage ("Attempting to send destination: " + destination);
				} else {
					ConsoleManager.LogMessage ("Cannot send destination because client was not found.");
				}*/


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
