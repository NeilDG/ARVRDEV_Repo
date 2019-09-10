using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class UserDefinedTargetBehavior : ImageTargetBehaviour, ITrackableEventHandler {

    private ObjectSpace objectSpace;
    private bool trackedSuccess = false;

    // Use this for initialization
    void Start () {
        this.objectSpace = this.transform.GetComponentInChildren<ObjectSpace>();
        this.RegisterTrackableEventHandler(this);
    }

    private void OnDestroy() {
        this.UnregisterTrackableEventHandler(this);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnTrackableStateChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED && this.trackedSuccess == false) {
            EventBroadcaster.Instance.PostEvent(EventNames.ExtendTrackEvents.ON_TARGET_SCAN);
            this.trackedSuccess = true;
            this.objectSpace.FacetoCamera();
        }
        else if (newStatus == Status.NO_POSE && this.trackedSuccess) {
            this.trackedSuccess = false;
            Debug.Log("[UserDefinedTargetBehavior] Target lost/rescanned.");
        }
    }
}
