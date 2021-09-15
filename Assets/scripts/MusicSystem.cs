using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour{
  private AudioSource audioSource;

  private AudioClip[] audioClips = new AudioClip[3];
  private int currentAudioClipNum;

  private bool playing;

  // Start is called before the first frame update
  void Start(){
    // GameObject setup
    if (GameObject.FindGameObjectsWithTag("music").Length > 1){
      Destroy(this.gameObject);
      return;
    }
    DontDestroyOnLoad(this.gameObject);


    // Load each audioclip
    //audioClips[0] = Resources.Load("Music/OneMinuteBuildUp") as AudioClip;
    audioClips[0] = Resources.Load("Music/Summer") as AudioClip;
    audioClips[1] = Resources.Load("Music/Innovation") as AudioClip;
    audioClips[2] = Resources.Load("Music/Mystery") as AudioClip;

    // audioSource reference setup
    audioSource = GetComponent<AudioSource>();

    // read data for settings:
    PlayerData data = SaveSystem.Load();
    if(data.Music){
      //Debug.Log("data.Music is true");
      // Set audio clip for audio source
      nextTrack();
      play();
    } else{
      pause();
    }
    updateVolume(data.MusicVolume);
  }

  // Update is called once per frame
  void Update(){
    if(!audioSource.isPlaying && playing && GetComponent<AudioSource>().enabled){
      nextTrack();
    }
  }

  public void setOn(bool playing){
    if(playing){
      play();
    } else{
      pause();
    }
  }

  public void pause(){
    audioSource.Pause();
    playing = false;
  }

  public void play(){
    audioSource.Play();
    playing = true;
  }

  public void updateVolume(float volume){
    if(audioSource == null){
      audioSource = GetComponent<AudioSource>();
    }
    audioSource.volume = volume;
  }

  // jump to next track
  public void nextTrack(){
    //Debug.Log("nextTrack");
    currentAudioClipNum++;
    if (currentAudioClipNum >= audioClips.Length){
      currentAudioClipNum = 0;
    }
    audioSource.clip = audioClips[currentAudioClipNum];
    if(playing){
      audioSource.Play();
    }
  }
  
  public void prevTrack(){
    currentAudioClipNum--;
    if(currentAudioClipNum < 0){
      currentAudioClipNum = 2;
    }
    audioSource.clip = audioClips[currentAudioClipNum];
    if(playing){
      audioSource.Play();
    }
  }
}
