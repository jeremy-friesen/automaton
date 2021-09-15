using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class levelCompleteTile : MonoBehaviour{
	[SerializeField]
  private LevelManager lvlManager = null;

  void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "robot"){
			lvlManager.setTileReached();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "robot"){
			lvlManager.setTileNotReached();
		}
	}

	public void packagePutDown(){
		lvlManager.setPackageSetOnTile();
	}
}