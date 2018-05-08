using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LostPolygon.AndroidBluetoothMultiplayer;

public class BluetoothScreen : View {

	[Header("Panel Displays")]
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject hiddenPanel;

	[Header("UI Elements")]
	[SerializeField] private Text deviceDisplay;
	[SerializeField] private Button bluetoothBtn;
	[SerializeField] private Button gameBtn;
	[SerializeField] private Button dummyBtn;

	// Use this for initialization
	void Start () {
		AndroidBluetoothMultiplayer.DeviceDiscovered += this.OnDeviceDiscovered;
		AndroidBluetoothMultiplayer.ClientConnected += this.OnClientConnected;
		AndroidBluetoothMultiplayer.ClientDisconnected += this.OnClientDisconnected;
		AndroidBluetoothMultiplayer.ConnectedToServer += this.OnServerConnected;
		AndroidBluetoothMultiplayer.DisconnectedFromServer += this.OnServerDisconnected;
		AndroidBluetoothMultiplayer.DevicePicked += this.OnDevicePicked;
		this.deviceDisplay.gameObject.SetActive (false);
		this.gameBtn.gameObject.SetActive (false);
		this.dummyBtn.gameObject.SetActive (false);
		this.ShowMainPanel ();

		this.bluetoothBtn.enabled = !ARNetworkHub.Instance.IsBluetoothEnabled ();
	}

	void OnDestroy() {
		AndroidBluetoothMultiplayer.DeviceDiscovered -= this.OnDeviceDiscovered;
		AndroidBluetoothMultiplayer.ClientConnected -= this.OnClientConnected;
		AndroidBluetoothMultiplayer.ClientDisconnected -= this.OnClientDisconnected;
		AndroidBluetoothMultiplayer.ConnectedToServer -= this.OnServerConnected;
		AndroidBluetoothMultiplayer.DisconnectedFromServer -= this.OnServerDisconnected;
		AndroidBluetoothMultiplayer.DevicePicked -= this.OnDevicePicked;
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
		ARNetworkHub.Instance.EnableBluetooth ();
	}

	public void OnStartServerButton() {
		ARNetworkHub.Instance.StartAsHost ();
	}

	public void OnStartScan() {
		ARNetworkHub.Instance.StartScan ();
	}

	public void OnStartClientButton() {
		ARNetworkHub.Instance.StartAsClient ();
	}

	public void OnProceedToGame() {
		this.HideMainPanel ();
	}

	public void HideMainPanel() {
		this.mainPanel.gameObject.SetActive (false);
		this.hiddenPanel.gameObject.SetActive (true);
	}

	public void ShowMainPanel() {
		this.mainPanel.gameObject.SetActive (true);
		this.hiddenPanel.gameObject.SetActive (false);
	}

	public void OnSendDummyButton() {
		ARNetworkHub.Instance.SendDummyData ();
	}

	///
	/// Bluetooth delegate methods
	///
	private void OnDeviceDiscovered(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device discovered! " + device.Name);
		Text displayText = GameObject.Instantiate (this.deviceDisplay, this.deviceDisplay.transform.parent);
		displayText.gameObject.SetActive (true);
		displayText.text = device.Name;
	}

	private void OnClientConnected(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device connected: " + device.Name);
		this.gameBtn.gameObject.SetActive (true);
		this.dummyBtn.gameObject.SetActive (true);
	}

	private void OnClientDisconnected(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device disconnected: " + device.Name);
	}

	private void OnServerConnected(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Successfully connected to device " + device.Name);
		this.gameBtn.gameObject.SetActive (true);
		this.dummyBtn.gameObject.SetActive (true);

		//ARNetworkHub.Instance.RegisterNetworkEvents ();
	}

	private void OnServerDisconnected(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Disconnected from device " + device.Name);
	}

	private void OnDevicePicked(BluetoothDevice device) {
		ConsoleManager.LogMessage ("Device picked: " + device.Name);
	}
}
