using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARDropoffDemo : MonoBehaviour {

    [SerializeField] private GameObject[] cubeCopies;
    [SerializeField] private Camera arCamera;

    private int index = 0;
    private List<GameObject> cubesSpawned = new List<GameObject>();

	// Use this for initialization
	void Start () {
        for(int i = 0; i < this.cubeCopies.Length; i++) {
            this.cubeCopies[i].SetActive(false);
        }
        EventBroadcaster.Instance.AddObserver(EventNames.ExtendTrackEvents.ON_RESET_CLICKED, this.OnResetEvent);
	}

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ExtendTrackEvents.ON_RESET_CLICKED);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButtonDown(0)) {

            Vector3 pos = this.cubeCopies[this.index].transform.localPosition;
            Vector3 hitPos = this.identifyPos();

            if(hitPos != Vector3.zero) {
                pos.x = hitPos.x; //get X and Z from ray cast
                pos.z = hitPos.z;
                pos.y += 2.0f; //use the original cube's Y position and offset.
                this.DropCube(pos);
            }
        }
	}

    /*private Vector3 identifyPos() {
        return this.cubeCopies[this.index].transform.localPosition;
    }*/

    private Vector3 identifyPos() {
        Ray ray = this.arCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            Vector3 hitPos = hit.point;
            Debug.Log("Hit pos: " + hitPos);
            return hitPos;
        }
        else {
            Debug.Log("No valid position found");
            return Vector3.zero;
        }
    }

    private void DropCube(Vector3 position) {
        GameObject cube = GameObject.Instantiate(this.cubeCopies[this.index], this.cubeCopies[this.index].transform.parent);
        cube.transform.localPosition = position;
        cube.SetActive(true);

        this.cubesSpawned.Add(cube);

        this.index++; this.index %= 3;
    }

    private void OnResetEvent() {
        for(int i = 0; i < this.cubesSpawned.Count; i++) {
            GameObject.Destroy(this.cubesSpawned[i]);
        }

        this.cubesSpawned.Clear();
    }
}
