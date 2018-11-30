using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScreen : View {

    [SerializeField] private GameObject hiddenContainer;
    [SerializeField] private GameObject expandContainer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnContainerToggleClicked(bool flag) {
        this.hiddenContainer.SetActive(!flag);
        this.expandContainer.SetActive(flag);
    }

    public void OnMoleculeBtnClicked(int index) {
        Parameters parameters = new Parameters();
        parameters.PutExtra(MoleculeViewer.MOLECULE_INDEX_KEY, index);
        EventBroadcaster.Instance.PostEvent(EventNames.ARMoleculeEvents.ON_BTN_STRUCTURE_CLICKED, parameters);
    }

    public override void OnRootScreenBack() {
        base.OnRootScreenBack();
        TwoChoiceDialog twoChoiceDialog = (TwoChoiceDialog)DialogBuilder.Create(DialogBuilder.DialogType.CHOICE_DIALOG);
        twoChoiceDialog.SetMessage("Go back to main menu?");
        twoChoiceDialog.SetOnConfirmListener(() => {
            LoadManager.Instance.LoadScene(SceneNames.MAIN_SCENE);
        });
    }
}
