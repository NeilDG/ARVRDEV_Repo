using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class WreckingBallTargetHandler : ImageTargetBehaviour, ITrackableEventHandler {

    private WreckingBallPlacer wreckingBallPlacer;

	// Use this for initialization
	void Start () {
        this.wreckingBallPlacer = this.transform.Find("WBPlatform").GetComponent<WreckingBallPlacer>();
        this.wreckingBallPlacer.gameObject.SetActive(false);

        this.RegisterTrackableEventHandler(this);
    }

    private void OnDestroy() {
        this.UnregisterTrackableEventHandler(this);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnTrackableStateChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED) {
            this.wreckingBallPlacer.gameObject.SetActive(true);
            this.wreckingBallPlacer.PlotWreckingBall();
        }
        else if (newStatus == Status.NO_POSE) {
            this.wreckingBallPlacer.gameObject.SetActive(false);
            this.wreckingBallPlacer.MarkTargetLost();
        }
    }
}
