using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X01_AnchorTarget : ImageTargetBehaviour {

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
			Debug.Log ("Anchor target tracked!");
			EventBroadcaster.Instance.PostEvent (EventNames.X01_Events.ON_FIRST_SCAN);
		}
	}
}
