using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_HudScreen : MonoBehaviour {

	private bool hidden = false;

	// Use this for initialization
	void Start () {
		this.OnHideUI ();

		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_TARGET_SCAN, this.OnShowUI);
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_TARGET_HIDE, this.OnHideUI);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_TARGET_SCAN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_TARGET_HIDE);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnShowUI() {
		if (!this.gameObject.activeInHierarchy) { 
			this.gameObject.SetActive (true);
		}
	}

	private void OnHideUI() {
		if (this.gameObject.activeInHierarchy) {
			this.gameObject.SetActive (false);
		}
	}

	public void OnButtonClicked(int index) {
		S18_ObjectManager.Instance.SetSelected (index);
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

	public void OnDestroyClicked() {
		EventBroadcaster.Instance.PostEvent(EventNames.ExtendTrackEvents.ON_DELETE_ALL);
	}
}
