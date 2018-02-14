using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_MainMenuScreen : View {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayTestDialog() {
		//display the other screen using the ViewHandler
		ViewHandler.Instance.Show(ViewNames.S18_NEW_SCREEN_NAME);
	}

	public void LoadARPhysics() {
		LoadManager.Instance.LoadScene (SceneNames.S18_AR_SCENE);
	}
}
