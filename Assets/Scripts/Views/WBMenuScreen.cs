using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A soon-to-be UI screen for the AR Wrecking Ball. So far only handles reset functionality.
/// By: NeilDG
/// </summary>
public class WBMenuScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnResetBtnClicked() {
        EventBroadcaster.Instance.PostEvent(EventNames.ARWreckBallEvents.ON_RESET_CLICKED);
    }
}
