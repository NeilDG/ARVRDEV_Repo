using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LostPolygon.AndroidBluetoothMultiplayer;

public class BluetoothScreen : View {

	[SerializeField] private Text deviceDisplay;

	// Use this for initialization
	void Start () {
		AndroidBluetoothMultiplayer.DeviceDiscovered += this.OnDeviceDiscovered;
		this.deviceDisplay.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void OnStartServerButton() {
		ConsoleManager.LogMessage ("Attempting to start server");
		ARNetworkManager.Instance.InitializeBluetooth ();
	}

	public void OnStartScan() {
		ARNetworkManager.Instance.StartScan ();
	}

	public override void OnRootScreenBack ()
	{
		DialogInterface dialog = DialogBuilder.Create (DialogBuilder.DialogType.CHOICE_DIALOG);
		dialog.SetMessage ("Go back to main menu?");
		dialog.SetOnConfirmListener (() => {
			LoadManager.Instance.LoadScene(SceneNames.MAIN_SCENE);
		});
	}

	private void OnDeviceDiscovered(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device discovered! " + device.Name);
		Text displayText = GameObject.Instantiate (this.deviceDisplay, this.deviceDisplay.transform.parent);
		displayText.gameObject.SetActive (true);
		displayText.text = device.Name;
	}
}
