using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Linq;

public class ButtonManager : MonoBehaviour {
	[SerializeField]
	private ScriptText scriptText = null;

	//[SerializeField]
	//private CodeManager codeManagerScript;

	// Use this for initialization
	void Start(){
	}

	// Update is called once per frame
	void Update(){
	}

	public void insertLine(String line){
		scriptText.attemptAddLine(line);
	}

	public void doButton(int loops){
		scriptText.attemptAddLineAndBracket("do " + loops + " {");
	}

	public void forButton(int loops){
		scriptText.attemptAddLineAndBracket("for(" + loops + "){");
	}

	public void ifButton(String property, String op, String value){
		scriptText.attemptAddLineAndBracket("if(" + property + " " + op + " " + value + "){");
	}

	public void functionButton(){
    scriptText.attemptAddFunctionDeclaration();
	}

	public void functionCallButton(int num){
    scriptText.attemptAddLine("func" + num + "();");
	}

	public void closeBracket(){
		scriptText.attemptAddLine("}");
	}
}
