using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X01_ObjectManager : MonoBehaviour {

	private static X01_ObjectManager sharedInstance = null;
	public static X01_ObjectManager Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private GameObject[] objectCopies;
	[SerializeField] private GameObject selectedObject;

	void Awake() {
		sharedInstance = this;
	}

	// Use this for initialization
	void Start () {
		this.selectedObject = this.objectCopies [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelected(int index) {
		this.selectedObject = this.objectCopies [index];
	}

	public GameObject GetSelected() {
		return this.selectedObject;
	}
}
