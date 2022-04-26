using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour{
    [SerializeField]
    private bool
		noPackagesRequired = false,
		tileLocationRequired = false,
		multipleTileLocationsRequired = false,
		packageSetOnLocationRequired = false;
	
    [SerializeField]
    private int numTileLocationsRequired = 2;

    private int numTilesReached;

    private bool tileReached;

    private bool noPackages;

    private bool packageSetOnTile;


    [SerializeField]
    private int levelNum = -1;

    [SerializeField]
    private GameObject endLevelBoxRoot = null;

    [SerializeField]
    private PanZoom panZoom = null;

    // Use this for initialization
    void Start(){
        tileReached = false;
        noPackages = false;
        packageSetOnTile = false;
        numTilesReached = 0;

        // un-comment if you want to see the user data path:
        //Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
    }

    void levelEnd(){
        // Save Stuff
        PlayerData data = SaveSystem.Load();
        if (data.LastLevel < levelNum){
            data.LastLevel = levelNum;
        }
        if (data.MaxLevel <= levelNum){
            data.MaxLevel = levelNum + 1;
        }

        // UI Stuff
        GameObject endLevelBox =
            endLevelBoxRoot.transform.Find("endLevelBox").gameObject;
        endLevelBox.SetActive(true);
        LiveInterpreter liveInterpreter =
            GameObject.Find("Code Window").GetComponent<LiveInterpreter>();
        if (liveInterpreter != null){
            endLevelBox.transform.Find("Stats").GetComponent<Text>().text =
                liveInterpreter.getStats();
            endLevelBox
                .GetComponent<LevelCompleteBox>()
                .setStars(liveInterpreter.getNumStars());

            // Save stars
            data.setNumStars(levelNum - 4, liveInterpreter.getNumStars());
        }
        else{
            LiveInterpreterManager liveInterpreterManager =
                GameObject
                    .Find("Code Window")
                    .GetComponent<LiveInterpreterManager>();
            endLevelBox.transform.Find("Stats").GetComponent<Text>().text =
                liveInterpreterManager.getStats();
            endLevelBox
                .GetComponent<LevelCompleteBox>()
                .setStars(liveInterpreterManager.getNumStars());

            // Save stars
            data
                .setNumStars(levelNum - 4,
                liveInterpreterManager.getNumStars());
        }

        panZoom.SettingsUp = true;
        SaveSystem.Save (data);
    }

    public void setNoPackages(){
        noPackages = true;
        if (
            (!tileLocationRequired || tileReached) &&
            (!packageSetOnLocationRequired || packageSetOnTile)
        ){
            levelEnd();
        }
    }

    public void setTileReached(){
        tileReached = true;
        if (
            (!noPackagesRequired || noPackages) &&
            (!packageSetOnLocationRequired || packageSetOnTile) &&
            (!multipleTileLocationsRequired)
        ){
            levelEnd();
        }
        else{
            numTilesReached++;
            if (
                multipleTileLocationsRequired &&
                numTilesReached >= numTileLocationsRequired
            ){
                levelEnd();
            }
        }
    }

    public void setTileNotReached(){
        if (multipleTileLocationsRequired){
            numTilesReached--;
            if (numTilesReached < 0){
                numTilesReached = 0;
            }
        }
        else{
            tileReached = false;
        }
    }

    public void resetNumTilesReached(){
        numTilesReached = 0;
        tileReached = false;
    }

    public void setPackageSetOnTile(){
        packageSetOnTile = true;
        if (
            (!noPackagesRequired || noPackages) &&
            (!tileLocationRequired || tileReached)
        ){
            levelEnd();
        }
    }
}
