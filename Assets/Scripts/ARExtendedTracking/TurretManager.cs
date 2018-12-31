using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TurretManager : MonoBehaviour, IVirtualButtonEventHandler {

    [SerializeField] private VirtualButtonBehaviour[] virtualButtons;
    [SerializeField] private BaseTurret[] turrets;

    // Use this for initialization
    void Start () {
        for(int i = 0; i < this.virtualButtons.Length; i++) {
            this.virtualButtons[i].RegisterEventHandler(this);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnButtonPressed(VirtualButtonBehaviour vb) {

        for (int i = 0; i < this.virtualButtons.Length; i++) {
            if (vb.VirtualButtonName == this.virtualButtons[i].VirtualButtonName) {

                if (this.turrets[i].IsTurretFiring()) {
                    this.turrets[i].StopTurret();
                }
                else {
                    this.turrets[i].FireTurretEndless();
                }
                
            }
        }
        
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        
    }


}
