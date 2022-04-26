using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{

	// last level completed by player
	private int lastLevel;
	public int LastLevel{
		get{
			return lastLevel;
		}
		set{
			lastLevel = value;
		}
	}

	// furthest level unlocked by player
	private int maxLevel;
	public int MaxLevel{
		get{
			return maxLevel;
		}
		set{
			maxLevel = value;
		}
	}

	// touch to pan/zoom option
	private bool touchPanZoom;
	public bool TouchPanZoom{
		get{
			return touchPanZoom;
		}
		set{
			touchPanZoom = value;
		}
	}

	// music option
	private bool music;
	public bool Music{
		get{
			return music;
		}
		set{
			music = value;
		}
	}

	// music volume
	private float musicVolume;
	public float MusicVolume{
		get{
			return musicVolume;
		}
		set{
			musicVolume = value;
		}
	}

	// music track
	/*private int musicTrack;
	public int MusicTrack{
		get{
			return musicTrack;
		}
		set{
			musicTrack = value;
		}
	}*/

	// target FPS
	private int targetFPS;
	public int TargetFPS{
		get{
			return targetFPS;
		}
		set{
			targetFPS = value;
		}
	}

	private int levelsSinceAd;
	public int LevelsSinceAd{
		get{
			return levelsSinceAd;
		}
		set{
			levelsSinceAd = value;
		}
	}

	public int[] HasStars{
		get{
			return new int[] {1,1,1,1,1,1,1,1,1,1,1,1,1};
		}
	}

	private int[] numStars = {0,0,0,0,0,0,0,0,0,0,0,0,0};
	public int[] NumStars{
		get{
			if(numStars == null){
				numStars = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0};
			}
			for(int i = 0; i <= lastLevel - 4; i++){
				if(numStars[i] == 0){
					numStars[i] = 1;
				}
			}
			return numStars;
		}
	}
	public void setNumStars(int index, int num){
		if(index < 0)
			return;

		if(index < numStars.Length){
			if(num <= 3){
				if(num > numStars[index]){
					numStars[index] = num;
				}
			} else{
				numStars[index] = 3;
			}
		}
	}


	public void resetLevelProgress(){
		lastLevel = -1;
		maxLevel = 0;
	}

	public PlayerData(int lastLevel, int maxLevel){
		this.lastLevel = lastLevel;
		this.maxLevel = maxLevel;
		this.touchPanZoom = false;
		this.music = false;
		this.musicVolume = 1f;
		//this.musicTrack = 1;
		this.targetFPS = 60;
		levelsSinceAd = 0;
	}

	public void print(){
		Debug.Log("Player Data: \nlastLevel:" + lastLevel
			+ "\nmaxLevel:" + maxLevel
			+ "\ntouchPanZoom: " + touchPanZoom
			+ "\nmusic:" + music
			+ "\nmusicVolume:" + musicVolume
			+ "\ntargetFPS:" + targetFPS);
	}
}
