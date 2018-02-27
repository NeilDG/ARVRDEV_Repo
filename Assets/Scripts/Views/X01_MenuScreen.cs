using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X01_MenuScreen : View {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayTestDialog() {
		Debug.Log ("Test!");
		//create a new screen that complies with the ViewHandler system, then show it!
		DialogInterface dialog = DialogBuilder.Create(DialogBuilder.DialogType.NOTIFICATION);
		dialog.SetMessage ("This is a test dialog");
		dialog.SetOnDismissListener (() => {
			//anonymous action
			DialogInterface addedDialog = DialogBuilder.Create(DialogBuilder.DialogType.NOTIFICATION);
			addedDialog.SetMessage("Dismiss dialog!");
		});
	}

	public void LoadARPhysics() {
		LoadManager.Instance.LoadScene (SceneNames.X01_AR_SCENE);
	}

	public void LoadExtendedTracking() {
		LoadManager.Instance.LoadScene (SceneNames.X01_EXTENDED_SCENE);
	}
}
