using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacerManager : MonoBehaviour {

	private static ObjectPlacerManager sharedInstance = null;
	public static ObjectPlacerManager Instance {
		get {
			return sharedInstance;
		}
	}

	public const int BUILDING_ONE_ID = 0;
	public const int BUILDING_TWO_ID = 1;
	public const int BUILDING_THREE_ID = 2;
	public const int BUILDING_FOUR_ID = 3;
	public const int BUILDING_FIVE_ID = 4;
	public const int BUILDING_SIX_ID = 5;

	[SerializeField] private GameObject[] placeableObjectsCopy;

	private int currentID = 0;

	void Awake() { 
		sharedInstance = this;
	}

	void OnDestroy() {
		sharedInstance = null;
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < this.placeableObjectsCopy.Length; i++) {
			this.placeableObjectsCopy [i].SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelected(int selectedID) {
		this.currentID = selectedID;
	}

	public GameObject GetObjectByID() {
		return this.placeableObjectsCopy[this.currentID];
	}
}
