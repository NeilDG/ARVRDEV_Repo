using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostPolygon.AndroidBluetoothMultiplayer;

public class ARNetworkManagerHelper : AndroidBluetoothNetworkManagerHelper {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private bool CheckDiscoverableCompatibility() {
		string info = SystemInfo.operatingSystem;
		string apiString = info.Split ('/')[1].Split('(')[0].Split('-')[1];
		int apiLevel = int.Parse(apiString);
		if (apiLevel >= 26) {
			return false;
		} else {
			return true;
		}
	}

	protected override void StartBluetoothHost (System.Action onReadyAction)
	{

		#if !UNITY_EDITOR
		_hostAction = onReadyAction;

		// If Bluetooth is enabled, immediately start the Bluetooth server
		if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {	
			
			this.StopAll(); // Just to be sure
			//If Android 8.0 and above, do not enable discoverability. Simply enable bluetooth
			if (this.CheckDiscoverableCompatibility ()) {
				AndroidBluetoothMultiplayer.RequestEnableDiscoverability(_bluetoothNetworkManagerSettings.DefaultBluetoothDiscoverabilityInterval);
			}
			else {
				ConsoleManager.LogMessage("Detected API >= 26! You must enable Bluetooth discoverability manually by opening the bluetooth settings and " +
					"wait for the client to connect.");
			}

			AndroidBluetoothMultiplayer.StartServer((ushort) _networkManager.networkPort);
		} else {
			// Otherwise, we have to enable Bluetooth first and wait for callback
			_desiredMode = BluetoothMultiplayerMode.Server;
			//If Android 8.0 and above, do not enable discoverability. Simply enable bluetooth
			if (this.CheckDiscoverableCompatibility ()) {
				AndroidBluetoothMultiplayer.RequestEnableDiscoverability(_bluetoothNetworkManagerSettings.DefaultBluetoothDiscoverabilityInterval);
			}
			else {
				AndroidBluetoothMultiplayer.RequestEnableBluetooth();
				ConsoleManager.LogMessage("Detected API >= 26! You must enable Bluetooth discoverability manually by opening the bluetooth settings and " +
					"wait for the client to connect.");
			}
		}
		#else
		onReadyAction();
		#endif
	}
}
