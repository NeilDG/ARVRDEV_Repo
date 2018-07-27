using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using static Vuforia.TrackableBehaviour;

public class BeaconTarget : MonoBehaviour, ITrackableEventHandler {

    public const string BEACON_POSITION_KEY = "BEACON_POSITION_KEY";
    [SerializeField] private PlatformTarget platformTarget;
    [SerializeField] private Camera arCamera;

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
            Parameters parameters = new Parameters();
            Vector3 trackedPos = this.TranslateBeaconPosition();
            parameters.PutObjectExtra(BEACON_POSITION_KEY, trackedPos);
            EventBroadcaster.Instance.PostEvent(EventNames.ARPathFindEvents.ON_BEACON_DETECTED, parameters);
        }
        else if (newStatus == Status.NO_POSE) {
            this.tracked = false;
        }
    }

    private Vector3 TranslateBeaconPosition() {
        Ray ray = this.arCamera.ScreenPointToRay(this.arCamera.WorldToScreenPoint(this.transform.position));
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(ray, out hit)) {
            hitPos = hit.point;
            Debug.Log("Hit pos: " + hitPos + " at object: " + hit.transform.gameObject.name);
        }

        return hitPos;
    }
}
