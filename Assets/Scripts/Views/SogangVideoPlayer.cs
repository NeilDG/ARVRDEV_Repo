using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SogangVideoPlayer : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private VideoPlayer videoPlayer;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutine(this.PlayVideo());
        this.StartCoroutine(this.DelayNextScene(15.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private IEnumerator DelayNextScene(float seconds) {
        yield return new WaitForSeconds(seconds);
        LoadManager.Instance.LoadScene(SceneNames.MAIN_SCENE);
    }

}
