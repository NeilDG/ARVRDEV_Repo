using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARController : MonoBehaviour {

	[SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private string clientID;

	private bool moving = false;
	private Vector3 destination;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (this.moving) {
			float step = this.moveSpeed * Time.deltaTime;
			Vector3 origin = this.transform.position;
			this.transform.position = Vector3.MoveTowards (origin, destination, step);

			if(Vector3.Distance(this.transform.position, destination) <= 0.0f) {
				this.moving = false;
			}

		}
	}

	public void MoveToDestination(Vector3 destination) {
		this.moving = true;
		this.destination = destination;
	}

    public void SetClientID(string clientID) {
        this.clientID = clientID;
    }

    public string GetClientID() {
        return this.clientID;
    }
}
