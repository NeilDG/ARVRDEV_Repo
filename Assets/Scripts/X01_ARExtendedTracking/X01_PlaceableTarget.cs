using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X01_PlaceableTarget : ImageTargetBehaviour {

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
			EventBroadcaster.Instance.PostEvent (EventNames.X01_Events.EXTENDED_TRACK_ON_SCAN);
		} else if (newStatus == Status.NOT_FOUND) {
			EventBroadcaster.Instance.PostEvent (EventNames.X01_Events.EXTENDED_TRACK_REMOVED);
		}
	}
}
