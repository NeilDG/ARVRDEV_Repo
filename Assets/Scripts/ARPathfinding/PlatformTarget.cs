using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlatformTarget : ImageTargetBehaviour {

    private bool tracked = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnTrackerUpdate(Status newStatus) {
        base.OnTrackerUpdate(newStatus);

        if (newStatus == Status.TRACKED && !this.tracked) {
            this.tracked = true;
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_PLATFORM_DETECTED);

        }
        else if (newStatus == Status.NO_POSE) {
            this.tracked = false;
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_PLATFORM_HIDDEN);
        }
    }
}
