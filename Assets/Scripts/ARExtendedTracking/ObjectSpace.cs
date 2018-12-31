﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the logic for placing virtual objects through screen tap.
/// By: NeilDG
/// </summary>
public class ObjectSpace : MonoBehaviour {

	[SerializeField] private Camera arCamera;

	private List<GameObject> placedObjects;

	// Use this for initialization
	void Start () {
		//TEST for object selection here
		//ObjectPlacerManager.Instance.SetSelected (5);

		this.placedObjects = new List<GameObject> ();

		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_HIDE_ALL, this.OnHideAllObjects);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_SHOW_ALL, this.OnShowAllObjects);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_DELETE_ALL, this.OnDeleteAllObjects);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_HIDE_ALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_SHOW_ALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_DELETE_ALL);
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

				GameObject spawnObject = GameObject.Instantiate (ObjectPlacerManager.Instance.GetObjectByID(), this.transform);
				spawnObject.transform.position = hitPos;
				spawnObject.SetActive (true);

				this.placedObjects.Add (spawnObject);
			}
		}
	}

	private void OnShowAllObjects() {
		if (this.placedObjects.Count == 0) {
			return;
		}

		for(int i = 0; i < this.placedObjects.Count; i++) {
			this.placedObjects [i].SetActive (true);
		}
	}

	private void OnHideAllObjects() {
		if (this.placedObjects.Count == 0) {
			return;
		}

		for(int i = 0; i < this.placedObjects.Count; i++) {
			this.placedObjects [i].SetActive (false);
		}
	}

	private void OnDeleteAllObjects() {
		for(int i = 0; i < this.placedObjects.Count; i++) {
			GameObject.Destroy (this.placedObjects [i]);
		}

		this.placedObjects.Clear ();
	}
}
