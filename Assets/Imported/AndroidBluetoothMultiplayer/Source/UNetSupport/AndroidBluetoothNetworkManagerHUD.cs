using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace LostPolygon.AndroidBluetoothMultiplayer {
    /// <summary>
    /// Version of <see cref="NetworkManagerHUD"/> that uses
    /// <see cref="AndroidBluetoothNetworkManagerHelper"/> for networking routines.
    /// </summary>
    [AddComponentMenu("Network/Android Bluetooth Multiplayer/AndroidBluetoothNetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [RequireComponent(typeof(AndroidBluetoothNetworkManagerHelper))]
    public class AndroidBluetoothNetworkManagerHUD : NetworkManagerHUD {
#if UNITY_ANDROID
        private AndroidBluetoothNetworkManagerHelper managerHelper;
#endif

        private void Awake() {
            manager = GetComponent<NetworkManager>();
#if UNITY_ANDROID
            managerHelper = GetComponent<AndroidBluetoothNetworkManagerHelper>();
#endif
        }

        private void Update() {
#if UNITY_ANDROID
            if (!showGUI)
                return;

            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null) {
                if (Input.GetKeyDown(KeyCode.S)) {
                    managerHelper.StartServer();
                }
                if (Input.GetKeyDown(KeyCode.H)) {
                    managerHelper.StartHost();
                }
                if (Input.GetKeyDown(KeyCode.C)) {
                    managerHelper.StartClient();
                }
            }
            if (NetworkServer.active && manager.IsClientConnected()) {
                if (Input.GetKeyDown(KeyCode.X)) {
                    manager.StopHost();
                }
            }
#else
            BaseUpdate();
#endif
        }

        private void OnGUI() {
#if UNITY_ANDROID
            if (!showGUI)
                return;

            if (Application.platform == RuntimePlatform.Android) {
                int xpos = 10 + offsetX;
                int ypos = 50 + offsetY;
                const int spacing = 60;
                const int spacingSmall = 24;
                const int buttonHeight = 55;

                bool noConnection = manager.client == null || manager.client.connection == null ||
                                    manager.client.connection.connectionId == -1;

                BluetoothMultiplayerMode bluetoothMultiplayerMode = AndroidBluetoothMultiplayer.GetCurrentMode();
                if (bluetoothMultiplayerMode == BluetoothMultiplayerMode.None && !manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null) {
                    if (noConnection) {
                        if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Bluetooth Client(C)")) {
                            managerHelper.StartClient();
                        }

                        ypos += spacing;

                        if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Bluetooth Host(H)")) {
                            managerHelper.StartHost();
                        }
                        ypos += spacing;

                        if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Bluetooth Server Only(S)")) {
                            managerHelper.StartServer();
                        }
                        ypos += spacing;
                    } else {
                        GUI.Label(new Rect(xpos, ypos, 200, buttonHeight), "Connecting to " + manager.networkAddress + ":" + manager.networkPort + "..");
                        ypos += spacingSmall;

                        if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Cancel Connection Attempt")) {
                            manager.StopClient();
                        }
                    }
                } else {
                    if (NetworkServer.active) {
                        string serverMsg = "Server: port=" + manager.networkPort;
                        GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                        ypos += spacingSmall;
                    }
                    if (manager.IsClientConnected()) {
                        GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                        ypos += spacingSmall;
                    }
                }

                if (manager.IsClientConnected() && !ClientScene.ready) {
                    if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Client Ready")) {
                        ClientScene.Ready(manager.client.connection);

                        if (ClientScene.localPlayers.Count == 0) {
                            ClientScene.AddPlayer(0);
                        }
                    }
                    ypos += spacing;
                }

                if (NetworkServer.active || manager.IsClientConnected()) {
                    if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Stop (X)")) {
                        manager.StopHost();
                    }
                    ypos += spacing;
                } else if (bluetoothMultiplayerMode != BluetoothMultiplayerMode.None) {
                    if (bluetoothMultiplayerMode == BluetoothMultiplayerMode.Client) {
                        GUI.Label(new Rect(xpos, ypos, 300, 20), "Connecting to Bluetooth server...");
                        ypos += spacingSmall;
                    } else if (bluetoothMultiplayerMode == BluetoothMultiplayerMode.Server) {
                        GUI.Label(new Rect(xpos, ypos, 300, 20), "Starting Bluetooth server...");
                        ypos += spacingSmall;
                    }
                    if (GUI.Button(new Rect(xpos, ypos, 200, buttonHeight), "Stop (X)")) {
                        AndroidBluetoothMultiplayer.Stop();
                    }
                    ypos += spacing;
                }
            } else {
                BaseOnGUI();
            }
#else
            BaseOnGUI();
#endif
        }

        private void BaseUpdate() {
            // Call the base method via reflection since it's private
            typeof(NetworkManagerHUD).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
        }

        private void BaseOnGUI() {
            // Call the base method via reflection since it's private
            typeof(NetworkManagerHUD).GetMethod("OnGUI", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, null);
        }
    }
}