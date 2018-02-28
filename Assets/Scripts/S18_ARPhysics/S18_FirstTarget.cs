using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class S18_FirstTarget : ImageTargetBehaviour {

	//NO NO!! Reference is external.
	//[SerializeField] private Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public override void OnTrackerUpdate (Status newStatus) {
		base.OnTrackerUpdate (newStatus);
		if (newStatus == Status.TRACKED) {
			EventBroadcaster.Instance.PostEvent (EventNames.S18_Events.ON_FIRST_SCAN);
		}
	}
}
