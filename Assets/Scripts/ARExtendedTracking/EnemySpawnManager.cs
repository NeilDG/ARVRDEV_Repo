using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    [SerializeField] private SpiderEnemy[] enemyCopies;
    private float ticks = 0.0f;
    private float interval = 2.0f;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < this.enemyCopies.Length; i++) {
            this.enemyCopies[i].gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.ticks += Time.deltaTime;

        if(this.ticks > this.interval) {
            this.ticks = 0.0f;
            this.ProcessSpawn();
        }
	}

    public void ProcessSpawn() {
        int randIndex = Random.Range(0, this.enemyCopies.Length);
        SpiderEnemy spawnEnemy = GameObject.Instantiate(this.enemyCopies[randIndex], this.transform) as SpiderEnemy;
        spawnEnemy.gameObject.SetActive(true);
        spawnEnemy.WalkTowards();
    }
}
