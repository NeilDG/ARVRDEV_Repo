﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace LostPolygon.AndroidBluetoothMultiplayer {
    /// <summary>
    /// A helper class that works in conjunction with <see cref="NetworkManager"/>.
    /// It automatically manages enabling Bluetooth, showing the device picker,
    /// and otherwise correctly handling the Bluetooth session.
    /// </summary>
    /// <example>
    /// The NetworkManager.Start* family of methods is mirrored, just use this class
    /// instead of using NetworkManager directly to start your client/server/host.
    /// </example>
    [RequireComponent(typeof(NetworkManager))]
    [AddComponentMenu("Network/Android Bluetooth Multiplayer/AndroidBluetoothNetworkManagerHelper")]
    public class AndroidBluetoothNetworkManagerHelper : MonoBehaviour {
        [SerializeField]
        [HideInInspector]
        protected NetworkManager _networkManager;

        [SerializeField]
        protected BluetoothNetworkManagerSettings _bluetoothNetworkManagerSettings = new BluetoothNetworkManagerSettings();

#if UNITY_ANDROID
        private bool _isInitialized;
        private BluetoothMultiplayerMode _desiredMode = BluetoothMultiplayerMode.None;
        private Action _clientAction;
        private Action _hostAction;

        /// <summary>
        /// A custom Bluetooth device browser can be used instead of native Android one.
        /// </summary>
        private ICustomDeviceBrowser _customDeviceBrowser;

        /// <summary>
        /// Gets a value indicating whether the plugin has initialized successfully.
        /// </summary>
        public bool IsInitialized {
            get { return _isInitialized; }
        }

        protected virtual void OnEnable() {
            _networkManager = GetComponent<NetworkManager>();

            // Setting the UUID. Must be unique for every application
            _isInitialized = AndroidBluetoothMultiplayer.Initialize(_bluetoothNetworkManagerSettings.Uuid);

            // Registering the event listeners
            AndroidBluetoothMultiplayer.ListeningStarted += OnBluetoothListeningStarted;
            AndroidBluetoothMultiplayer.ListeningStopped += OnBluetoothListeningStopped;
            AndroidBluetoothMultiplayer.AdapterEnabled += OnBluetoothAdapterEnabled;
            AndroidBluetoothMultiplayer.AdapterEnableFailed += OnBluetoothAdapterEnableFailed;
            AndroidBluetoothMultiplayer.AdapterDisabled += OnBluetoothAdapterDisabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnabled += OnBluetoothDiscoverabilityEnabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed += OnBluetoothDiscoverabilityEnableFailed;
            AndroidBluetoothMultiplayer.ConnectedToServer += OnBluetoothConnectedToServer;
            AndroidBluetoothMultiplayer.ConnectionToServerFailed += OnBluetoothConnectionToServerFailed;
            AndroidBluetoothMultiplayer.DisconnectedFromServer += OnBluetoothDisconnectedFromServer;
            AndroidBluetoothMultiplayer.ClientConnected += OnBluetoothClientConnected;
            AndroidBluetoothMultiplayer.ClientDisconnected += OnBluetoothClientDisconnected;
            AndroidBluetoothMultiplayer.DevicePicked += OnBluetoothDevicePicked;
        }

        protected virtual void OnDisable() {
            // Unregistering the event listeners
            AndroidBluetoothMultiplayer.ListeningStarted -= OnBluetoothListeningStarted;
            AndroidBluetoothMultiplayer.ListeningStopped -= OnBluetoothListeningStopped;
            AndroidBluetoothMultiplayer.AdapterEnabled -= OnBluetoothAdapterEnabled;
            AndroidBluetoothMultiplayer.AdapterEnableFailed -= OnBluetoothAdapterEnableFailed;
            AndroidBluetoothMultiplayer.AdapterDisabled -= OnBluetoothAdapterDisabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnabled -= OnBluetoothDiscoverabilityEnabled;
            AndroidBluetoothMultiplayer.DiscoverabilityEnableFailed -= OnBluetoothDiscoverabilityEnableFailed;
            AndroidBluetoothMultiplayer.ConnectedToServer -= OnBluetoothConnectedToServer;
            AndroidBluetoothMultiplayer.ConnectionToServerFailed -= OnBluetoothConnectionToServerFailed;
            AndroidBluetoothMultiplayer.DisconnectedFromServer -= OnBluetoothDisconnectedFromServer;
            AndroidBluetoothMultiplayer.ClientConnected -= OnBluetoothClientConnected;
            AndroidBluetoothMultiplayer.ClientDisconnected -= OnBluetoothClientDisconnected;
            AndroidBluetoothMultiplayer.DevicePicked -= OnBluetoothDevicePicked;
        }

        /// <summary>
        /// Sets the custom Bluetooth device browser.
        /// </summary>
        public virtual void SetCustomDeviceBrowser(ICustomDeviceBrowser customDeviceBrowser) {
            if (_customDeviceBrowser != null) {
                _customDeviceBrowser.OnDevicePicked -= OnBluetoothDevicePicked;
            }

            _customDeviceBrowser = customDeviceBrowser;
            if (_customDeviceBrowser != null) {
                _customDeviceBrowser.OnDevicePicked += OnBluetoothDevicePicked;
            }
        }

        #region NetworkManager methods

        /// <seealso cref="NetworkManager.StartClient()"/>
        public void StartClient() {
            StartBluetoothClient(() => _networkManager.StartClient());
        }

        /// <seealso cref="NetworkManager.StartClient()"/>
        public void StartClient(MatchInfo info) {
            StartBluetoothClient(() => _networkManager.StartClient(info));
        }

        /// <seealso cref="NetworkManager.StartClient()"/>
        public void StartClient(MatchInfo info, ConnectionConfig config) {
            StartBluetoothClient(() => _networkManager.StartClient(info, config));
        }

        /// <seealso cref="NetworkManager.StartServer()"/>
        public void StartServer() {
            StartBluetoothHost(() => _networkManager.StartServer());
        }

        /// <seealso cref="NetworkManager.StartServer()"/>
        public void StartServer(MatchInfo info) {
            StartBluetoothHost(() => _networkManager.StartServer(info));
        }

        /// <seealso cref="NetworkManager.StartServer()"/>
        public void StartServer(ConnectionConfig config, int maxConnections) {
            StartBluetoothHost(() => _networkManager.StartServer(config, maxConnections));
        }

        /// <seealso cref="NetworkManager.StartHost()"/>
        public void StartHost() {
            StartBluetoothHost(() => _networkManager.StartHost());
        }

        /// <seealso cref="NetworkManager.StartHost()"/>
        public void StartHost(MatchInfo info) {
            StartBluetoothHost(() => _networkManager.StartHost(info));
        }

        /// <seealso cref="NetworkManager.StartHost()"/>
        public void StartHost(ConnectionConfig config, int maxConnections) {
            StartBluetoothHost(() => _networkManager.StartHost(config, maxConnections));
        }

        #endregion

        #region Bluetooth events

        protected virtual void OnBluetoothListeningStarted() {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - ListeningStarted");
            }

            // Starting networking server if Bluetooth listening started successfully
            if (_hostAction != null) {
                _hostAction();
                _hostAction = null;
            }
        }

        protected virtual void OnBluetoothListeningStopped() {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - ListeningStopped");
            }

            if (_bluetoothNetworkManagerSettings.StopBluetoothServerOnListeningStopped) {
                AndroidBluetoothMultiplayer.Stop();
            }
        }

        protected virtual void OnBluetoothDevicePicked(BluetoothDevice device) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - DevicePicked: " + device);
            }

            if (_customDeviceBrowser != null) {
                _customDeviceBrowser.Close();
            }

            // Trying to connect to the device picked by user
            AndroidBluetoothMultiplayer.Connect(device.Address, (ushort) _networkManager.networkPort);
        }

        protected virtual void OnBluetoothClientDisconnected(BluetoothDevice device) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - ClientDisconnected: " + device);
            }
        }

        protected virtual void OnBluetoothClientConnected(BluetoothDevice device) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - ClientConnected: " + device);
            }
        }

        protected virtual void OnBluetoothDisconnectedFromServer(BluetoothDevice device) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - DisconnectedFromServer: " + device);
            }

            // Stop networking on Bluetooth failure
            StopAll();
            ClearState();
        }

        protected virtual void OnBluetoothConnectionToServerFailed(BluetoothDevice device) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - ConnectionToServerFailed: " + device);
            }
        }

        protected virtual void OnBluetoothConnectedToServer(BluetoothDevice device) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - ConnectedToServer: " + device);
            }

            // Trying to negotiate a Unity networking connection,
            // when Bluetooth client connected successfully
            if (_clientAction != null) {
                _clientAction();
                _clientAction = null;
            }
        }

        protected virtual void OnBluetoothAdapterDisabled() {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - AdapterDisabled");
            }

            if (NetworkServer.active) {
                StopAll();
                ClearState();
            }
        }

        protected virtual void OnBluetoothAdapterEnableFailed() {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - AdapterEnableFailed");
            }
        }

        protected virtual void OnBluetoothAdapterEnabled() {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - AdapterEnabled");
            }

            // Resuming desired action after enabling the adapter
            switch (_desiredMode) {
                case BluetoothMultiplayerMode.Server:
                    StopAll();
                    AndroidBluetoothMultiplayer.StartServer((ushort) _networkManager.networkPort);
                    break;
                case BluetoothMultiplayerMode.Client:
                    StopAll();
                    // Open device picker dialog
                    if (_customDeviceBrowser != null) {
                        _customDeviceBrowser.Open();
                    } else {
                        AndroidBluetoothMultiplayer.ShowDeviceList();
                    }
                    break;
            }

            _desiredMode = BluetoothMultiplayerMode.None;
        }

        protected virtual void OnBluetoothDiscoverabilityEnableFailed() {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log("Bluetooth Event - DiscoverabilityEnableFailed");
            }
        }

        protected virtual void OnBluetoothDiscoverabilityEnabled(int discoverabilityDuration) {
            if (_bluetoothNetworkManagerSettings.LogBluetoothEvents) {
                Debug.Log(string.Format("Event - DiscoverabilityEnabled: {0} seconds", discoverabilityDuration));
            }
        }

        #endregion

        private void StartBluetoothClient(Action onReadyAction) {
#if !UNITY_EDITOR
            _clientAction = onReadyAction;

            // If Bluetooth is enabled, immediately open the device picker
            if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
                StopAll();
                // Open device picker dialog
                if (_customDeviceBrowser != null) {
                    _customDeviceBrowser.Open();
                } else {
                    AndroidBluetoothMultiplayer.ShowDeviceList();
                }
            } else {
                // Otherwise, we have to enable Bluetooth first and wait for callback
                _desiredMode = BluetoothMultiplayerMode.Client;
                AndroidBluetoothMultiplayer.RequestEnableBluetooth();
            }
#else
            onReadyAction();
#endif
        }

        private void StartBluetoothHost(Action onReadyAction) {
#if !UNITY_EDITOR
            _hostAction = onReadyAction;

            // If Bluetooth is enabled, immediately start the Bluetooth server
            if (AndroidBluetoothMultiplayer.GetIsBluetoothEnabled()) {
                AndroidBluetoothMultiplayer.RequestEnableDiscoverability(_bluetoothNetworkManagerSettings.DefaultBluetoothDiscoverabilityInterval);
                StopAll(); // Just to be sure
                AndroidBluetoothMultiplayer.StartServer((ushort) _networkManager.networkPort);
            } else {
                // Otherwise, we have to enable Bluetooth first and wait for callback
                _desiredMode = BluetoothMultiplayerMode.Server;
                AndroidBluetoothMultiplayer.RequestEnableDiscoverability(_bluetoothNetworkManagerSettings.DefaultBluetoothDiscoverabilityInterval);
            }
#else
            onReadyAction();
#endif
        }

        private void StopAll() {
           AndroidBluetoothMultiplayer.Stop();
            _networkManager.StopHost();
        }

        private void ClearState() {
            _desiredMode = BluetoothMultiplayerMode.None;
            _clientAction = null;
            _hostAction = null;
        }
#endif

#if UNITY_EDITOR
        protected virtual void Reset() {
            OnValidate();
        }

        protected virtual void OnValidate() {
            if (String.IsNullOrEmpty(_bluetoothNetworkManagerSettings.Uuid)) {
                _bluetoothNetworkManagerSettings.Uuid = Guid.NewGuid().ToString();
            }
        }
#endif

        /// <summary>
        /// Container of Bluetooth-related settings.
        /// </summary>
        [Serializable]
        public class BluetoothNetworkManagerSettings {
            [Tooltip("Bluetooth service application identifier, must be unique for every application. " +
                     "If you have multiple scenes with different NetworkManager's in your project, " +
                     "make sure the UUID is identical everywhere, otherwise Bluetooth connections will fail.")]
            [SerializeField]
            protected string _uuid = "";

            [Tooltip("Bluetooth discoverability interval. Server is made discoverable over Bluetooth, so clients would " +
                     "have the ability to locate the server. On Android 4.0 and higher, value of 0 allows making device discoverable " +
                     "\"forever\" (until discoverability is disabled manually or Bluetooth is disabled).")]
            [SerializeField]
            protected int _defaultBluetoothDiscoverabilityInterval = 120;

            [Tooltip("Indicates whether to stop the Bluetooth server when listening " +
                     "for incoming Bluetooth connections has stopped.")]
            [SerializeField]
            protected bool _stopBluetoothServerOnListeningStopped = true;

            [Tooltip("Indicates whether Android Bluetooth Multiplayer events should be logged.")]
            [SerializeField]
            protected bool _logBluetoothEvents;

            /// <summary>
            /// Gets or sets the Bluetooth service UUID.
            /// </summary>
            /// <value>
            /// The UUID. Must be unique for every application
            /// </value>
            /// <exception cref="ArgumentException">UUID can't be empty.</exception>
            public string Uuid {
                get { return _uuid; }
                set {
                    if (String.IsNullOrEmpty(value))
                        throw new ArgumentException("UUID can't be empty", "value");

                    _uuid = value;
                }
            }

            /// <summary>
            /// Gets or sets the default Bluetooth discoverability interval.
            /// </summary>
            /// <exception cref="ArgumentException">Discoverability interval can't be less than 0.</exception>
            public int DefaultBluetoothDiscoverabilityInterval {
                get { return _defaultBluetoothDiscoverabilityInterval; }
                set {
                    if (value < 0)
                        throw new ArgumentException("Discoverability interval can't be < 0", "value");

                    _defaultBluetoothDiscoverabilityInterval = value;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether to stop the Bluetooth server when listening
            /// for incoming Bluetooth connections has stopped.
            /// </summary>
            public bool StopBluetoothServerOnListeningStopped {
                get { return _stopBluetoothServerOnListeningStopped; }
                set { _stopBluetoothServerOnListeningStopped = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether Android Bluetooth Multiplayer events should be logged.
            /// </summary>
            public bool LogBluetoothEvents {
                get { return _logBluetoothEvents; }
                set { _logBluetoothEvents = value; }
            }
        }
    }
}