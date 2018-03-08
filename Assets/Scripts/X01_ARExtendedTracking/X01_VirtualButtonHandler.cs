using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X01_VirtualButtonHandler : MonoBehaviour, IVirtualButtonEventHandler {

	[SerializeField] private GameObject[] objectGroupList;
	[SerializeField] private VirtualButtonBehaviour[] buttonList;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < this.buttonList.Length; i++) {
			this.buttonList [i].RegisterEventHandler (this);
		}

		this.Toggle (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Toggle(int index) {
		for (int i = 0; i < this.objectGroupList.Length; i++) {
			this.objectGroupList [i].SetActive (false);
		}

		this.objectGroupList [index].SetActive (true);
	}

	public void OnButtonPressed (VirtualButtonBehaviour vb) {
		if (vb == this.buttonList [0]) {
			this.Toggle (0);
		}
		if (vb == this.buttonList [1]) {
			this.Toggle (1);
		}
		if (vb == this.buttonList [2]) {
			this.Toggle (2);
		}
	}

	public void OnButtonReleased (VirtualButtonBehaviour vb) {

	}
}
