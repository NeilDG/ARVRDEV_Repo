using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectPlaceScreen : MonoBehaviour {

	[SerializeField] private Text selectedText;

	private int eventFires = 0;

	private bool showToggle = true;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ExtendTrackEvents.ON_TARGET_SCAN, this.OnTargetScan);
		this.gameObject.SetActive (false);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ExtendTrackEvents.ON_TARGET_SCAN);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTargetScan() {
		this.gameObject.SetActive(true);
	}

	public void OnSelectedButton(int buildingID) {
		this.selectedText.text = "Selected: Building " +buildingID;
		ObjectPlacerManager.Instance.SetSelected (buildingID);
	}

	public void OnShowHideClicked() {
		this.showToggle = !this.showToggle;

		if (this.showToggle) {
			EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_SHOW_ALL);
		} else {
			EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_HIDE_ALL);
		}
	}

	public void OnDeleteAll() {
		this.showToggle = true;
		EventBroadcaster.Instance.PostEvent (EventNames.ExtendTrackEvents.ON_DELETE_ALL);
	}


}
