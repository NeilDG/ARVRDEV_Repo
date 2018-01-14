using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHUDScreen : View {

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnMenuButtonClicked() {
		LoadManager.Instance.LoadScene (SceneNames.MAIN_SCENE);
	}
}
