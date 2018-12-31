using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CameraSettingsScreen : View {

    [SerializeField] private Slider exposureSlider;
    [SerializeField] private Text valueText;

	// Use this for initialization
	void Start () {
        this.SetSliderValues();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetSliderValues() {
        string minExpValue;
        string maxExpValue;
        string currExpValue;
        
        CameraDevice.Instance.GetField("min-exposure-compensation", out minExpValue);
        CameraDevice.Instance.GetField("max-exposure-compensation", out maxExpValue);
        CameraDevice.Instance.GetField("exposure-compensation", out currExpValue);

        Debug.Log("Min value: " + minExpValue + " Max value: " + maxExpValue + " Curr  value: " + currExpValue);
        if(!string.IsNullOrEmpty(minExpValue)) {
            this.exposureSlider.minValue = float.Parse(minExpValue);
        }
        if(!string.IsNullOrEmpty(maxExpValue)) {
            this.exposureSlider.maxValue = float.Parse(maxExpValue);
        }
        if(!string.IsNullOrEmpty(currExpValue)) {
            this.exposureSlider.value = float.Parse(currExpValue);
        }
        
       
       
    }

    public void OnValueChanged() {
        Debug.Log("Value: " + this.exposureSlider.value);
        CameraDevice.Instance.SetField("exposure-compensation", this.exposureSlider.value.ToString());
        this.valueText.text = this.exposureSlider.value.ToString();
    }

    public void OnApplyClicked() {
        this.Hide();
    }

    public override void OnHideStarted() {
        base.OnHideStarted();
    }

    public override void OnShowStarted() {
        base.OnShowStarted();
        this.SetSliderValues();
    }

    public override void OnShowCompleted() {
        base.OnShowCompleted();
        BlackOverlay.Hide(); //hide overlay so user can get a better look at the camera setting.
    }
}
