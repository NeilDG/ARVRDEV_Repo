using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the logic for placing virtual objects through screen tap.
/// By: NeilDG
/// </summary>
public class ObjectSpace : MonoBehaviour {

	[SerializeField] private Camera arCamera;
	[SerializeField] private GameObject[] placeableObjectsCopy;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < this.placeableObjectsCopy.Length; i++) {
			this.placeableObjectsCopy [i].SetActive (false);
		}
	}

	void OnDestroy() {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = this.arCamera.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction, Color.red);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Vector3 hitPos = hit.point;
				Debug.Log ("Hit pos: " + hitPos);

				GameObject spawnObject = GameObject.Instantiate (this.placeableObjectsCopy [0], this.transform);
				spawnObject.transform.position = hitPos;
				spawnObject.SetActive (true);
			}

		}
	}
}
