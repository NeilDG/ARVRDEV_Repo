using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScreen : View {

    [SerializeField] private GameObject hiddenContainer;
    [SerializeField] private GameObject expandContainer;

    [Header("FPS Settings")]

    [SerializeField] private Text fpsButtonText;
    [SerializeField] private Text fpsCounter;
    [SerializeField] private Text fpsLog;
    [SerializeField] private GameObject fpsContainer;
    [SerializeField] private Transform fpsLogParent;

    private bool isFPSShown = true;

    private float updateInterval = 0.5F;
    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    private float accumFPS = 0.0f;
    private int counter = 0;
    private const int MAX_COUNTER = 10;

    // Use this for initialization
    void Start () {
        this.OnToggleFPS();
        this.OnContainerToggleClicked(false);
        this.fpsContainer.SetActive(false);
        this.fpsLog.gameObject.SetActive(false);

        this.timeleft = this.updateInterval;
	}
	
	// Update is called once per frame
	void Update () {
        this.ComputeFPS();
	}


    private void ComputeFPS() {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        float fps = 0.0f;
        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0) {
            // display two fractional digits (f2 format)
            fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            this.fpsCounter.text = format;

            if (fps < 30)
                this.fpsCounter.color = Color.yellow;
            else
                if (fps < 10)
                this.fpsCounter.color = Color.red;
            else
                this.fpsCounter.color = Color.green;
            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;

            if (counter < MAX_COUNTER) {
                counter++;
                this.accumFPS += fps;
            }
            else {
                float overtime = this.updateInterval * MAX_COUNTER;
                this.CreateFPSLog((this.accumFPS / MAX_COUNTER), overtime);   
                counter = 0;
                this.accumFPS = 0.0f;
            }
        }

       
    }

    private void CreateFPSLog(float averageFPS, float overtime) {
        string text = "Average FPS over " + overtime + " seconds: " + averageFPS;

        Text fpsLog = GameObject.Instantiate(this.fpsLog, this.fpsLogParent) as Text;
        fpsLog.text = text;
        fpsLog.gameObject.SetActive(true);

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

    public void OnToggleFPS() {
        this.isFPSShown = !this.isFPSShown;
        this.fpsContainer.SetActive(this.isFPSShown);

        if(this.isFPSShown) {
            this.fpsButtonText.text = "Hide FPS";
        }
        else {
            this.fpsButtonText.text = "Show FPS";
        }
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
