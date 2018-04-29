using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class UDEventHandler : IUserDefinedTargetEventHandler{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnInitialized () {
		Debug.Log ("UDT Event Handler initialized!");
	}

	public void OnFrameQualityChanged (ImageTargetBuilder.FrameQuality frameQuality) {
		
	}



	public void OnNewTrackableSource (TrackableSource trackableSource) {
		
	}

}
