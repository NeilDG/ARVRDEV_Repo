using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoDebugScreen : View {

    [SerializeField] private Text debugText;
    [SerializeField] private Text disjoinText;

    private bool disjointed = false;

	// Use this for initialization
	void Start () {
        this.debugText.text = "No targets detected. Cannot print distance.";
        this.disjoinText.text = "Disjoint";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayDistance(float distance) {
        this.debugText.text = "Target Distance: "+distance;
    }

    public void OnDisjointClicked() {

        if(this.disjointed) {
            this.disjointed = false;
            this.disjoinText.text = "DISJOINT";
            EventBroadcaster.Instance.PostEvent(EventNames.VideoAREvents.ON_VIDEO_ANCHORED);
        }
        else {
            this.disjointed = true;
            this.disjoinText.text = "ANCHOR";
            EventBroadcaster.Instance.PostEvent(EventNames.VideoAREvents.ON_VIDEO_DISJOINTED);
        }

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
