using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoDebugScreen : View {

    [SerializeField] private Text debugText;

	// Use this for initialization
	void Start () {
        this.debugText.text = "No targets detected. Cannot print distance.";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayDistance(float distance) {
        this.debugText.text = "Target Distance: "+distance;
    }

    public void OnDisjointClicked() {
        EventBroadcaster.Instance.PostEvent(EventNames.VideoAREvents.ON_VIDEO_DISJOINTED);
    }

    public override void OnRootScreenBack() {
        base.OnRootScreenBack();
        TwoChoiceDialog twoChoiceDialog = (TwoChoiceDialog)DialogBuilder.Create(DialogBuilder.DialogType.CHOICE_DIALOG);
        twoChoiceDialog.SetMessage("Go back to main menu?");
        twoChoiceDialog.SetOnConfirmListener(() => {
            LoadManager.Instance.LoadScene(SceneNames.MAIN_SCENE);
        });
    }
}
