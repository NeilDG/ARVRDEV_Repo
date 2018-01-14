using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : APoolable {

	[SerializeField] private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		this.rigidBody = this.GetComponent<Rigidbody> ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.localPosition.y <= -5.0f && this.poolRef != null) {
			this.poolRef.ReleasePoolable (this);
		}
	}

	public override void Initialize ()
	{
		
	}

	public override void Release ()
	{
		this.rigidBody.velocity = Vector3.zero;
	}

	public override void OnActivate ()
	{
		
	}
}
