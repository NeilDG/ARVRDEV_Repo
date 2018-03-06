using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X01_ObjectPlacer : MonoBehaviour {

	[SerializeField] private Camera arCamera;

	private List<GameObject> placedObjects = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_SHOW_ALL, this.OnShowAllObjects);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_HIDE_ALL, this.OnHideAllObjects);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_DELETE_ALL, this.OnDeleteAllObjects);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_SHOW_ALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_HIDE_ALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_DELETE_ALL);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = this.arCamera.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction, Color.red);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Vector3 hitPos = hit.point;
				Debug.Log ("<b><color=green>Hit position at: " + hit.point+ " </color></b>");
			
				GameObject template = X01_ObjectManager.Instance.GetSelected ();
				GameObject spawnObject = GameObject.Instantiate (template, this.transform);
				spawnObject.transform.position = hitPos;
				spawnObject.SetActive (true);

				this.placedObjects.Add (spawnObject);
			}
		}
	}

	private void OnHideAllObjects() {
		for (int i = 0; i < this.placedObjects.Count; i++) {
			this.placedObjects [i].SetActive (false);
		}
	}

	private void OnShowAllObjects() {
		for (int i = 0; i < this.placedObjects.Count; i++) {
			this.placedObjects [i].SetActive (true);
		}
	}

	private void OnDeleteAllObjects() {
		for (int i = 0; i < this.placedObjects.Count; i++) {
			GameObject.Destroy (this.placedObjects [i]);
		}

		this.placedObjects.Clear ();
	}
}
