using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour{
    [SerializeField]
    private GameObject panZoomButton = null;

    [SerializeField]
    private PanZoom panZoom = null;

    [SerializeField]
    private GameObject musicButton = null;

    [SerializeField]
    private GameObject musicVolumeSlider = null;

    [SerializeField]
    private GameObject FPSButton30 = null;
    [SerializeField]
    private GameObject FPSButton60 = null;

    public PlayerData data;

    // Start is called before the first frame update
    public void Start(){
        data = SaveSystem.Load();
        refreshButtons();
        
        if(panZoom != null){
            //panZoom.setEnabled(data.TouchPanZoom);
            panZoom.Enabled = data.TouchPanZoom;
        }
    }

    public void setTargetFPS(int target){
        Application.targetFrameRate = target;
        data.TargetFPS = target;
        refreshFPSButtons();
        SaveSystem.Save(data);
    }

    public void toggleTouchPanZoom(){
        data.TouchPanZoom = !data.TouchPanZoom;
        if(panZoom != null)
            panZoom.Enabled = data.TouchPanZoom;
        refreshTouchPanZoomButton();
        SaveSystem.Save(data);
    }
    
    public void toggleMusic(){
        data.Music = !data.Music;
        GameObject Music = GameObject.Find("Music");
        if(data.Music){
            Music.GetComponent<MusicSystem>().play();
        } else{
            Music.GetComponent<MusicSystem>().pause();
        }
        refreshMusicButton();
        SaveSystem.Save(data);
    }

    public void musicNextTrack(){
        MusicSystem musicSystem = GameObject.Find("Music").GetComponent<MusicSystem>();
        musicSystem.nextTrack();
    }

    public void musicPrevTrack(){
        MusicSystem musicSystem = GameObject.Find("Music").GetComponent<MusicSystem>();
        musicSystem.prevTrack();
    }

    public void setMusicVolume(float volume){
        // fix for bug where this function is called (by MusicVolumeSlider) before Start is called
        if(data.LastLevel == 0 && data.MaxLevel == 0)
            data = SaveSystem.Load();

        if(volume != data.MusicVolume){
            data.MusicVolume = volume;
            SaveSystem.Save(data);
        }
    }

    void refreshButtons(){
        refreshTouchPanZoomButton();
        refreshMusicButton();
        refreshFPSButtons();
        refreshMusicVolumeSlider();
    }

    void refreshTouchPanZoomButton(){
        if(data.TouchPanZoom){
            panZoomButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkboxTrue");
        } else {
            panZoomButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkbox");
        }
    }

    void refreshMusicButton(){
        if(data.Music){
            musicButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkboxTrue");
        } else{
            musicButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkbox");
        }
    }

    void refreshFPSButtons(){
        if(data.TargetFPS == 30){
            FPSButton30.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkboxTrue");
            FPSButton60.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkbox");
        } else if(data.TargetFPS == 60){
            FPSButton30.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkbox");
            FPSButton60.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/checkboxTrue");
        } else {
            //Debug.Log("Error: Target FPS not set to 30 or 60.");
        }
    }

    void refreshMusicVolumeSlider(){
        musicVolumeSlider.GetComponent<MusicVolumeSlider>().setVolume(data.MusicVolume);
    }

    // Update is called once per frame
    void Update(){
        
    }

    void resetData(){
        data = new PlayerData(-1, 0);
        SaveSystem.Save(data);
    }
}
