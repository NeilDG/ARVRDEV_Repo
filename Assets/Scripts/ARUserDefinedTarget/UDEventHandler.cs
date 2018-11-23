using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

public class UDEventHandler : MonoBehaviour, IUserDefinedTargetEventHandler{

    public const int MAX_UD_TARGETS = 1;

    [SerializeField] private ImageTargetBehaviour imageTargetTemplate;
    [SerializeField] private UserDefinedTargetBuildingBehaviour buildingBehaviour;
    [SerializeField] private FrameQualityScreen frameQualityScreen;

    private ImageTargetBuilder.FrameQuality currentQuality = ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE; //the current frame quality
    private ObjectTracker objectTracker;
    private DataSet userDefinedSet; //where new user-defined targets are added
    private int targetCounter = 0; //used for naming acquired targets

	// Use this for initialization
	void Start () {
        this.buildingBehaviour.RegisterEventHandler(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnInitialized () {
        this.objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if (this.objectTracker != null) {
            // Create a new dataset
            this.userDefinedSet = this.objectTracker.CreateDataSet();
            this.objectTracker.ActivateDataSet(this.userDefinedSet);
        }
    }

	public void OnFrameQualityChanged (ImageTargetBuilder.FrameQuality frameQuality) {
        this.currentQuality = frameQuality;
        this.frameQualityScreen.UpdateQuality(frameQuality);
	}

 
    public void OnNewTrackableSource(TrackableSource trackableSource) {
        this.targetCounter++;

        //deactivate dataset first
        this.objectTracker.DeactivateDataSet(this.userDefinedSet);

        // Destroy the oldest target if the dataset is full or the dataset 
        // already contains five user-defined targets.
        if (this.userDefinedSet.HasReachedTrackableLimit() || this.userDefinedSet.GetTrackables().Count() >= MAX_UD_TARGETS) {
            IEnumerable<Trackable> trackables = this.userDefinedSet.GetTrackables();
            Trackable oldest = null;
            foreach (Trackable trackable in trackables) {
                if (oldest == null || trackable.ID < oldest.ID)
                    oldest = trackable;
            }

            if (oldest != null) {
                Debug.Log("Destroying oldest trackable in UDT dataset: " + oldest.Name);
                this.userDefinedSet.Destroy(oldest, false);
            }
        }

        // Get predefined trackable and instantiate it
        //ImageTargetBehaviour imageTargetCopy = Instantiate(this.imageTargetTemplate);
        //imageTargetCopy.gameObject.name = "UserDefinedTarget-" + this.targetCounter;

        // Add the duplicated trackable to the data set and activate it
        this.userDefinedSet.CreateTrackable(trackableSource, this.imageTargetTemplate.gameObject);

        // Activate the dataset again
        this.objectTracker.ActivateDataSet(this.userDefinedSet);
        
        this.objectTracker.Stop();
        //this.imageTargetTemplate.ImageTarget.StartExtendedTracking();
        //this.objectTracker.ResetExtendedTracking();
        this.objectTracker.Start();
        

        // Make sure TargetBuildingBehaviour keeps scanning...
        this.buildingBehaviour.StartScanning();
    }

    public void OnScanClicked() {
        this.BuildNewTarget();
    }

    public IEnumerator StartAutoScan(float delay) {
        yield return new WaitForSeconds(delay);
        Debug.Log("UDT Event Handler auto scan initiated.");
        this.BuildNewTarget();
        this.StartCoroutine(this.StartAutoScan(delay));
    }

    public void BuildNewTarget() {
        if (this.currentQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_MEDIUM || this.currentQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH) {
            // create the name of the next target.
            // the TrackableName of the original, linked ImageTargetBehaviour is extended with a continuous number to ensure unique names
            string targetName = string.Format("{0}-{1}", this.imageTargetTemplate.TrackableName, this.targetCounter);

            // generate a new target:
            this.buildingBehaviour.BuildNewTarget(targetName, this.imageTargetTemplate.GetSize().x);

            
            InfoScreen infoScreen = (InfoScreen)ViewHandler.Instance.FindActiveView(ViewNames.INFO_SCREEN_NAME);
            infoScreen.SetVisibility(false);
        }
        else {
            InfoScreen infoScreen = (InfoScreen)ViewHandler.Instance.FindActiveView(ViewNames.INFO_SCREEN_NAME);
            infoScreen.SetMessage("Cannot show AR object. Point your camera at a surface with clear edges or textures.");
            infoScreen.SetVisibility(true);
        }
    }

}
