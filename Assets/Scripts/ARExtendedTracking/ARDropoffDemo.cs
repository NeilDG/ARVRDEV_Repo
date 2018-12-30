using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARDropoffDemo : MonoBehaviour {

    [SerializeField] private GameObject cubeCopy;
    [SerializeField] private Camera arCamera;

    private List<GameObject> cubesSpawned = new List<GameObject>();

	// Use this for initialization
	void Start () {
        this.cubeCopy.SetActive(false);
        EventBroadcaster.Instance.AddObserver(EventNames.ExtendTrackEvents.ON_RESET_CLICKED, this.OnResetEvent);
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ExtendTrackEvents.ON_RESET_CLICKED);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButtonDown(0)) {
            Debug.Log("Drop cube!");

            Vector3 pos = this.cubeCopy.transform.localPosition;
            pos.y += 2.0f;

            this.DropCube(pos);
        }
	}

    private void DropCube(Vector3 position) {
        GameObject cube = GameObject.Instantiate(this.cubeCopy, this.cubeCopy.transform.parent);
        cube.transform.localPosition = position;
        cube.SetActive(true);

        this.cubesSpawned.Add(cube);
    }

    private void OnResetEvent() {
        for(int i = 0; i < this.cubesSpawned.Count; i++) {
            GameObject.Destroy(this.cubesSpawned[i]);
        }

        this.cubesSpawned.Clear();
    }
}
