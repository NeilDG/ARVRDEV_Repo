using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class S18_SecondTarget : ImageTargetBehaviour {

	public const string OBJECTS_SPAWNED_KEY = "OBJECTS_SPAWNED_KEY";
	public const string DELAY_UI_KEY = "DELAY_UI_KEY";

	[SerializeField] private float delayUI = 1.5f;

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
			Parameters parameters = new Parameters ();
			parameters.PutExtra (OBJECTS_SPAWNED_KEY, 20);
			parameters.PutExtra (DELAY_UI_KEY, this.delayUI);
			EventBroadcaster.Instance.PostEvent (EventNames.S18_Events.ON_FINAL_SCAN, parameters);
		}
	}
}
