using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour {

	private static ConsoleManager sharedInstance = null;
	/*public static ConsoleManager Instance {
		get {
			return sharedInstance;
		}
	}*/

	[SerializeField] private Text consoleTextCopy;

	void Awake() {
		sharedInstance = this;
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	// Use this for initialization
	void Start () {
		this.consoleTextCopy.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void LogMessage(string message) {
		if (sharedInstance != null) {
			Text consoleText = GameObject.Instantiate (sharedInstance.consoleTextCopy, sharedInstance.consoleTextCopy.transform.parent);
			consoleText.gameObject.SetActive (true);
			consoleText.text = message;
		} else {
			Debug.Log ("No console manager found!");
		}

		Debug.Log (message);
	}
}
