using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectPlaceScreen : MonoBehaviour {

	[SerializeField] private Text numScanText;

	private int eventFires = 0;

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
		this.eventFires++;
		this.numScanText.text = "Event Fires: " + this.eventFires;
	}


}
