using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Plays video on a UI Raw Image
/// </summary>
public class UIVideoPlayer : MonoBehaviour {

    [SerializeField] private RawImage rawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Text buttonText;

    private bool isPlaying = false;

	// Use this for initialization
	void Start () {
        this.rawImage.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnVideoClicked() {
        if(!this.isPlaying) {
            this.isPlaying = true;
            this.buttonText.text = "STOP VIDEO";
            this.StartCoroutine(this.PlayVideo());
        }
        else {
            this.rawImage.gameObject.SetActive(false);
            this.videoPlayer.Stop();
            this.isPlaying = false;
            this.buttonText.text = "PLAY VIDEO";
        }
        
    }

    private IEnumerator PlayVideo() {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared) {
            yield return waitForSeconds;
            break;
        }
        this.rawImage.gameObject.SetActive(true);
        this.rawImage.texture = videoPlayer.texture;
        this.videoPlayer.Play();
    }
}
