using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucySpawner : MonoBehaviour
{
    [SerializeField] private GameObject lucyObject;

    private float Y_SPAWN = 0.0361f;
    private float Y_THRESHOLD = -0.311f;

    private Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        this.originPos = this.lucyObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.lucyObject.transform.localPosition.y < Y_THRESHOLD) {
            Vector3 pos = this.originPos;
            pos.y = Y_SPAWN;

            this.lucyObject.transform.localPosition = pos;
            this.lucyObject.transform.localRotation = Quaternion.identity;
        }
    }
}
