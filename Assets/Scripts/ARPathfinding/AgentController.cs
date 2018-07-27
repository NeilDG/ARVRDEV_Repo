using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class AgentController : MonoBehaviour {

    [SerializeField] private AICharacterControl[] aiAgents;

	// Use this for initialization
	void Start () {
        EventBroadcaster.Instance.AddObserver(EventNames.ARPathFindEvents.ON_BEACON_DETECTED, this.OnBeaconDetected);
        EventBroadcaster.Instance.AddObserver(EventNames.ARPathFindEvents.ON_PLATFORM_DETECTED, this.OnPlatformDetected);
        EventBroadcaster.Instance.AddObserver(EventNames.ARPathFindEvents.ON_PLATFORM_HIDDEN, this.OnPlatformHidden);
        this.SetAgentsActive(false);
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ARPathFindEvents.ON_BEACON_DETECTED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ARPathFindEvents.ON_PLATFORM_DETECTED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ARPathFindEvents.ON_PLATFORM_HIDDEN);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnBeaconDetected(Parameters parameters) {
        Transform target = parameters.GetObjectExtra(BeaconTarget.BEACON_TRANSFORM_KEY) as Transform;
        this.MoveAllAgents(target);
    }

    private void OnPlatformDetected() {
        this.SetAgentsActive(true);
        Debug.Log("Platform detected. Agents active.");
    }

    public void OnPlatformHidden() {
        this.SetAgentsActive(false);
        Debug.Log("Platform hidden. Agents hidden.");
    }

    private void MoveAllAgents(Transform target) {
        for(int i = 0; i < this.aiAgents.Length; i++) {
            this.aiAgents[i].SetTarget(target);
            this.aiAgents[i].agent.SetDestination(target.localPosition);
            Debug.Log("Set agents target to position: " + target.position.ToString());
        }
    }

    public void SetAgentsActive(bool active) {
        for(int i = 0; i < this.aiAgents.Length; i++) {
            this.aiAgents[i].gameObject.SetActive(active);
        }
    }
}
