using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the proper spawning of the physics box container. The game object parent needs to instantiate first  after detecting the 
/// image target, before instantiating the physics box.
/// 
/// By: NeilDG
/// </summary>
public class PhysicsBoxSpawner : MonoBehaviour {

	[SerializeField] private GameObjectPool capsulePool;
	[SerializeField] private Transform spawnPlace;

	private const float SPAWN_DELAY = 0.25f;
	private float ticks = 0.0f;

	private float LOWER_Z = -0.062f;
	private float UPPER_Z = -0.3f;

	// Use this for initialization
	void Start () {
		this.capsulePool.Initialize ();

	}

	// Update is called once per frame
	void Update () {
		this.ticks += Time.deltaTime;

		if (this.ticks >= SPAWN_DELAY) {
			this.ticks = 0.0f;

			int numObjects = Random.Range (1, 10);

			if (this.capsulePool.HasObjectAvailable (numObjects)) {
				APoolable[] poolableObjects = this.capsulePool.RequestPoolableBatch (numObjects);

				for (int i = 0; i < poolableObjects.Length; i++) {
					Vector3 localPos = this.spawnPlace.localPosition;
					localPos.z = Random.Range (LOWER_Z, UPPER_Z);
					poolableObjects [i].transform.localPosition = localPos;
				}
			}

		}
	}
}
