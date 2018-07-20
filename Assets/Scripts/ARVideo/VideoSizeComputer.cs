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
    private bool disjointed = false;

    private float ticks = 0.0f;
    private float timeout = 1.0f;

    private float distanceApart = 0.0f; //the distance between the first vs second target
    private const float SCALING_FACTOR = 0.2f;

    private VideoDebugScreen debugScreen;

    private Vector3 baseVideoSize;

    private void Awake() {
        sharedInstance = this;
    }
   
    // Use this for initialization
    void Start () {
        this.debugScreen = (VideoDebugScreen) ViewHandler.Instance.Show(ViewNames.VIDEO_DEBUG_SCREEN);

        this.baseVideoSize = this.videoPlane.localScale;
        this.videoPlane.gameObject.SetActive(false);

        EventBroadcaster.Instance.AddObserver(EventNames.VideoAREvents.ON_VIDEO_DISJOINTED, this.OnVideoDisjointed);
        EventBroadcaster.Instance.AddObserver(EventNames.VideoAREvents.ON_VIDEO_ANCHORED, this.OnVideoAnchored);
        
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.VideoAREvents.ON_VIDEO_DISJOINTED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.VideoAREvents.ON_VIDEO_ANCHORED);
        sharedInstance = null;
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

        //do not resize if video has been disjointed.
        if(this.disjointed) {
            return;
        }

        Vector3 newScale = Vector3.zero;
        newScale.y = this.baseVideoSize.y;

        newScale.x = this.baseVideoSize.x + (this.distanceApart * SCALING_FACTOR);
        newScale.z = this.baseVideoSize.z + (this.distanceApart * SCALING_FACTOR);

        this.videoPlane.localScale = newScale;
        this.videoPlane.gameObject.SetActive(true);
    }

    private void OnVideoDisjointed() {
        this.disjointed = true;
    }

    private void OnVideoAnchored() {
        this.disjointed = false;
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
