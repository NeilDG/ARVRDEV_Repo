using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VideoTargetRef : ImageTargetBehaviour, ITrackableEventHandler {

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
        if (newStatus == Status.TRACKED) {
            VideoSizeComputer.Instance.OnDetected(this);
        }
    }
}
