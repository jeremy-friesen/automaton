using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMeta : MonoBehaviour{
	
	private PlayerData data;

	private int lastLevel;
	private int maxLevel;
	private int[] numStars;
	private int[] hasStars;

	[SerializeField]
	private GameObject[] levelButtons = null;

	// Start is called before the first frame update
	void Start(){
		loadLevelProgress();
		Debug.Log(Application.persistentDataPath);
	}

	public void resetLevelProgress(){
		Debug.Log("resetLevelProgress");
		PlayerData data = SaveSystem.Load();
		data.resetLevelProgress();
		SaveSystem.Save(data);
		loadLevelProgress();
	}

	void loadLevelProgress(){
		data = SaveSystem.Load();

		lastLevel = data.LastLevel;
		maxLevel = data.MaxLevel;
		numStars = data.NumStars;
		hasStars = data.HasStars;

		SaveSystem.Save(data);

		for(int i = 0; i < levelButtons.Length; i++){
			if(i <= lastLevel){
				levelButtons[i].GetComponent<LevelButton>().setColor("blue");
			}else{
				levelButtons[i].GetComponent<LevelButton>().setColor("grey");
			}
			if(i <= maxLevel){
				levelButtons[i].GetComponent<Button>().interactable = true;
			}else{
				levelButtons[i].GetComponent<Button>().interactable = false;
			}
		}

		for(int i = 4; i < levelButtons.Length; i++){
			Debug.Log(numStars[i-4]);
			if(hasStars[i-4] == 1){
				if(numStars[i-4] > 0){
					levelButtons[i].GetComponent<LevelButton>().setStars(numStars[i-4]);
				}
			}
		}
	}

	public int getLastLevel(){
		return lastLevel;
	}

	public int getMaxLevel(){
		return maxLevel;
	}
}
