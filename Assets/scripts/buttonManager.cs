/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Linq;

public class buttonManager : MonoBehaviour {
	//what the text box is currently showing
	[SerializeField]
	private Text codeBox;

	[SerializeField]
	private CodeManager codeManagerScript;

	// array to store input script
	private List<string> text;

	// "|" cursor toggle information
	private bool cursorToggle;
	private float nextToggleTime;

	// indentation level
	private List<int> indentLevel;

	// cursor line number
	private int lineNumber;
	private int topDisplayedLine;

	//line number limit
	[SerializeField]
	private bool lineLimit;
	[SerializeField]
	private int maxLine;

	// Use this for initialization
	void Start(){
		// Stores user-made code
		text = new List<string>();

		// text postion cursor flashing
		cursorToggle = true;
		nextToggleTime = Time.time + 0.5f;

		lineNumber = -1;
		topDisplayedLine = 0;

		indentLevel = new List<int>();
	}

	public void resetButton(){
		// Destroys all emitted packages
		GameObject[] packages = GameObject.FindGameObjectsWithTag("package");
		foreach(GameObject package in packages){
			package.GetComponent<package>().reset();
		}
		// Each shoot emits a new package
		GameObject[] shoots = GameObject.FindGameObjectsWithTag("shoot");
		foreach(GameObject shoot in shoots){
			shoot.GetComponent<Shoot>().emitPackage();
		}
	}

	public void codeLineTap(int line){
		// if tapped line has text
		if(text.Count > line){
			lineNumber = line;
			cursorToggle = true;
			nextToggleTime = Time.time + 0.5f;
		}else{
			lineNumber = text.Count - 1;
			cursorToggle = true;
			nextToggleTime = Time.time + 0.5f;
		}
	}

	public void playButton(){
		String outputString = "";
		foreach(string line in text){
			outputString = outputString + line + "\n";
		}

    //Write some text to the script.txt file, the boolean is to append
    StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/script.txt", false);
    writer.Write(outputString);
    writer.Close();

		codeManagerScript.playButton();
	}

	public void insertLine(String line){
		//code size limit reached case
		if(lineLimit && (lineNumber+1 > maxLine))
			return;

		text.Insert(lineNumber + 1, line);

		//Indent Level
		if(indentLevel.Count > 0){
		} else{
			indentLevel.Insert(lineNumber + 1, 0);
		}

		//for(int i = 0; i < indentLevel[lineNumber + 1]; i++){
		//	text[lineNumber + 1] = "  " + text[lineNumber + 1];
		//}

		indentLevel.Insert(lineNumber + 2, indentLevel[lineNumber + 1]); // add this line to other methods
		downButton();
	}

	public void forButton(int loops){
		//code size limit reached case
		if(lineLimit && (lineNumber+1 > maxLine))
			return;

		//lineNumber++;
		text.Insert(lineNumber + 1, "for(" + loops + "){");

		//Indent Level
		if(indentLevel.Count > 0){
			//indentLevel.Insert(lineNumber + 1, indentLevel[lineNumber + 1]);
		} else{
			indentLevel.Insert(lineNumber + 1, 0);
		}

		//for(int i = 0; i < indentLevel[lineNumber + 1]; i++){
		//	text[lineNumber + 1] = "  " + text[lineNumber + 1];
		//}

		indentLevel.Insert(lineNumber + 2, indentLevel[lineNumber + 1] + 1); // add this line to other methods
		
		downButton();
	}

	public void functionButton(){
		//code size limit reached case
		if(lineLimit && (lineNumber+1 > maxLine))
			return;
		
		int num = 1;
		foreach(string line in text){
			if(cutString(killWhiteSpace(line), 0, 4) == "func"){
				num = (int)Char.GetNumericValue(line[4]) + 1;
			}
		}

		text.Insert(lineNumber + 1, "func" + num.ToString() + "(){");

		if(indentLevel.Count > 0){
			//indentLevel.Insert(lineNumber + 1, indentLevel[lineNumber + 1]);
		} else{
			indentLevel.Insert(lineNumber + 1, 0);
		}

		//for(int i = 0; i < indentLevel[lineNumber + 1]; i++){
		//	text[lineNumber + 1] = "  " + text[lineNumber + 1];
		//}

		indentLevel.Insert(lineNumber + 2, indentLevel[lineNumber + 1] + 1);
		downButton();
	}

	public void functionCallButton(int funcNum){
		//code size limit reached case
		if(lineLimit && (lineNumber+1 > maxLine))
			return;

		text.Insert(lineNumber + 1, "func" + funcNum + "();");

		//BUG:
		//because of the following code, two indentlevels will be added
		//Indent Level
		if(indentLevel.Count > 0){
			//indentLevel.Insert(lineNumber + 1, indentLevel[lineNumber + 1]);
		} else{
			indentLevel.Insert(lineNumber + 1, 0);
		}

		//for(int i = 0; i < indentLevel[lineNumber + 1]; i++){
		//	text[lineNumber + 1] = "  " + text[lineNumber + 1];
		//}

		indentLevel.Insert(lineNumber + 2, indentLevel[lineNumber + 1]);
		downButton();
	}

	public void upButton(){
		if(lineNumber > 0){
			lineNumber--;
			cursorToggle = true;
			nextToggleTime = Time.time + 0.5f;
			if(lineNumber < topDisplayedLine){
				topDisplayedLine--;
			}
		}
	}

	public void downButton(){
		if(lineNumber < text.Count - 1){
			lineNumber++;
			cursorToggle = true;
			nextToggleTime = Time.time + 0.5f;
			if(lineNumber > topDisplayedLine + 11){
				topDisplayedLine++;
			}
		}
	}

	public void backSpaceButton(){
		//Debug.Log("pre|lineNumber: " + lineNumber + ", text.Count: " + (text.Count) + ", indentLevel.Count: " + (indentLevel.Count));
		if(text.Count == 0){
			return;
		}

		if(text[lineNumber][text[lineNumber].Length - 1] == '{'){
			for(int i = lineNumber + 1; i < indentLevel.Count; i++){
				if(indentLevel[i] > 0)
					indentLevel[i]--;
			}
		} else if(killWhiteSpace(text[lineNumber]) == "}" && lineNumber < (text.Count - 1)){
			for(int i = lineNumber + 1; i < indentLevel.Count; i++){
				indentLevel[i]++;
			}
		}
		text.RemoveAt(lineNumber);
		indentLevel.RemoveAt(lineNumber);

		if(lineNumber > text.Count - 1 || lineNumber > indentLevel.Count - 1)
			lineNumber--;

		//Debug.Log("post|lineNumber: " + lineNumber + ", text.Count: " + (text.Count) + ", indentLevel.Count: " + (indentLevel.Count));
	}

	public void closeBracket(){
		//code size limit reached case
		if(lineLimit && (lineNumber+1 > maxLine)){
			return;
		}
			
		text.Insert(lineNumber + 1, "}");

		if(indentLevel.Count > 0){
			indentLevel[lineNumber + 1]--;
		} else{
			indentLevel.Insert(lineNumber + 1, 0);
		}

		indentLevel.Insert(lineNumber + 2, indentLevel[lineNumber + 1]);
		downButton();
	}

	// Update is called once per frame
	void Update(){
		if(text.Count > 12){
				codeBox.text = "";
				//FIX: if topDisplayedLine + 12 > text.Count
				for(int i = topDisplayedLine; i < topDisplayedLine + 12; i++){
					// line numbering/spacing
					codeBox.text = codeBox.text + i;
					if(i < 10){
						codeBox.text = codeBox.text + " ";
					}
					codeBox.text = codeBox.text + " ";

					// indentation
					for(int j = 0; j < indentLevel[i]; j++){
						codeBox.text = codeBox.text + "  ";
					}

					// code
					codeBox.text = codeBox.text + text[i];

					// cursor toggle
					if(cursorToggle){
						if((i == lineNumber) || (i == 0 && lineNumber == -1)){
							codeBox.text = codeBox.text + "|";
						}
					}
					codeBox.text = codeBox.text + "\n";
				}

		} else{
			codeBox.text = "";

			for(int i = 0; i < text.Count; i++){
				// line numbering
				codeBox.text = codeBox.text + i;
				if(i < 10){
					codeBox.text = codeBox.text + "";
				}
				codeBox.text = codeBox.text + " ";

				// indentation
				for(int j = 0; j < indentLevel[i]; j++){
					codeBox.text = codeBox.text + "  ";
				}

				// code
				codeBox.text = codeBox.text + text[i];

				// cursor toggle
				if(cursorToggle){
					if((i == lineNumber) || (i == 0 && lineNumber == -1)){
						codeBox.text = codeBox.text + "|";
					}
				}
				codeBox.text = codeBox.text + "\n";
			}

			//fill in the rest of the lines
			for(int i = text.Count; i < 12; i++){
				// line numbering & spacing
				codeBox.text = codeBox.text + i;
				if(i < 10){
					codeBox.text = codeBox.text + "";
				}
				codeBox.text = codeBox.text + " ";

				// cursor toggle
				if(cursorToggle){
					if((i == lineNumber) || (i == 0 && lineNumber == -1)){
						codeBox.text = codeBox.text + "|";
					}
				}

				codeBox.text = codeBox.text + "\n";
			}
		}

		if(Time.time > nextToggleTime){
			cursorToggle = !cursorToggle;
			nextToggleTime = Time.time + 0.5f;
		}
	}

	string killWhiteSpace(string str){
		string newStr = "";
		for(int i = 0; i < str.Length; i++){
			if(str[i] != ' '){
				newStr = newStr + str[i];
			}
		}
		return newStr;
	}

	string cutString(string str, int start, int length){
		if(str.Length < start + length){
			length = str.Length;
		}
		string newString = "";
		for(int i = start; i < start + length; i++){
			newString = newString + str[i];
		}

		return newString;
	}
}
*/