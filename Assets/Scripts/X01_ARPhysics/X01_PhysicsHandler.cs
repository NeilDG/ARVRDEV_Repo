using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X01_PhysicsHandler : MonoBehaviour {
	private const float Y_COORD_THRESHOLD = -1.757f;

	[SerializeField] private List<Transform> fallableObjects;
	[SerializeField] private GameObjectPool capsulePool;
	[SerializeField] private Transform spawnPlace;
	[SerializeField] private int spawnObjects = 5;

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
		for (int i = 0; i < this.fallableObjects.Count; i++) {
			//track and check the position of each object
			if (this.fallableObjects [i].position.y <= Y_COORD_THRESHOLD) {
				this.ResetObjectPosition (i);
			}
		}

		this.ticks += Time.deltaTime;
		if(this.ticks >= SPAWN_DELAY) {
			this.ticks = 0.0f;

			Debug.Log ("Requesting poolable");
			APoolable[] objectList = this.capsulePool.RequestPoolableBatch (this.spawnObjects);

			for (int i = 0; i < objectList.Length; i++) {
				objectList [i].transform.position = this.spawnPlace.position;
			}
		}
	}

	private void StoreOriginPositions() {
		this.originCoords = new Vector3[this.fallableObjects.Count];
		for (int i = 0; i < fallableObjects.Count; i++) {
			this.originCoords [i] = this.fallableObjects [i].position;
		}
	}

	private void ResetObjectPosition(int i) {
		this.fallableObjects [i].position = this.originCoords [i];
		this.fallableObjects [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}
}
