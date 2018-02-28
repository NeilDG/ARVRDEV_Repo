using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S18_StepsScreen : View {
	
	[SerializeField] private Text myText;

	private bool firstScan = false;
	private bool secondScan = false;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.S18_Events.ON_FIRST_SCAN, this.OnFirstTargetScan);
		EventBroadcaster.Instance.AddObserver (EventNames.S18_Events.ON_FINAL_SCAN, this.OnSecondTargetScan);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.S18_Events.ON_FIRST_SCAN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.S18_Events.ON_FINAL_SCAN);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnFirstTargetScan() {
		if (this.firstScan == false) {
			myText.text = "Please scan the 2nd image target!! :D";
			this.firstScan = true;
		}

	}

	private void OnSecondTargetScan(Parameters parameters) {
		if (this.secondScan == false) {
			this.secondScan = true;

			int number = parameters.GetIntExtra("OBJECTS_SPAWNED", 0);
			myText.text = "SUCCESS!! YAY!! There are " +number+ " objects spawned! :D";

			this.StartCoroutine (this.DelayHideUI (parameters.GetFloatExtra(S18_SecondTarget.DELAY_UI_KEY, 1.5f)));
		}
	}

	//create delay to hide the UI
	private IEnumerator DelayHideUI(float seconds) {
		yield return new WaitForSeconds (seconds);

		this.gameObject.SetActive (false);
	}
}
