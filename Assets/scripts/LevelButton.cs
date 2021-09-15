using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour{
	[SerializeField]
	private String number = null;
	
	public void setColor(String c){
		if(c == "blue"){
			this.GetComponent<Image>().sprite 
					= Resources.Load<Sprite>("NumberButtons\\" + number + "ButtonDarkHighlighted");
		} else if(c == "grey"){
			this.GetComponent<Image>().sprite 
					= Resources.Load<Sprite>("NumberButtons\\" + number + "ButtonDark");
		} else if(c == "pink"){
			this.GetComponent<Image>().sprite 
					= Resources.Load<Sprite>("NumberButtons\\" + number + "ButtonDarkHighlightedPink");
		} else{
			Debug.Log("LevelButton: setColor called with incorrect parameter \"" + c + "\"");
		}
	}

	public void setStars(int numStars){
		if(numStars > 0){
			GameObject parentStar = transform.Find("star").gameObject;
			parentStar.GetComponent<Image>().sprite
				= Resources.Load<Sprite>("UI\\starFilledWhite");
			if(numStars >= 2){
				parentStar.transform.Find("star (1)").gameObject.GetComponent<Image>().sprite
					= Resources.Load<Sprite>("UI\\starFilledWhite");
			}
			if(numStars >= 3){
				parentStar.transform.Find("star (2)").gameObject.GetComponent<Image>().sprite
					= Resources.Load<Sprite>("UI\\starFilledWhite");
			}
		}
	}
}
