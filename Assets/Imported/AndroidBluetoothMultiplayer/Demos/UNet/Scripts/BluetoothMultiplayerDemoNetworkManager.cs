using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples.UNet {
    public class BluetoothMultiplayerDemoNetworkManager : AndroidBluetoothNetworkManager {
        public GameObject TapMarkerPrefab; // Reference to the tap effect
        public bool StressTestMode;

        private const int kStressModeActors = 30;

        public override void OnStartServer() {
            base.OnStartServer();

            // Register the handler for the CreateTapMarkerMessage that is sent from client to server
            //NetworkServer.RegisterHandler(CreateTapMarkerMessage.kMessageType, OnServerCreateTapMarkerHandler);
			NetworkServer.RegisterHandler(ARMessage.messageType, this.OnReceivedClientMessage);
        }

        public override void OnStartClient(NetworkClient client) {
            base.OnStartClient(client);

            // Register the handler for the CreateTapMarkerMessage that is sent from server to clients
           // client.RegisterHandler(CreateTapMarkerMessage.kMessageType, OnClientCreateTapMarkerHandler);
			NetworkServer.RegisterHandler(ARMessage.messageType, this.OnHandleClientMessage);
        }

        public override void OnServerReady(NetworkConnection conn) {
            base.OnServerReady(conn);

            // Spawn the controllable actors
            int actorCount = !StressTestMode ? 1 : kStressModeActors;
            for (int i = 0; i < actorCount; i++) {
                Vector3 position = Random.insideUnitCircle * 15f;
                GameObject actorGameObject = (GameObject) Instantiate(playerPrefab, position, Quaternion.identity);
                TestActor testActor = actorGameObject.GetComponent<TestActor>();

                // Make them smaller and more random in stress test mode
                if (StressTestMode) {
                    testActor.PositionRandomOffset = 10f;
                    actorGameObject.transform.localScale *= 0.5f;
                    testActor.TransformLocalScale = actorGameObject.transform.localScale;
                }

                // Set player authority
                NetworkServer.SpawnWithClientAuthority(actorGameObject, conn);

                // Create a new virtual player for this actor
                PlayerController playerController = new PlayerController();
                playerController.gameObject = actorGameObject;
                playerController.unetView = actorGameObject.GetComponent<NetworkIdentity>();

                conn.playerControllers.Add(playerController);
            }
        }

        // Called when client receives a CreateTapMarkerMessage
        private void OnClientCreateTapMarkerHandler(NetworkMessage netMsg) {
            // Just instantiate a tap marker in the tap position
            CreateTapMarkerMessage createTapMarkerMessage = netMsg.ReadMessage<CreateTapMarkerMessage>();
            Instantiate(TapMarkerPrefab, createTapMarkerMessage.Position, Quaternion.identity);
        }

        // Called when server receives a CreateTapMarkerMessage
        private void OnServerCreateTapMarkerHandler(NetworkMessage netMsg) {
            CreateTapMarkerMessage createTapMarkerMessage = netMsg.ReadMessage<CreateTapMarkerMessage>();

            // Retransmit this message to all other clients except the one who initially sent it,
            // since that client already creates a local tap marker on his own
            foreach (NetworkConnection connection in NetworkServer.connections) {
                if (connection == null || connection == netMsg.conn)
                    continue;

                connection.Send(CreateTapMarkerMessage.kMessageType, createTapMarkerMessage);
            }

            foreach (NetworkConnection connection in NetworkServer.localConnections) {
                if (connection == null || connection == netMsg.conn)
                    continue;

                connection.Send(CreateTapMarkerMessage.kMessageType, createTapMarkerMessage);
            }
        }

		private void OnHandleClientMessage(NetworkMessage networkMsg) {
			ConsoleManager.LogMessage ("[CLIENT] Received message from " + networkMsg.conn.address + " with message: " + networkMsg.reader.ReadMessage<ARMessage> ().destination);
		}

		/// <summary>
		/// The server receives a message from its clients (possibly from itself). This function sends the message to all clients (except the sender).
		/// </summary>
		/// <param name="networkMsg">Network message.</param>
		private void OnReceivedClientMessage(NetworkMessage networkMsg) {
			ConsoleManager.LogMessage ("[SERVER] Received message from " + networkMsg.conn.address + " with message: " + networkMsg.reader.ReadMessage<ARMessage> ().destination);

			ARMessage arMessage = networkMsg.ReadMessage<ARMessage> ();

			for (int i = 0; i < NetworkServer.connections.Count; i++) {
				NetworkConnection connection = NetworkServer.connections [i];

				if (connection != null && connection != networkMsg.conn) {
					connection.Send (ARMessage.messageType, arMessage);
				}
			}

			for (int i = 0; i < NetworkServer.localConnections.Count; i++) {
				NetworkConnection connection = NetworkServer.connections [i];

				if (connection != null && connection != networkMsg.conn) {
					connection.Send (ARMessage.messageType, arMessage);
				}
			}
		}
    }


}