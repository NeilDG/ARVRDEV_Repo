using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_ObjectPlacer : MonoBehaviour {
	[SerializeField] private Camera arCamera;


	// Use this for initialization
	void Start () {
		
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
			}
		}
	}
}
