using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class S18_PlaceableTarget : ImageTargetBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnTrackerUpdate (Status newStatus)
	{
		base.OnTrackerUpdate (newStatus);
		if (newStatus == Status.TRACKED) {
			EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_TARGET_SCAN);
		} else if (newStatus == Status.NOT_FOUND) {
			EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_TARGET_HIDE);
		}
	}
}
