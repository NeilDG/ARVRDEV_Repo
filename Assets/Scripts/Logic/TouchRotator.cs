using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotator : MonoBehaviour {
	
	[SerializeField] private float _sensitivity;
	[SerializeField] private Vector3 _mouseReference;
	[SerializeField] private Vector3 _mouseOffset;
	[SerializeField] private Vector3 _rotation;
	[SerializeField] private bool _isRotating;

	void Start ()
	{
		_rotation = Vector3.zero;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0) && this._isRotating == false) {
			this._isRotating = true;
			this._mouseReference = Input.mousePosition;

		} else if (Input.GetMouseButtonUp (0)) {
			this._isRotating = false;
			//this.GetComponent<Rigidbody> ().AddTorque (this._rotation);
		}

		if(_isRotating)
		{
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

			// apply rotation
			//_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
			Vector3 force = Vector3.zero;
			force.z = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
			this.GetComponent<Rigidbody> ().AddTorque (force);

			// rotate
			transform.Rotate(_rotation);

			// store mouse
			_mouseReference = Input.mousePosition;
		}
	}

	/*void OnMouseDown()
	{
		// rotating flag
		_isRotating = true;

		// store mouse
		_mouseReference = Input.mousePosition;
	}

	void OnMouseUp()
	{
		// rotating flag
		_isRotating = false;
	}*/
}
