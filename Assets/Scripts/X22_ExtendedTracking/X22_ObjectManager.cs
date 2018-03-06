using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X22_ObjectManager : MonoBehaviour {
	private static X22_ObjectManager sharedInstance = null;

	public static X22_ObjectManager Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private GameObject[] spawnList;
	[SerializeField] private GameObject selectedObject;

	void Awake() {
		sharedInstance = this;
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	// Use this for initialization
	void Start () {
		this.selectedObject = spawnList [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelected(int index) {
		this.selectedObject = this.spawnList [index];
	}

	public GameObject GetSelected() {
		return this.selectedObject;
	}
}
