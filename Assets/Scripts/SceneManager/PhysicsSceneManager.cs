using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Stops the positional device tracker to make physics work effectively.
/// </summary>
public class PhysicsSceneManager : MonoBehaviour {

    // Use this for initialization
    void Start () {
        VuforiaARController.Instance.RegisterVuforiaInitializedCallback(this.OnARStart);
    }

    private void OnDestroy() {
        VuforiaARController.Instance.UnregisterVuforiaInitializedCallback(this.OnARStart);
    }

    private void OnARStart() {
       bool success = TrackerManager.Instance.DeinitTracker<PositionalDeviceTracker>();
       Debug.Log("Successfully paused device tracker!");
    }
}
