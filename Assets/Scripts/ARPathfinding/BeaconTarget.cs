using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using static Vuforia.TrackableBehaviour;

public class BeaconTarget : MonoBehaviour, ITrackableEventHandler {

    public const string BEACON_POSITION_KEY = "BEACON_POSITION_KEY";
    [SerializeField] private Transform beacon;
    [SerializeField] private PlatformTarget platformTarget;

    private bool tracked = true;
    private TrackableBehaviour mTrackableBehaviour;



    // Use this for initialization
    void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    private void OnDestroy() {
        this.mTrackableBehaviour.UnregisterTrackableEventHandler(this);
        mTrackableBehaviour = null;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
        if (newStatus == Status.TRACKED) {
            this.tracked = true;
            //transfer beacon to platform target parent to get local position
            this.beacon.SetParent(this.platformTarget.transform);

            Parameters parameters = new Parameters();
            Vector3 trackedPos = new Vector3(this.beacon.localPosition.x, this.beacon.localPosition.y, this.beacon.localPosition.z);
            parameters.PutObjectExtra(BEACON_POSITION_KEY, trackedPos);
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_BEACON_DETECTED, parameters);

            this.beacon.SetParent(this.transform); //put the beacon back to its original parent.

        }
        else if (newStatus == Status.NO_POSE) {
            this.tracked = false;
        }
    }
}
