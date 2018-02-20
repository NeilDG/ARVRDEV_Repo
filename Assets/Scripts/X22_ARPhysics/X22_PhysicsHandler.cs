using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X22_PhysicsHandler : MonoBehaviour {
	private const float Y_COORD_THRESHOLD = -1.757f;

	[SerializeField] private List<Transform> fallableObjects;
	[SerializeField] private GameObjectPool capsulePool;
	[SerializeField] private int spawnCount = 5;
	private Vector3[] originCoords;

	private const float SPAWN_DELAY = 0.25f;
	private float ticks = 0.0f;

	// Use this for initialization
	void Start () {
		this.StoreOriginPositions ();
		this.capsulePool.Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		/*for (int i = 0; i < this.fallableObjects.Count; i++) {
			//track and check the position of each object
			if (this.fallableObjects [i].position.y <= Y_COORD_THRESHOLD) {
				this.ResetObjectPosition (i);
			}
		}*/

		this.ticks += Time.deltaTime;
		if (this.ticks >= SPAWN_DELAY) {
			this.ticks = 0.0f;

			//request for poolable objects
			APoolable[] objects = this.capsulePool.RequestPoolableBatch(this.spawnCount);
			if (objects == null) {
				return;
			}

			for (int i = 0; i < objects.Length; i++) {
				//reset position
				objects[i].transform.localPosition = this.transform.localPosition;
			}
		}
	}

	private void StoreOriginPositions() {
		this.originCoords = new Vector3[this.fallableObjects.Count];
		for (int i = 0; i < fallableObjects.Count; i++) {
			this.originCoords [i] = this.fallableObjects [i].localPosition;
		}
	}

	private void ResetObjectPosition(int i) {
		this.fallableObjects [i].localPosition = this.originCoords [i];
		this.fallableObjects [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}
}
