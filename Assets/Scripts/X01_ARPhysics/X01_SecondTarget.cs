using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X01_SecondTarget : ImageTargetBehaviour{

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
			int pool = 20;
			Parameters parameters = new Parameters ();
			parameters.PutExtra ("POOL_SIZE", pool);
			EventBroadcaster.Instance.PostEvent (EventNames.X01_Events.ON_FINAL_SCAN, parameters);
		}
	}
}
