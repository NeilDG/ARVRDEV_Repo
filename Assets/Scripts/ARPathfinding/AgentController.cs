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
        Vector3 target = (Vector3) parameters.GetObjectExtra(BeaconTarget.BEACON_POSITION_KEY);
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

    private void MoveAllAgents(Vector3 target) {
        for(int i = 0; i < this.aiAgents.Length; i++) {
            this.aiAgents[i].SetDestination(target);
            Debug.Log("Set agents target to position: " + target.ToString());

            InfoScreen infoScreen = (InfoScreen) ViewHandler.Instance.FindActiveView(ViewNames.INFO_SCREEN_NAME);
            infoScreen.SetMessage("Set agents target to position: " + target.ToString());
        }
    }

    public void SetAgentsActive(bool active) {
        for(int i = 0; i < this.aiAgents.Length; i++) {
            this.aiAgents[i].gameObject.SetActive(active);
        }
    }
}
