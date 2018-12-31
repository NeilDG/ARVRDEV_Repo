using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ARCameraConfig : MonoBehaviour {

	// Use this for initialization
	void Start () {
		VuforiaARController.Instance.RegisterVuforiaStartedCallback (this.OnVuforiaStarted);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
        }
	}


	private void OnVuforiaStarted() {
		CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);

		/*if (CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.GRAYSCALE, true))
		{
			Debug.Log("Successfully registered pixel format " + Image.PIXEL_FORMAT.GRAYSCALE);

		}
		else
		{
			Debug.LogError(
				"Failed to register pixel format " + Image.PIXEL_FORMAT.GRAYSCALE +
				"\n the format may be unsupported by your device;" +
				"\n consider using a different pixel format.");
		}

        // Get the fields
        IEnumerable cameraFields = CameraDevice.Instance.GetCameraFields();
        // Print fields to device logs
        foreach (CameraDevice.CameraField field in cameraFields) {
            Debug.Log("Key: " + field.Key + "; Type: " + field.Type.ToString());
        }*/

    }
}
