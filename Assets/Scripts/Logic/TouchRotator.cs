using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotator : MonoBehaviour {
	
	[SerializeField] private float sensitivity = 5.0f;
	[SerializeField] private Vector3 _rotation;
	[SerializeField] private bool _isRotating;

    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;

    private const float AMPLIFY = 2.0f;
    private float inertiaX = 0.0f;
    private float inertiaY = 0.0f;

    private float rotX = 0.0f;
    private float rotY = 0.0f;

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
            this.inertiaX = this._mouseOffset.x;
            this.inertiaY = this._mouseOffset.y;
        }

        if (_isRotating)
		{
			// offset
			_mouseOffset = (Input.mousePosition - _mouseReference);

            this.rotX = _mouseOffset.x * this.sensitivity * Mathf.Deg2Rad;
            this.rotY = _mouseOffset.y * this.sensitivity * Mathf.Deg2Rad;

            this.transform.Rotate(Vector3.up, -rotX);
            this.transform.Rotate(Vector3.right, rotY);

            // store mouse
            _mouseReference = Input.mousePosition;
		}

        if (this.inertiaX > 0.0f) {
            this.rotX = this.sensitivity * Mathf.Deg2Rad * this.inertiaX;
            this.inertiaX -= Time.deltaTime * this.sensitivity * AMPLIFY;
            this.transform.Rotate(Vector3.up, -rotX);  
        }
        else if(this.inertiaX < 0.0f) {
            this.rotX = this.sensitivity * Mathf.Deg2Rad * this.inertiaX;
            this.inertiaX += Time.deltaTime * this.sensitivity * AMPLIFY;
            this.transform.Rotate(Vector3.up, -rotX);
        }
        
        if (this.inertiaY > 0.0f) {
            this.rotY = this.sensitivity * Mathf.Deg2Rad * this.inertiaY;
            this.inertiaY -= Time.deltaTime * this.sensitivity * AMPLIFY;
            this.transform.Rotate(Vector3.right, rotY);
        }
        else if (this.inertiaY < 0.0f) {
            this.rotY = this.sensitivity * Mathf.Deg2Rad * this.inertiaY;
            this.inertiaY += Time.deltaTime * this.sensitivity * AMPLIFY;
            this.transform.Rotate(Vector3.right, rotY);
        }

    }
}
