using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsScreen : MonoBehaviour {

	[SerializeField] private Text displayText;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ARPhysicsEvents.ON_FIRST_TARGET_SCAN, this.OnFirstScan);
		EventBroadcaster.Instance.AddObserver (EventNames.ARPhysicsEvents.ON_FINAL_TARGET_SCAN, this.OnFinalScan);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ARPhysicsEvents.ON_FIRST_TARGET_SCAN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ARPhysicsEvents.ON_FINAL_TARGET_SCAN);
	}

	public void SetMessage(string message) {
		this.displayText.text = message;
	}

	private void OnFirstScan() {
		this.SetMessage ("Success! Step 2: Scan the 'Stones' target to show the AR physics demo.");
	}

	private void OnFinalScan() {
		this.SetMessage ("Success!");
		this.StartCoroutine (this.DelayHide (1.5f));
	}

	private IEnumerator DelayHide(float seconds) {
		yield return new WaitForSeconds (seconds);
		GameObject.Destroy (this.gameObject);
	}

}
