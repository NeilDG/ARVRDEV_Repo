using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class FirstTarget : ImageTargetBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public override void OnTrackerUpdate (Status newStatus)
	{
		base.OnTrackerUpdate (newStatus);
		if (newStatus == Status.TRACKED) {
			EventBroadcaster.Instance.PostEvent (EventNames.ARPhysicsEvents.ON_FIRST_TARGET_SCAN);
		}
	}
}
