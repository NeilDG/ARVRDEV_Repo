using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image3DScene : MonoBehaviour {

	[TextArea][SerializeField] private string displayMsg;

	// Use this for initialization
	void Start () {
		InfoScreen infoScreen = (InfoScreen) ViewHandler.Instance.Show (ViewNames.INFO_SCREEN_NAME);
		infoScreen.SetMessage (this.displayMsg);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
