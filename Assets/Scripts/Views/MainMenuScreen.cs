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

	public void OnExtendedTrackingClicked() {
		LoadManager.Instance.LoadScene (SceneNames.AR_EXTENDED_TRACKING_SCENE);
	}

	public void OnOcclusionClicked() {
		LoadManager.Instance.LoadScene (SceneNames.OCCLUSION_SCENE);
	}

	public void OnVisualEffectsClicked() {
		LoadManager.Instance.LoadScene (SceneNames.VISUAL_EFFECTS_MENU_SCENE);
	}

	public void OnUserDefinedTargetsClicked() {
		LoadManager.Instance.LoadScene (SceneNames.USER_DEFINED_TARGET_SCENE);
	}

	public void OnBluetoothDemoClicked() {
		LoadManager.Instance.LoadScene (SceneNames.BLUETOOTH_AR_SCENE);
	}

    public void OnARVideoPlaybackClicked() {
        LoadManager.Instance.LoadScene(SceneNames.AR_VIDEO_PLAYBACK);
    }

    public void OnARPathfindingClicked() {
        LoadManager.Instance.LoadScene(SceneNames.AR_PATH_FINDING);
    }

    public void OnMoleculeViewerClicked() {
        LoadManager.Instance.LoadScene(SceneNames.AR_MOLECULE_VIEWER);
    }

    public void OnWreckBallClicked() {
        LoadManager.Instance.LoadScene(SceneNames.AR_WRECKING_BALL_SCENE);
    }

    public void OnARBoxClicked() {
        LoadManager.Instance.LoadScene(SceneNames.AR_BOX_SCENE);
    }

	public override void OnRootScreenBack ()
	{
		DialogInterface dialog = DialogBuilder.Create (DialogBuilder.DialogType.CHOICE_DIALOG);
		dialog.SetMessage ("Exit the application?");
		dialog.SetOnConfirmListener (() => {
			Application.Quit();
		});
	}
}
