using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class X22_AnchorTarget : ImageTargetBehaviour {

	//NO NO!!!
	//[SerializeField] private GameObject stepsScreen;

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
			EventBroadcaster.Instance.PostEvent (EventNames.X22_Events.ON_FIRST_SCAN);
		}
	}
}
