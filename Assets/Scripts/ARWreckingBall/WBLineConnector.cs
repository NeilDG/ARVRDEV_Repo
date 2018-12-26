using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBLineConnector : MonoBehaviour {

    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform startConnector;
    [SerializeField] private Transform endConnector;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        line.SetPosition(0, this.startConnector.localPosition);
        line.SetPosition(1, this.endConnector.localPosition);
	}
}
