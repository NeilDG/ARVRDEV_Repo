using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LostPolygon.AndroidBluetoothMultiplayer;

public class BluetoothScreen : View {

	[SerializeField] private Text deviceDisplay;
	[SerializeField] private Button bluetoothBtn;

	// Use this for initialization
	void Start () {
		AndroidBluetoothMultiplayer.DeviceDiscovered += this.OnDeviceDiscovered;
		AndroidBluetoothMultiplayer.ClientConnected += this.OnClientConnected;
		this.deviceDisplay.gameObject.SetActive (false);

		this.bluetoothBtn.enabled = !ARNetworkManager.Instance.IsBluetoothEnabled ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public override void OnRootScreenBack ()
	{
		DialogInterface dialog = DialogBuilder.Create (DialogBuilder.DialogType.CHOICE_DIALOG);
		dialog.SetMessage ("Go back to main menu?");
		dialog.SetOnConfirmListener (() => {
			LoadManager.Instance.LoadScene(SceneNames.MAIN_SCENE);
		});
	}

	public void OnEnableBluetoothButton() {
		ARNetworkManager.Instance.EnableBluetooth ();
	}

	public void OnStartServerButton() {
		ARNetworkManager.Instance.StartAsHost ();
	}

	public void OnStartScan() {
		ARNetworkManager.Instance.StartScan ();
	}

	public void OnStartClientButton() {
		ARNetworkManager.Instance.StartAsClient ();
	}

	///
	/// Delegate methods
	///

	private void OnDeviceDiscovered(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device discovered! " + device.Name);
		Text displayText = GameObject.Instantiate (this.deviceDisplay, this.deviceDisplay.transform.parent);
		displayText.gameObject.SetActive (true);
		displayText.text = device.Name;
	}

	private void OnClientConnected(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device connected! " + device.Name);
	}
}
