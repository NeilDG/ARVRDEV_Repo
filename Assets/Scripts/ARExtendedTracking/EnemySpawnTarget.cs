using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class EnemySpawnTarget : ImageTargetBehaviour {

    private EnemySpawnManager spawnManager;
    private bool activated = false;

	// Use this for initialization
	void Start () {
        this.spawnManager = this.transform.Find("Container").GetComponent<EnemySpawnManager>();
        this.spawnManager.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnTrackerUpdate(Status newStatus) {
        base.OnTrackerUpdate(newStatus);

        if(newStatus == Status.TRACKED && !this.activated) {
            this.spawnManager.gameObject.SetActive(true);
            this.activated = true;
        }
        else if(newStatus == Status.NO_POSE && this.activated) {
            this.spawnManager.gameObject.SetActive(false);
            this.activated = false;
        }
    }
}
