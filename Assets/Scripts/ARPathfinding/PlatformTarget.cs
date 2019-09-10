using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlatformTarget : ImageTargetBehaviour, ITrackableEventHandler {

    private bool tracked = false;

    // Use this for initialization
    void Start () {
        this.RegisterTrackableEventHandler(this);
	}

    private void OnDestroy() {
        this.UnregisterTrackableEventHandler(this);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnTrackableStateChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED && !this.tracked) {
            this.tracked = true;
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_PLATFORM_DETECTED);

        }
        else if (newStatus == Status.NO_POSE && this.tracked) {
            this.tracked = false;
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_PLATFORM_HIDDEN);
        }
    }
}
