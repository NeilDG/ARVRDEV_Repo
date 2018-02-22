using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X22_SecondTarget : ImageTargetBehaviour {

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
			int poolSize = 20;
			Parameters parameters = new Parameters ();
			parameters.PutExtra ("POOL_SIZE", poolSize);
			parameters.PutObjectExtra ("GAME_OBJECT", this.gameObject);
			EventBroadcaster.Instance.PostEvent (EventNames.X22_Events.ON_FINAL_SCAN, parameters);
		}
	}
}
