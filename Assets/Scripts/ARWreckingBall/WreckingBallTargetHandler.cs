using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class WreckingBallTargetHandler : ImageTargetBehaviour {

    private GameObject playContainer;

	// Use this for initialization
	void Start () {
        this.playContainer = this.transform.Find("Container").gameObject;
        this.playContainer.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnTrackerUpdate(Status newStatus) {
        base.OnTrackerUpdate(newStatus);
        if (newStatus == Status.TRACKED) {
            this.playContainer.SetActive(true);
        }
        else if(newStatus == Status.NO_POSE) {
            this.playContainer.SetActive(false);
        }
    }


}
