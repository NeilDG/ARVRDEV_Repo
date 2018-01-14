using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : View {

	// Use this for initialization
	void Start () {
		
	}

	public void On3DImageClicked() {
		LoadManager.Instance.LoadScene(SceneNames.ROTATE_OBJECT_SCENE);
	}

	public void OnPhysicsARClicked() {
		LoadManager.Instance.LoadScene (SceneNames.AR_PHYSICS_SCENE);
	}
}
