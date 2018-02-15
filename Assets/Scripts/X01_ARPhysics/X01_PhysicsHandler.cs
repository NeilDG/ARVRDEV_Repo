using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X01_PhysicsHandler : MonoBehaviour {

	private const float Y_COORD_THRESHOLD = -3.66f;
	[SerializeField] private List<Transform> fallableObjects;

	private Vector3[] originPositions;

	// Use this for initialization
	void Start () {
		this.originPositions = new Vector3[this.fallableObjects.Count];
		this.GetOriginPositions ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		for (int i = 0; i < this.fallableObjects.Count; i++) {
			if (this.fallableObjects [i].localPosition.y <= Y_COORD_THRESHOLD) {
				this.ResetObject (i);
			}
		}
	}

	private void GetOriginPositions() {
		for (int i = 0; i < this.fallableObjects.Count; i++) {
			this.originPositions [i] = this.fallableObjects [i].position;
		}
	}

	private void ResetObject(int i) {
		this.fallableObjects [i].position = this.originPositions [i];
		this.fallableObjects [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}
}
