using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SecondTarget : ImageTargetBehaviour, ITrackableEventHandler {

    // Use this for initialization

	void Start () {
        this.RegisterTrackableEventHandler(this);
	}

    private void OnDestroy() {
        this.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED) {
            EventBroadcaster.Instance.PostEvent(EventNames.ARPhysicsEvents.ON_FINAL_TARGET_SCAN);
        }
    }
}
