using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S18_CylinderPoolable : APoolable {
	private const float Y_THRESHOLD = -0.78f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.localPosition.y <= Y_THRESHOLD && this.poolRef != null) {
			this.poolRef.ReleasePoolable (this);
		}
	}

	public override void Initialize ()
	{
		Debug.Log ("Cylinder initialized!");
	}

	public override void OnActivate ()
	{
		
	}

	public override void Release ()
	{
		this.transform.localPosition = Vector3.zero;
		this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}
}
