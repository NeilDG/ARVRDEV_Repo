using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoAnchorManager : MonoBehaviour {

    [SerializeField] private Transform origParent;
    [SerializeField] private Transform alterParent;
    [SerializeField] private Transform videoTransform;

	// Use this for initialization
	void Start () {
        EventBroadcaster.Instance.AddObserver(EventNames.VideoAREvents.ON_VIDEO_DISJOINTED, this.OnVideoDisjointed);
        EventBroadcaster.Instance.AddObserver(EventNames.VideoAREvents.ON_VIDEO_ANCHORED, this.OnVideoAnchored);
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.VideoAREvents.ON_VIDEO_DISJOINTED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.VideoAREvents.ON_VIDEO_ANCHORED);
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnVideoDisjointed() {
        this.videoTransform.SetParent(this.alterParent);
    }

    private void OnVideoAnchored() {
        this.videoTransform.SetParent(this.origParent);
    }
}
