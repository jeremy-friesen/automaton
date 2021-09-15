using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	[SerializeField]
	private string colour = "yellow";

	// Use this for initialization
	void Start(){
		emitPackage();
	}

	public void emitPackage(){
		if(colour == "red"){
			Instantiate((GameObject)Resources.Load("redPackage"), transform.position + new Vector3(1.0f, 0f, 0f), Quaternion.identity);
		} else{
			Instantiate((GameObject)Resources.Load("yellowPackage"), transform.position + new Vector3(1.0f, 0f, 0f), Quaternion.identity);
		}
	}
}
