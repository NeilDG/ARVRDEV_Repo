using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : View {

	[SerializeField] private Text displayText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMessage(string msgDisplay) {
		this.displayText.text = msgDisplay;
	}
    
    /// <summary>
    /// For camera control settings
    /// </summary>
    public void OnSettingsClicked() {
        ViewHandler.Instance.Show(ViewNames.CAMERA_SETTINGS_SCREEN_NAME);
    }
		
	public override void OnRootScreenBack ()
	{
		base.OnRootScreenBack ();
		TwoChoiceDialog twoChoiceDialog = (TwoChoiceDialog)DialogBuilder.Create (DialogBuilder.DialogType.CHOICE_DIALOG);
		twoChoiceDialog.SetMessage ("Go back to main menu?");
		twoChoiceDialog.SetOnConfirmListener (() => {
			LoadManager.Instance.LoadScene(SceneNames.MAIN_SCENE);
		});
	}
}
