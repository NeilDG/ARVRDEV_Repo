using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X22_HudScreen : MonoBehaviour {

	private bool targetScanned = false;
	private bool targetHidden = false;

	private bool hidden = false;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
		EventBroadcaster.Instance.AddObserver (EventNames.X22_Events.EXTENDED_TRACK_ON_SCAN, this.OnTargetScan);
		EventBroadcaster.Instance.AddObserver (EventNames.X22_Events.EXTENDED_TRACK_REMOVED, this.OnTargetHidden);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.X22_Events.EXTENDED_TRACK_ON_SCAN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.X22_Events.EXTENDED_TRACK_REMOVED);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTargetScan() {
		if (!this.targetScanned) {
			this.targetScanned = true;
			this.targetHidden = false;
			this.gameObject.SetActive (true);
		}
	}

	private void OnTargetHidden() {
		if (!this.targetHidden && this.targetScanned == true) {
			Debug.Log ("Target hidden: " + this.targetHidden);
			this.targetHidden = true;
			this.targetScanned = false;
			this.gameObject.SetActive (false);
		}
	}

	public void OnButtonClicked(int index) {
		X22_ObjectManager.Instance.SetSelected (index);
	}

	public void OnToggleClicked() {
		if (this.hidden) {
			this.hidden = false;
			EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_SHOW_ALL);
		} else {
			this.hidden = true;
			EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_HIDE_ALL);
		}
	}

	public void OnDeleteClicked() {
		EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_DELETE_ALL);
	}
}
