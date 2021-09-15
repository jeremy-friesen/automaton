using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour{
    [SerializeField]
    Text percentageText = null;

    MusicSystem musicSystem;

    [SerializeField]
    Settings settings =  null;

    // Start is called before the first frame update
    void Start(){
        musicSystem = GameObject.Find("Music").GetComponent<MusicSystem>();
        setVolume(SaveSystem.Load().MusicVolume);
        //GetComponent<MusicVolumeSlider>().setVolume(SaveSystem.Load().MusicVolume);
    }

    public void updatePercentage(){
        percentageText.text = ((int)(GetComponent<Slider>().value * 100)).ToString() + "%";
    }

    public void updateVolume(){
        musicSystem.updateVolume(GetComponent<Slider>().value);
        if(settings.data == null)
            settings.Start();
        settings.setMusicVolume(GetComponent<Slider>().value);
    }

    public void setVolume(float volume){
        GetComponent<Slider>().value = volume;
        updatePercentage();
    }

    // Update is called once per frame
    void Update(){
        
    }
}
