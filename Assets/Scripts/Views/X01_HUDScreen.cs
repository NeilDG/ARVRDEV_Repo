using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class X01_HUDScreen : MonoBehaviour {

	[SerializeField] private Text displayText;

	private bool targetScanned = false;
	private bool targetHidden = false;

	private bool hidden = false;

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
		this.displayText.text = "Selected: Object " + 1;
		EventBroadcaster.Instance.AddObserver (EventNames.X01_Events.EXTENDED_TRACK_ON_SCAN, this.OnTargetScan);
		EventBroadcaster.Instance.AddObserver (EventNames.X01_Events.EXTENDED_TRACK_REMOVED, this.OnTargetHidden);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.X01_Events.EXTENDED_TRACK_ON_SCAN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.X01_Events.EXTENDED_TRACK_REMOVED);
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
		if (!this.targetHidden && this.targetScanned) {
			this.targetHidden = true;
			this.targetScanned = false;
			this.gameObject.SetActive (false);
		}
	}

	public void OnButtonSpawnClicked(int index) {
		this.displayText.text = "Selected: Object " + (index + 1);
		X01_ObjectManager.Instance.SetSelected (index);
	}

	public void OnToggleHideClicked() {
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
