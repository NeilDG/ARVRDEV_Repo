using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingMenuHUD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMenuClicked() {
		LoadManager.Instance.LoadScene (SceneNames.VISUAL_EFFECTS_MENU_SCENE);
	}
}
