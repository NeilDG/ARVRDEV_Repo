using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BeaconTarget : ImageTargetBehaviour {

    public const string BEACON_TRANSFORM_KEY = "BEACON_TRANSFORM_KEY";

    private bool tracked = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnTrackerUpdate(Status newStatus) {
        base.OnTrackerUpdate(newStatus);

        if(newStatus == Status.TRACKED && !this.tracked) {
            this.tracked = true;
            Parameters parameters = new Parameters();
            parameters.PutObjectExtra(BEACON_TRANSFORM_KEY, this.transform);
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_BEACON_DETECTED, parameters);

        }
        else if(newStatus == Status.NO_POSE) {
            this.tracked = false;
        }
    }
}
