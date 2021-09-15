using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LiveInterpreterParallel{
	private robotD robot;

	//private int numRobots = 1;

	private bool play;

	// command-following variable
	private int currentLineNum;
	private float nextCommandTime;
	List<string> function;
	List<int> functionStarts;
	List<bool> isFunc;
	List<int> lineNumbers;
	List<int> loopsLeft;

	// statistic trackers
	private int operationCount;
	private int lineCount;

	// to visualize code walkthrough
	private ScriptText scriptText;

	// command/fileRead variables
	private string[] script;
	private bool[] line;

	public LiveInterpreterParallel(robotD robot, ScriptText scriptText){
		this.robot = robot;
		this.scriptText = scriptText;
	}
  
  public void Start(){
		// stats
		operationCount = 0;
		lineCount = 0;

		// initial text file creation
		StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/script.txt", false);
    writer.Write("");
    writer.Close();

		// initial text file read
		readFile();

		reset();
  }

	public void reset(){
		play = false;

		currentLineNum = -1;
		nextCommandTime = Time.time;

		isFunc = new List<bool>();
		lineNumbers = new List<int>();
		loopsLeft = new List<int>();

		function = new List<string>();
		functionStarts = new List<int>();

    scriptText.setRunningLineNum(-1);
	}

	public void FixedUpdate(){
		if(play){
			runCode(script);
		}
	}

	// called on play button press
	public void start(){
		
		operationCount = 0;
		scriptText.setRunningLineNum(currentLineNum);
    
		//textArea.lineHighlights[0].GetComponent<Image>().enabled = true;
		play = true;
		readFile();
	}

	public void stop(){
		play = false;
    scriptText.setRunningLineNum(-1);
	}

	// reads "script.txt" and puts each line into String array 'script'
	// - this method should be rewritten eventually
	// - possibly rewrite with List type for 'script'
	public void readFile(){
		lineCount = 0;
		int counter = 0;
		string curLine;

		System.IO.StreamReader file;
		file = new System.IO.StreamReader(Application.persistentDataPath + "/script.txt");
		while((curLine = file.ReadLine()) != null){
			if(curLine != "")
				lineCount++;
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
		
		if(currentLineNum == -1){
			//currentLineNum = 0;
    	scriptText.setRunningLineNum(0);
		}

		if(Time.time >= (nextCommandTime - 0.05f)){
			robot.neutralAnim();
		}

		if(currentLineNum < script.Length && Time.time >= nextCommandTime){
			advanceCode();
		}else if(currentLineNum >= script.Length && Time.time >= nextCommandTime){
			currentLineNum = -1;
      
			stop();
		}
	}

	void advanceCode(){
		if(currentLineNum == script.Length - 1){
			stop();
      scriptText.setRunningLineNum(-1);
			return;
		}
	
		nextCommandTime = Time.time + 0.5f;
		//Debug.Log("currentLineNum = " + currentLineNum + ", script.Length = " + script.Length);
		//Debug.Log("line " + currentLineNum + ": " + StringMethods.killWhiteSpace(script[currentLineNum]));
		if(currentLineNum < script.Length - 1){
			currentLineNum++;
			//Debug.Log("run line " + currentLineNum);
			runLine(StringMethods.killWhiteSpace(script[currentLineNum]));
		}
		if(currentLineNum < script.Length)
			scriptText.setRunningLineNum(currentLineNum);
	}

	void runLine(string command){
		if(command == ""){
			advanceCode();
			/*if(currentLineNum < script.Length - 1)
      	scriptText.setRunningLineNum(currentLineNum + 1);
      nextCommandTime = Time.time;*/
		} else if(command == "moveForward();"){
      operationCount++;
			robot.moveForward();
		} else if(command == "turnRight();"){
      operationCount++;
			robot.turnRight();
			nextCommandTime = Time.time + 0.4f;
		} else if(command == "turnLeft();"){
      operationCount++;
			robot.turnLeft();
			nextCommandTime = Time.time + 0.4f;
		} else if(command == "pickUp();"){
			operationCount++;
			//nextCommandTime = Time.time + 0.4f;
			robot.pickUp();
		} else if(command == "putDown();"){
      operationCount++;
			//nextCommandTime = Time.time + 0.4f;
			robot.putDown();
		} else if(command == "throwItem();"){
      operationCount++;
      nextCommandTime = Time.time + 0.65f;
      robot.throwItem();
		} else if(StringMethods.cutString(command, 0, 2) == "if"){
			isFunc.Add(false);
			lineNumbers.Add(currentLineNum);
			if(StringMethods.cutString(command, 3, 14) == "package.colour"){
				String colour = StringMethods.cutString(command, 19, StringMethods.findFirstOccurance(command, ')') - 19);
				String op = StringMethods.cutString(command, Math.Max(StringMethods.findFirstOccuranceStr(command, "!="), StringMethods.findFirstOccuranceStr(command, "==")), 2);
				if(op == "=="){
					if(robot.getPackageColour() == colour){
						loopsLeft.Add(0);
					} else{
						loopsLeft.Add(-1);
						jumpToCloseBracket();
					}
				} else if(op == "!="){
					if(robot.getPackageColour() == colour){
            loopsLeft.Add(-1);
            jumpToCloseBracket();
					} else{
            loopsLeft.Add(0);
					}
				}
			} else if(StringMethods.cutString(command,3,12) == "robot.colour"){
				String colour = StringMethods.cutString(command, 17, StringMethods.findFirstOccurance(command, ')') - 17);
				String op = StringMethods.cutString(command, Math.Max(StringMethods.findFirstOccuranceStr(command, "!="), StringMethods.findFirstOccuranceStr(command, "==")), 2);
				if(op == "=="){
					//Debug.Log(robot.Colour + " == " + colour);
					if(robot.Colour == colour){
						loopsLeft.Add(0);
					} else{
						loopsLeft.Add(-1);
						jumpToCloseBracket();
					}
				} else if(op == "!="){
					if(robot.Colour == colour){
            loopsLeft.Add(-1);
            jumpToCloseBracket();
					} else{
            loopsLeft.Add(0);
					}
				}
			} else{
				Debug.Log("LiveInterpreterParallel: conditional not related to package.colour");
				loopsLeft.Add(0);
			}
		} else if(StringMethods.cutString(command, 0, 2) == "do"){
      isFunc.Add(false);
      lineNumbers.Add(currentLineNum);
      loopsLeft.Add((int)char.GetNumericValue(command[2]) - 1);
      nextCommandTime = Time.time + 0.25f;
		} else if(StringMethods.cutString(command, 0, 3) == "for"){
			isFunc.Add(false);
			lineNumbers.Add(currentLineNum);
			loopsLeft.Add((int)char.GetNumericValue(command[4]) - 1);
			nextCommandTime = Time.time + 0.25f;
		} else if(StringMethods.killWhiteSpace(command) == "}"){
			if(isFunc.Count > 0 && isFunc[isFunc.Count - 1]){
				currentLineNum = lineNumbers[lineNumbers.Count-1];
				scriptText.setRunningLineNum(currentLineNum);
        
				isFunc.RemoveAt(isFunc.Count - 1);
				lineNumbers.RemoveAt(lineNumbers.Count - 1);
				loopsLeft.RemoveAt(loopsLeft.Count - 1);
			} else{
				if(loopsLeft.Count > 0 && loopsLeft[loopsLeft.Count - 1] > 0){
					loopsLeft[loopsLeft.Count - 1]--;
					currentLineNum = lineNumbers[lineNumbers.Count - 1];
					scriptText.setRunningLineNum(currentLineNum);
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
			Debug.Log("function declaration");
			function.Add(StringMethods.funcName(command));
			functionStarts.Add(currentLineNum);

			jumpToCloseBracket();
			scriptText.setRunningLineNum(currentLineNum);
      
			nextCommandTime = Time.time + 0.25f;
		} else{
			Debug.Log("Error: string not recognized.");
			nextCommandTime = Time.time;
		}
	}

	// script line jumping
	/*void findForLoopStart(){ //Not currently in use
		while(StringMethods.cutString(StringMethods.killWhiteSpace(script[currentLineNum]), 0, 3) != "for"){
			currentLineNum--;
			textArea.RunningLine = currentLineNum;
		}
	}*/

	void jumpToCloseBracket(){
		int numOpenBrackets = 1;
		while(numOpenBrackets > 0){
			currentLineNum++;
			if(script[currentLineNum].Length > 0 && script[currentLineNum][script[currentLineNum].Length - 1] == '{'){
				numOpenBrackets++;
			}
			if(script[currentLineNum] == "}"){
				numOpenBrackets--;
			}
		}
	}

	void funcJump(string func){
		for(int i = 0; i < function.Count; i++){
			if(func == function[i]){
				currentLineNum = functionStarts[i];
				scriptText.setRunningLineNum(currentLineNum);
        
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

	public String getStats(){
		return "You completed the level with:\n" +
       "<color=#00FFFF>" + lineCount + "</color> lines\n" +
       "<color=#00FFFF>" + operationCount +"</color> operations";
	}

	public int getLineCount(){
		return lineCount;
	}

	public int getOperationCount(){
		return operationCount;
	}
}
