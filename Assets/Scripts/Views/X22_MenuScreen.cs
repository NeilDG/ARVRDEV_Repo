using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X22_MenuScreen : View {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayTestDialog() {
		Debug.Log ("Test");
		//display a UI using the view handler system.
		//ViewHandler.Instance.Show(ViewNames.X22_NEW_SCREEN_NAME);

		DialogInterface dialog = DialogBuilder.Create (DialogBuilder.DialogType.NOTIFICATION);
		dialog.SetMessage ("This is GAM-ENG X22!");
		dialog.SetOnDismissListener (() => {
			//anonymous action
			DialogInterface dialog2 = DialogBuilder.Create(DialogBuilder.DialogType.NOTIFICATION);
			dialog2.SetMessage("Another dialog popup!");
		});
	}

	public void LoadARScene() {
		LoadManager.Instance.LoadScene (SceneNames.X22_AR_SCENE);
	}

	public void LoadExtendedScene() {
		LoadManager.Instance.LoadScene (SceneNames.X22_EXTENDED_SCENE);
	}
}
