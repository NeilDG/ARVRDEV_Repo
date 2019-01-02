using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallPlacer : MonoBehaviour {

    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject wbBase;
    [SerializeField] private GameObject wbJoints;

    [SerializeField] private GameObject templateCopy;
    [SerializeField] private GameObject activePlatform;

    private bool plotSucess = false;
    private Vector3 baseOrigin;

	// Use this for initialization
	void Start () {
        EventBroadcaster.Instance.AddObserver(EventNames.ARWreckBallEvents.ON_RESET_CLICKED, this.OnResetSetupClicked);

        this.baseOrigin = this.wbBase.transform.localPosition;
        this.wbJoints.SetActive(false);
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ARWreckBallEvents.ON_RESET_CLICKED);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void MarkTargetLost() {
        this.plotSucess = false;
        this.wbJoints.SetActive(false);
        this.wbBase.transform.SetParent(this.transform.parent);
        this.wbBase.transform.localPosition = this.baseOrigin;
        this.wbBase.transform.localRotation = Quaternion.identity;
    }

    public void PlotWreckingBall() {
        if(this.plotSucess) {
            return;
        }

        this.wbBase.SetActive(true);
        this.wbBase.transform.SetParent(this.arCamera.transform);
        this.wbJoints.SetActive(true);
        this.plotSucess = true;
    }


    private void OnResetSetupClicked() {
        GameObject.Destroy(this.activePlatform);
        GameObject template = GameObject.Instantiate(this.templateCopy, this.transform);
        this.templateCopy.SetActive(true);

        this.activePlatform = template;
    }
}
