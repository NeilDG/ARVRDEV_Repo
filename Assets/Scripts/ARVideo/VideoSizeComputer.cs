using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Computes for the size of the video based on the distance of two image targets.
/// By: NeilDG
/// </summary>
public class VideoSizeComputer : MonoBehaviour {

    private static VideoSizeComputer sharedInstance = null;
    public static VideoSizeComputer Instance {
        get {
            return sharedInstance;
        }
    }

    [SerializeField] private VideoTargetRef firstTarget;
    [SerializeField] private VideoTargetRef secondTarget;
    [SerializeField] private Transform videoPlane;

    private bool firstTargetDetected = false;
    private bool secondTargetDetected = false;

    private float ticks = 0.0f;
    private float timeout = 1.0f;

    private float distanceApart = 0.0f; //the distance between the first vs second target
    private const float SCALING_FACTOR = 0.2f;

    private VideoDebugScreen debugScreen;

    private Vector3 baseVideoSize;

    private void Awake() {
        sharedInstance = this;
    }

    private void OnDestroy() {
        sharedInstance = null;
    }

    // Use this for initialization
    void Start () {
        this.debugScreen = (VideoDebugScreen) ViewHandler.Instance.Show(ViewNames.VIDEO_DEBUG_SCREEN);

        this.baseVideoSize = this.videoPlane.localScale;
        this.videoPlane.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        this.ticks += Time.deltaTime;

        //compute video size every timeout
		if(this.ticks > this.timeout && this.firstTargetDetected && this.secondTargetDetected) {
            this.ticks = 0.0f;

            this.distanceApart = Vector2.Distance(this.firstTarget.transform.position, this.secondTarget.transform.position);
            this.debugScreen.DisplayDistance(this.distanceApart);

            this.Resize();
        }
	}

    private void Resize() {
        Vector3 newScale = Vector3.zero;
        newScale.y = this.baseVideoSize.y;

        newScale.x = this.baseVideoSize.x + (this.distanceApart * SCALING_FACTOR);
        newScale.z = this.baseVideoSize.z + (this.distanceApart * SCALING_FACTOR);

        this.videoPlane.localScale = newScale;
        this.videoPlane.gameObject.SetActive(true);
    }

    public void OnDetected(VideoTargetRef videoTargetRef) {
       if(videoTargetRef == this.firstTarget && !this.firstTargetDetected) {
            Debug.Log("Detected first target");
            this.firstTargetDetected = true;
        }

       else if(videoTargetRef == this.secondTarget && !this.secondTargetDetected) {
            Debug.Log("Detected second target");
            this.secondTargetDetected = true;
        }
    }

    public void OnLost(VideoTargetRef videoTargetRef) {

    }

}
