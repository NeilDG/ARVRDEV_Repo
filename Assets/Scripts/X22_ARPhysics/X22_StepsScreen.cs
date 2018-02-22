using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class X22_StepsScreen : MonoBehaviour {

	[SerializeField] private Text displayText;

	private bool firstScanSuccess = false;
	private bool secondScanSuccess = false;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.X22_Events.ON_FIRST_SCAN, this.OnFirstScan);
		EventBroadcaster.Instance.AddObserver (EventNames.X22_Events.ON_FINAL_SCAN, this.OnSecondScan);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.X22_Events.ON_FIRST_SCAN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.X22_Events.ON_FINAL_SCAN);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnFirstScan() {
		if (!this.firstScanSuccess) {
			this.displayText.text = "SUCCESSFUL! Scan the 2nd target pls!! :D";
			this.firstScanSuccess = true;
		}
	}

	private void OnSecondScan(Parameters parameters) {
		if (!this.secondScanSuccess) {
			int poolSize = parameters.GetIntExtra ("POOL_SIZE", 0);
			this.displayText.text = "There are "+poolSize+" objects! WOOW! :O";
		}
	}
}
