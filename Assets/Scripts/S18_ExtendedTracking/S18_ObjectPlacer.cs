using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_ObjectPlacer : MonoBehaviour {
	[SerializeField] private Camera arCamera;

	private List<GameObject> spawnedObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_SHOW_ALL, this.OnShowAll);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_HIDE_ALL, this.OnHideAll);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_DELETE_ALL, this.OnDestroyAll);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_SHOW_ALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_HIDE_ALL);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_DELETE_ALL);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = arCamera.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction, Color.red);

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Vector3 hitPos = hit.point;
				Debug.Log ("<b><color=yellow>Hit position at: " + hit.point+ " </color></b>");

				//spawn objects
				GameObject template = S18_ObjectManager.Instance.GetSelected();
				GameObject spawnObject = GameObject.Instantiate (template, this.transform);
				spawnObject.transform.position = hitPos;
				spawnObject.SetActive (true);

				this.spawnedObjects.Add (spawnObject);
			}
		}
	}

	private void OnHideAll() {
		for (int i = 0; i < this.spawnedObjects.Count; i++) {
			this.spawnedObjects [i].SetActive (false);
		}
	}

	private void OnShowAll() {
		for (int i = 0; i < this.spawnedObjects.Count; i++) {
			this.spawnedObjects [i].SetActive (true);
		}
	}

	private void OnDestroyAll() {
		for (int i = 0; i < this.spawnedObjects.Count; i++) {
			GameObject.Destroy (this.spawnedObjects [i]);
		}

		this.spawnedObjects.Clear ();
	}
}
