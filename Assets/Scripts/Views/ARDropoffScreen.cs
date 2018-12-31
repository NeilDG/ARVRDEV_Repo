using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARDropoffScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnResetClicked() {
        EventBroadcaster.Instance.PostEvent(EventNames.ExtendTrackEvents.ON_RESET_CLICKED);
    }
}
