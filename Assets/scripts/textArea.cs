using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class textArea : MonoBehaviour {
	public GameObject[] lineHighlights = new GameObject[12];
	private int runningLine;
	public int RunningLine{
		get{return runningLine;}
		set{runningLine = value;}
	}
	private int highlightedLine;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < lineHighlights.Length; i++){
			lineHighlights[i].GetComponent<Image>().enabled = true;
			lineHighlights[i].GetComponent<Image>().enabled = false;
		}
		lineHighlights[0].GetComponent<Image>().enabled = true;
		lineHighlights[0].GetComponent<Image>().enabled = false;
	}

	// Code Walk-through
	void Update(){
		if(highlightedLine != runningLine && lineHighlights.Length > runningLine){
			if(runningLine >= 0){
				if(highlightedLine >= 0)
					lineHighlights[highlightedLine].GetComponent<Image>().enabled = false;
				lineHighlights[runningLine].GetComponent<Image>().enabled = true;
				highlightedLine = runningLine;
			}else{
				lineHighlights[highlightedLine].GetComponent<Image>().enabled = false;
				highlightedLine = runningLine;
			}
		}
	}
}