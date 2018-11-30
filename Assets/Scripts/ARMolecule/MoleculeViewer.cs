using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeViewer : MonoBehaviour {

    [SerializeField] private GameObject[] structures;

    private int lastIndex = 0;

    public const string MOLECULE_INDEX_KEY = "MOLECULE_INDEX_KEY";

    // Use this for initialization
    void Start () {
        
        for (int i = 0; i < structures.Length; i++) {
            structures[i].SetActive(false);
        }

        //show first structure
        this.ShowStructure(0);

        EventBroadcaster.Instance.AddObserver(EventNames.ARMoleculeEvents.ON_BTN_STRUCTURE_CLICKED, this.OnMoleculeClicked);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ARMoleculeEvents.ON_BTN_STRUCTURE_CLICKED);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ShowStructure(int index) {
        this.structures[this.lastIndex].SetActive(false);
        this.structures[index].SetActive(true);

        this.lastIndex = index;
    }

    private void OnMoleculeClicked(Parameters parameters) {
        int index = parameters.GetIntExtra(MOLECULE_INDEX_KEY, 0);

        //select structure
        this.ShowStructure(index);
    }
}
