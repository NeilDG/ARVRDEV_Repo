using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualButtonDemo : MonoBehaviour, IVirtualButtonEventHandler {

	private const string VIRTUAL_BUTTON_1 = "toggle_1";
	private const string VIRTUAL_BUTTON_2 = "toggle_2";
	private const string VIRTUAL_BUTTON_3 = "toggle_3";

	[SerializeField] private GameObject[] objects;
	[SerializeField] private VirtualButtonBehaviour[] buttonList;

	private int currentActiveObject = 0;

	// Use this for initialization
	void Start () {
		this.buttonList [0].RegisterEventHandler (this);
		this.buttonList [1].RegisterEventHandler (this);
		this.buttonList [2].RegisterEventHandler (this);
		this.ToggleActiveObject ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnButtonPressed (VirtualButtonBehaviour vb) {
		if (vb.VirtualButtonName == VIRTUAL_BUTTON_1) {
			this.currentActiveObject = 0;
		}
		else if (vb.VirtualButtonName == VIRTUAL_BUTTON_2) {
			this.currentActiveObject = 1;
		}
		else if (vb.VirtualButtonName == VIRTUAL_BUTTON_3) {
			this.currentActiveObject = 2;
		}

		this.ToggleActiveObject ();
	}

	private void ToggleActiveObject() {
		for (int i = 0; i < this.objects.Length; i++) {
			this.objects [i].SetActive (false);
		}

		this.objects [this.currentActiveObject].SetActive (true);
	}

	public void OnButtonReleased (VirtualButtonBehaviour vb) {

	}
}
