using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer;
using UnityEngine.Networking;

public class ARNetworkManager : MonoBehaviour {

	private static ARNetworkManager sharedInstance = null;
	public static ARNetworkManager Instance {
		get {
			return sharedInstance;
		}
	}

	public const string UUID = "5acee51c-76c4-425d-9c22-25e08fab14da";

	[SerializeField] private AndroidBluetoothNetworkManagerHelper bluetoothHelper;
	[SerializeField] private AndroidBluetoothNetworkManager uNetManager;

	private bool isServer = false;

	void Awake() {
		sharedInstance = this;
		AndroidBluetoothMultiplayer.Initialize (UUID);
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	public AndroidBluetoothNetworkManagerHelper GetBluetoothHelperInstance() {
		return this.bluetoothHelper;
	}

	public void EnableBluetooth() {
		if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled ()) {
			ConsoleManager.LogMessage ("Bluetooth already enabled.");
		} else {
			if (AndroidBluetoothMultiplayer.RequestEnableBluetooth ()) {
				ConsoleManager.LogMessage ("Attempting to initialize bluetooth");
			}
		}
	}

	public bool IsBluetoothEnabled() {
		return AndroidBluetoothMultiplayer.GetIsBluetoothEnabled ();
	}

	public void StartAsHost() {
		this.isServer = true;
		bool result = AndroidBluetoothMultiplayer.StartServer((ushort) this.uNetManager.networkPort);
		if (result) {
			ConsoleManager.LogMessage ("Successfully started bluetooth host at " + this.uNetManager.networkAddress + " port " + this.uNetManager.networkPort);
			AndroidBluetoothMultiplayer.StartListening ();
		} else {
			ConsoleManager.LogMessage ("Failed to start bluetooth host.");
		}
	}

	public void StartAsClient() {
		if (this.isServer) {
			ConsoleManager.LogMessage ("Cannot start as client because the device is set as a host server.");
		} else {
			ConsoleManager.LogMessage ("Attempting to start as client");
			this.bluetoothHelper.StartClient ();
		}

	}

	public void StartScan() {
		AndroidBluetoothMultiplayer.StartDiscovery ();
		ConsoleManager.LogMessage("Started discovery");
	}

	public void AttemptConnect(BluetoothDevice device) {
		if (AndroidBluetoothMultiplayer.Connect (device.Address, (ushort)this.uNetManager.networkPort)) {
			ConsoleManager.LogMessage ("Successfully connected to device " +device.Name);
		} else {
			ConsoleManager.LogMessage ("Cannot connect to device " +device.Name);
		}
	}
}
