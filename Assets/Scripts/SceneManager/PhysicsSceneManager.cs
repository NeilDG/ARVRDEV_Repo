using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Stops the positional device tracker to make physics work effectively.
/// </summary>
public class PhysicsSceneManager : MonoBehaviour {

    private static PhysicsSceneManager sharedInstance = null;
    public PhysicsSceneManager Instance {
        get {
            return sharedInstance;
        }
    }

    private void Awake() {
        sharedInstance = this;

    }
    // Use this for initialization
    void Start () {
        VuforiaARController.Instance.RegisterVuforiaInitializedCallback(this.OnARStart);
    }

    private void OnDestroy() {
        VuforiaARController.Instance.UnregisterVuforiaInitializedCallback(this.OnARStart);
        sharedInstance = null; 
    }

    private void OnARStart() {
       bool success = TrackerManager.Instance.DeinitTracker<PositionalDeviceTracker>();
       Debug.Log("Successfully paused device tracker!");
    }
}
