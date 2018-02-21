using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_PhysicsHandler : MonoBehaviour {

	private const float Y_THRESHOLD = -0.78f;

	[SerializeField] private GameObject[] fallableObjects;
	[SerializeField] private GameObjectPool cylinderPool;
	[SerializeField] private int spawnSize = 5;
	private Vector3[] originPositions;
	private const float TIME_DELAY = 0.25f;
	private float ticks = 0.0f;

	// Use this for initialization
	void Start () {
		this.originPositions = new Vector3[this.fallableObjects.Length];
		this.StoreOriginPositions ();

		this.cylinderPool.Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < this.fallableObjects.Length; i++) {
			//check every object if it reaches a certain threshold, then reset its position.
			if (this.fallableObjects [i].transform.localPosition.y <= Y_THRESHOLD) {
				this.ResetToOrigin (i);
			}
		}

		//spawn N objects periodically
		this.ticks += Time.deltaTime;
		if (this.ticks >= TIME_DELAY) {
			this.ticks = 0.0f;

			//do the spawning
			APoolable[] poolableObject = this.cylinderPool.RequestPoolableBatch(this.spawnSize);
			if (poolableObject == null) {
				return;
			}
		}
	}

	private void StoreOriginPositions() {
		for (int i = 0; i < this.fallableObjects.Length; i++) {
			this.originPositions[i] = this.fallableObjects[i].transform.localPosition;
		}
	}

	private void ResetToOrigin(int i) {
		this.fallableObjects [i].transform.localPosition = this.originPositions [i];
		this.fallableObjects [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}
}
