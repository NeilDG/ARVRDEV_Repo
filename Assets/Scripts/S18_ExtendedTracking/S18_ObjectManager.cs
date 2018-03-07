using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_ObjectManager : MonoBehaviour {
	
	private static S18_ObjectManager sharedInstance = null;
	public static S18_ObjectManager Instance {
		get {
			return sharedInstance;
		}
	}

	[SerializeField] private GameObject[] templates;
	[SerializeField] private GameObject selectedTemplate;

	void Awake() {
		sharedInstance = this;
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	// Use this for initialization
	void Start () {
		this.selectedTemplate = this.templates [0]; //set default
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelected(int index) {
		this.selectedTemplate = this.templates [index];
	}

	public GameObject GetSelected() {
		return this.selectedTemplate;
	}
}
