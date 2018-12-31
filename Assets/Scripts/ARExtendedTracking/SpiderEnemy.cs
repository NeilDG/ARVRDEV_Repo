using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour {

    [SerializeField] private Animator animator;

    private const string WALK_KEY_ANIM = "Walk";
    private const string ATTACK_KEY_ANIM = "Attack";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(this.IsWalking()) {
            Vector3 pos = this.transform.localPosition;
            pos.z -= 0.4f * Time.deltaTime;

            this.transform.localPosition = pos;
        }

        if(this.transform.localPosition.y <= -2.0f) {
            GameObject.Destroy(this.gameObject);
        }
	}
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name.Contains("Turret")) {
            this.BeIdle();
            this.Attack();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name.Contains("Turret")) {
            this.StopAttack();
            this.WalkTowards();
        }
    }

    public void WalkTowards() {
        this.animator.SetBool(WALK_KEY_ANIM, true);
    }

    public bool IsWalking() {
        return this.animator.GetBool(WALK_KEY_ANIM);
    }

    public void BeIdle() {
        this.animator.SetBool(WALK_KEY_ANIM, false);
    }

    public void Attack() {
        this.animator.SetBool(ATTACK_KEY_ANIM, true);
    }

    public void StopAttack() {
        this.animator.SetBool(ATTACK_KEY_ANIM, false);
    }
}
