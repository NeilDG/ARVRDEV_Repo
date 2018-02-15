using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X01_Spawner : MonoBehaviour {

	[SerializeField] private GameObjectPool pool;
	public GameObjectPool pool2; //IN OOP, this is a NO-NO!!! attributes should be only retrieved by a getter.

	// Use this for initialization
	void Start () {
		pool.Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
