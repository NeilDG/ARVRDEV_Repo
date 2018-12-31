using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurret : MonoBehaviour {

    [SerializeField] private Animator animator;

    public const string FIRING_ANIM_KEY = "Firing";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Spider") && this.IsTurretFiring()) {
            //simply apply a push force
            other.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10.0f, ForceMode.Impulse);
            Debug.Log("Applying push force!");
        }
    }

    public void FireTurretEndless() {
        this.animator.SetBool(FIRING_ANIM_KEY, true);
    }

    public void StopTurret() {
        this.animator.SetBool(FIRING_ANIM_KEY, false);
    }

    public bool IsTurretFiring() {
        return this.animator.GetBool(FIRING_ANIM_KEY);
    }
}
