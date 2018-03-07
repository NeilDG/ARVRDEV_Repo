using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_HudScreen : MonoBehaviour {

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
}
