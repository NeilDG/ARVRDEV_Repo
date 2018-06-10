using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class FrameQualityScreen : View {

    [SerializeField] private Text frameText;

    // Use this for initialization
    void Start () {
        this.UpdateQuality(ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateQuality(ImageTargetBuilder.FrameQuality frameQuality) {
        if(this.frameText == null) {
            return;
        }

        if(frameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE) {
            this.frameText.text = "Frame Quality: NONE";
        }
        else if (frameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_LOW) {
            this.frameText.text = "Frame Quality: LOW";
        }
        else if(frameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_MEDIUM) {
            this.frameText.text = "Frame Quality: MEDIUM";
        }
        else if (frameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH) {
            this.frameText.text = "Frame Quality: HIGH";
        }

    }



}
