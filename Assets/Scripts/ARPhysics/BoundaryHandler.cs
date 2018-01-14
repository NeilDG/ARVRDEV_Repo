using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryHandler : MonoBehaviour {

	private IBoundaryListener boundaryListener;

	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy() {
		this.boundaryListener = null;
	}

	public void SetListener(IBoundaryListener boundaryListener) {
		this.boundaryListener = boundaryListener;
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("[BoundaryHandler] Collision detected: " + collision.gameObject.name);
		APoolable poolableObject = collision.gameObject.GetComponent<APoolable> ();

		if (this.boundaryListener != null) {
			this.boundaryListener.OnExitBoundary (poolableObject);
		}
	}
}
