using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class AgentController : MonoBehaviour {

    [SerializeField] private AICharacterControl[] aiAgents;

	// Use this for initialization
	void Start () {
        EventBroadcaster.Instance.AddObserver(EventNames.ARPathFindEvents.ON_BEACON_DETECTED, this.OnBeaconDetected);
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ARPathFindEvents.ON_BEACON_DETECTED); 
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnBeaconDetected(Parameters parameters) {
        Transform target = parameters.GetObjectExtra(BeaconTarget.BEACON_TRANSFORM_KEY) as Transform;
        this.MoveAllAgents(target);
    }

    private void MoveAllAgents(Transform target) {
        for(int i = 0; i < this.aiAgents.Length; i++) {
            this.aiAgents[i].SetTarget(target);
            Debug.Log("Set agents target to position: " + target.position.ToString());
        }
    }
}
