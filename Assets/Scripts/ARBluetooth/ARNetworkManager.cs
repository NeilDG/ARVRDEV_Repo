using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer;

public class ARNetworkManager : MonoBehaviour {

	private static ARNetworkManager sharedInstance = null;
	public static ARNetworkManager Instance {
		get {
			return sharedInstance;
		}
	}

	public const string UUID = "5acee51c-76c4-425d-9c22-25e08fab14da";

	[SerializeField] private AndroidBluetoothNetworkManagerHelper bluetoothHelper;


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

	public void InitializeBluetooth() {
		if (AndroidBluetoothMultiplayer.RequestEnableBluetooth()) {
		}
	}

	public void StartScan() {
		AndroidBluetoothMultiplayer.StartDiscovery ();
		ConsoleManager.LogMessage("Started discovery");
	}
}
