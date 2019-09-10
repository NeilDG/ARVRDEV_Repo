using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Object placer handler logic using virtual button.
/// By: NeilDG
///HINT: Alt+Insert to implement interface functions
/// </summary>
public class VirtualObjectPlacer : ImageTargetBehaviour, IVirtualButtonEventHandler, ITrackableEventHandler {

	public enum GameState {
		NONE,
		INITIALIZE,
		PLACEMENT,
		VIEW_MODE
	}

	private GameState currentState = GameState.NONE;
	[SerializeField] private GameObject abominationCopy;
	[SerializeField] private VirtualButtonBehaviour buttonPlacer;

	private bool trackedSuccess = false;

    // Use this for initialization
    void Start () {
        this.buttonPlacer.RegisterEventHandler (this);
        this.RegisterTrackableEventHandler(this);
    }

    private void OnDestroy() {
        this.UnregisterTrackableEventHandler(this);
    }

    public void OnButtonPressed (VirtualButtonBehaviour vb)
	{
		Vector2 topLeftPos = Vector2.zero;
		Vector2 bottomRightPos = Vector2.zero;
		//this.abominationCopy.SetActive (true);

		if (vb.CalculateButtonArea (out topLeftPos, out bottomRightPos)) {
			GameObject abomination = GameObject.Instantiate<GameObject> (this.abominationCopy, this.transform);
			abomination.SetActive (true);
			Vector3 placement = this.abominationCopy.transform.localPosition;
			placement.x = topLeftPos.x;
			placement.z = topLeftPos.y;

			abomination.transform.localPosition = placement;

			Debug.Log ("Placed object!");
		} else {
			Debug.LogError ("Calculate button area failed!");
		}
	}

	public void OnButtonReleased (VirtualButtonBehaviour vb)
	{

	}

    public void OnTrackableStateChanged(Status previousStatus, Status newStatus) {
        if (newStatus == Status.TRACKED && this.trackedSuccess == false) {
            EventBroadcaster.Instance.PostEvent(EventNames.ExtendTrackEvents.ON_TARGET_SCAN);
            this.abominationCopy.SetActive(false);
            this.trackedSuccess = true;
        }
    }
}
