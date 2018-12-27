using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreenModifier : MonoBehaviour {

	[TextArea][SerializeField] protected string displayMsg;

	// Use this for initialization
	protected virtual void Start () {
		InfoScreen infoScreen = (InfoScreen) ViewHandler.Instance.Show (ViewNames.INFO_SCREEN_NAME);
		infoScreen.SetMessage (this.displayMsg);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
