using UnityEngine;

#pragma warning disable 618

namespace LostPolygon.AndroidBluetoothMultiplayer.Examples.LegacyNetworking {
    /// <summary>
    /// A very simple object. Moves to the position of the touch with interpolation.
    /// </summary>
    public class TestActor : MonoBehaviour {
        public float Speed = 100f;
        public double NetworkInterpolationBackTime = 0.11;
        public float PositionRandomOffset = 0f;
        public bool IsUseInterpolation;

        private Vector3 _destination;
        private Color _color;
        private Transform _transform;
        private NetworkView _networkView;
        private NetworkTransformInterpolation _transformInterpolation;
        private SpriteRenderer _spriteRenderer;

        private readonly Color[] kColors = {
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.red,
            Color.white,
            Color.yellow
        };

        private void Awake() {
            _transform = GetComponent<Transform>();
            _networkView = GetComponent<NetworkView>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _destination = transform.position;

            _color = kColors[Random.Range(0, kColors.Length)];
            _spriteRenderer.color = _color;

            _transformInterpolation = new NetworkTransformInterpolation();
            _transformInterpolation.InterpolationBackTime = NetworkInterpolationBackTime;
        }

        private void Start() {
            if (_networkView.isMine) {
                _networkView.RPC(
                    "SetProperties",
                    RPCMode.OthersBuffered,
                    _color.r,
                    _color.g,
                    _color.b,
                    _color.a,
                    transform.localScale,
                    IsUseInterpolation
                    );
            }
        }

        private void Update() {
            if (_networkView.isMine) {
                _destination.z = 0f;
                _transform.position = Vector3.MoveTowards(_transform.position, _destination, Speed * Time.deltaTime);

                if (Input.GetMouseButtonDown(0)) {
                    _destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _destination += (Vector3) (Random.insideUnitCircle * PositionRandomOffset);
                }
            } else {
                Vector3 interpolatedPosition = _transform.position;
                Quaternion interpolatedRotation = _transform.rotation;
                _transformInterpolation.Update(ref interpolatedPosition, ref interpolatedRotation);

                _transform.position = interpolatedPosition;
                _transform.rotation = interpolatedRotation;
            }
        }

        [RPC]
        private void SetProperties(float colorR, float colorG, float colorB, float colorA, Vector3 localScale, bool isUseInterpolation) {
            _spriteRenderer.color = new Color(colorR, colorG, colorB, colorA);
            _transform.localScale = localScale;
            IsUseInterpolation = isUseInterpolation;
        }

        private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
            // Serialize the position
            if (IsUseInterpolation) {
                _transformInterpolation.OnSerializeNetworkView(stream, info, _transform.position, _transform.rotation);
            } else {
                Vector3 position;
                Quaternion rotation;
                if (stream.isWriting) {
                    position = _transform.position;
                    rotation = _transform.rotation;
                    stream.Serialize(ref position);
                    stream.Serialize(ref rotation);
                } else {
                    position = Vector3.zero;
                    rotation = Quaternion.identity;
                    stream.Serialize(ref position);
                    stream.Serialize(ref rotation);

                    _transform.position = position;
                    _transform.rotation = rotation;
                }
            }
        }
    }
}

#pragma warning restore 618