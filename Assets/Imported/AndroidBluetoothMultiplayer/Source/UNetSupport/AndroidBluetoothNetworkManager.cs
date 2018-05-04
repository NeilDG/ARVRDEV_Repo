﻿using UnityEngine;
using UnityEngine.Networking;

namespace LostPolygon.AndroidBluetoothMultiplayer {
    /// <summary>
    /// Version of <see cref="NetworkManager"/> that disconnects
    /// from the Bluetooth server when UNet client is stopped.
    /// </summary>
    [AddComponentMenu("Network/Android Bluetooth Multiplayer/AndroidBluetoothNetworkManager")]
    public class AndroidBluetoothNetworkManager : NetworkManager {
        public override void OnStopClient() {
            base.OnStopClient();

#if UNITY_ANDROID
            // Stopping all Bluetooth connectivity on Unity networking disconnect event
            AndroidBluetoothMultiplayer.Stop();
#endif
        }

#if UNITY_EDITOR
        protected virtual void Reset() {
            OnValidate();
        }

        protected virtual void OnValidate() {
            networkAddress = "127.0.0.1";
        }
#endif
    }
}