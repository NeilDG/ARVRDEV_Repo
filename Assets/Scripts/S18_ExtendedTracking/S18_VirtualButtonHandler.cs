using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class S18_VirtualButtonHandler : MonoBehaviour, IVirtualButtonEventHandler {

	[SerializeField] private VirtualButtonBehaviour[] virtualButtons;
	[SerializeField] private GameObject[] objectGroups;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < this.virtualButtons.Length; i++) {
			this.virtualButtons [i].RegisterEventHandler (this);
		}

		this.Toggle (0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Toggle(int index) {
		for (int i = 0; i < this.objectGroups.Length; i++) {
			this.objectGroups [i].SetActive (false);
		}

		this.objectGroups [index].SetActive (true);
	}

	public void OnButtonPressed (VirtualButtonBehaviour vb) {
		if (vb == this.virtualButtons [0]) {
			this.Toggle (0);
		}
		if (vb == this.virtualButtons [1]) {
			this.Toggle (1);
		}
		if (vb == this.virtualButtons [2]) {
			this.Toggle (2);
		}
	}

	public void OnButtonReleased (VirtualButtonBehaviour vb) {

	}
}
