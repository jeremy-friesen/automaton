using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// Project Info:
  // tile positions are 21 units apart
  // the triggers on the tiles are 5*5 boxes in the center
  // The player-input script is stored at: Application.persistentDataPath + "/script.txt"
  //   - Application.persistentDataPath changes for each platform, so we don't have to 
  //     worry about file system differences

// TODO:
	// - finish idle/throw animations
	// - perfect animation and timing

// Priority Features list:
	// - Register game mechanic
	// - level failure state
	// - object shadows
	// - level intro text
	// - organize workspace, make prefabs

// TODO eventuallly:
	// - reconsider timing, make sure it exits putDown/pickUp animation
	//   before changing direction or moving
	// - add while loops
	// - allow functions to be called before they are defined

// IDEAS:
	// - ground conveyor-belt that moves robot one tile per instruction
	// - moving obstacles
	// - permanent looping as a gameplay mechanic
	// - teach player pathfinding algorithms
	// - level editor
	// - robot power-up objects, instead of objects that need to be moved to
	// 	 another place, to support simpler algorithms
	// - character skins, to unlock with currency gained by completing levels
	//   (or watching ads?)
//

/*public class CodeManager : MonoBehaviour{

	[SerializeField]
	private GameObject robot;
	private robotD robotScript;

	private bool play;

	//command-following variable
	private int currentLineNum;
	private float nextCommandTime;
	List<string> function;
	List<int> functionStarts;
	List<bool> isFunc;
	List<int> lineNumbers;
	List<int> loopsLeft;

	// to visualize code walkthrough
	[SerializeField]
	private textArea textArea;

	// command/fileRead variables
	private string[] script;
	private bool[] line;
  
  void Start(){
		robotScript	= robot.GetComponent<robotD>();

		// initial text file creation
		StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/script.txt", false);
    writer.Write("");
    writer.Close();
		// initial text file read
		readFile();

		reset();
  }

	void reset(){
		play = false;

		currentLineNum = 0;
		nextCommandTime = Time.time;

		isFunc = new List<bool>();
		lineNumbers = new List<int>();
		loopsLeft = new List<int>();

		function = new List<string>();
		functionStarts = new List<int>();
	}

	void FixedUpdate(){
		if(play){
			runCode(script); 
		}
	}

	public void playButton(){
		textArea.RunningLine = currentLineNum;
		//textArea.lineHighlights[0].GetComponent<Image>().enabled = true;
		play = true;

		readFile();
	}

	// reads "script.txt" and puts each line into String array 'script'
	// - this method should be rewritten eventually
	// - possibly rewrite with List type for 'script'
	public void readFile(){
		int counter = 0;
		string curLine;

		System.IO.StreamReader file;	
		file = new System.IO.StreamReader(Application.persistentDataPath + "/script.txt");
		while((curLine = file.ReadLine()) != null){
    	counter++;
		}
		file.Close();
		script = new string[counter];

		counter = 0;
		file = new System.IO.StreamReader(Application.persistentDataPath + "/script.txt");
		while((curLine = file.ReadLine()) != null){
    	script[counter] = curLine;
    	counter++;
		}
		file.Close();
	}

	void runCode(string[] code){
		if(currentLineNum == -1)
			currentLineNum = 0;

		if(Time.time >= (nextCommandTime - 0.05f)){
			robotScript.neutralAnim();
		}

		if(currentLineNum < script.Length && Time.time >= nextCommandTime){
			nextCommandTime = Time.time + 0.5f;
			Debug.Log("currentLineNum = " + currentLineNum + ", script.Length = " + script.Length);
			Debug.Log("line " + currentLineNum + ": " + StringMethods.killWhiteSpace(script[currentLineNum]));
			runLine(StringMethods.killWhiteSpace(script[currentLineNum]));
			currentLineNum++;
			textArea.RunningLine = currentLineNum;
		}else if(currentLineNum >= script.Length && Time.time >= nextCommandTime){
			currentLineNum = -1;
			textArea.RunningLine = -1;
			play = false;
		}
	}

	void runLine(string command){
		if(command == ""){
		} else if(command == "moveForward();"){
			robotScript.moveForward();
		} else if(command == "turnRight();"){
			robotScript.turnRight();
			nextCommandTime = Time.time + 0.4f;
		} else if(command == "turnLeft();"){
			robotScript.turnLeft();
			nextCommandTime = Time.time + 0.4f;
		} else if(command == "pickUp();"){
			nextCommandTime = Time.time + 0.4f;
			robotScript.pickUp();
		} else if(command == "putDown();"){
			nextCommandTime = Time.time + 0.4f;
			robotScript.putDown();
		} else if(StringMethods.cutString(command, 0, 3) == "for"){
			isFunc.Add(false);
			lineNumbers.Add(currentLineNum);
			loopsLeft.Add((int)char.GetNumericValue(command[4]) - 1);
			nextCommandTime = Time.time + 0.25f;
		} else if(StringMethods.killWhiteSpace(command) == "}"){
			if(isFunc[isFunc.Count - 1]){ //PROBLEM HERE
				currentLineNum = lineNumbers[lineNumbers.Count-1];
				textArea.RunningLine = currentLineNum;
				isFunc.RemoveAt(isFunc.Count - 1);
				lineNumbers.RemoveAt(lineNumbers.Count - 1);
				loopsLeft.RemoveAt(loopsLeft.Count - 1);
			} else{
				if(loopsLeft[loopsLeft.Count - 1] > 0){
					loopsLeft[loopsLeft.Count - 1]--;
					currentLineNum = lineNumbers[lineNumbers.Count - 1];
					textArea.RunningLine = currentLineNum;
				} else {
					isFunc.RemoveAt(isFunc.Count - 1);
					lineNumbers.RemoveAt(lineNumbers.Count - 1);
					loopsLeft.RemoveAt(loopsLeft.Count - 1);
				}
			}
			nextCommandTime = Time.time + 0.25f;
		} else if(isFunctionCall(command)){
			isFunc.Add(true);
			lineNumbers.Add(currentLineNum);
			loopsLeft.Add(0);
			funcJump(StringMethods.funcName(command));
			nextCommandTime = Time.time + 0.25f;
		} else if(isFunctionDeclaration(command)){
			function.Add(StringMethods.funcName(command));
			functionStarts.Add(currentLineNum);
			// this next line could be a problem. TODO: test it
			while(script[currentLineNum] != "}"){
				currentLineNum++;
			}
			textArea.RunningLine = currentLineNum;
			nextCommandTime = Time.time + 0.25f;
		} else{
			Debug.Log("Error: string not recognized.");
			nextCommandTime = Time.time;
		}
	}

	// script line jumping
	//void findForLoopStart(){ //Not currently in use
	//	while(StringMethods.cutString(StringMethods.killWhiteSpace(script[currentLineNum]), 0, 3) != "for"){
	//		currentLineNum--;
	//		textArea.RunningLine = currentLineNum;
	//	}
	//}

	void funcJump(string func){
		for(int i = 0; i < function.Count; i++){
			if(func == function[i]){
				currentLineNum = functionStarts[i];
				textArea.RunningLine = currentLineNum;
				return;
			}
		}
	}

	// boolean functions
	bool isFunctionCall(string line){
		for(int i = 0; i < function.Count; i++){
			if(StringMethods.funcName(line) == function[i]){
				return true;
			}
		}
		return false;
	}

	bool isFunctionDeclaration(string line){
		return line[line.Length - 1] == '{';
	}

}*/

