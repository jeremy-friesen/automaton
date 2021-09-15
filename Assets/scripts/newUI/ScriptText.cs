using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class ScriptText : MonoBehaviour{
	[SerializeField]
	private GameObject[] lines = null;
	private int selectedLineNum;

	private int runningLineNum;

	private bool[] takenFuncNums;

	[SerializeField]
	private LiveInterpreter liveInterpreter = null;

	[SerializeField]
	private LiveInterpreterManager liveInterpreterManager = null;

  private Scene scene;

	public int getRunningLineNum(){
		return runningLineNum;
	}

	public void setRunningLineNum(int runningLineNum){
		////Debug.Log("running line set to " + runningLineNum);
		int lastRunningLineNum = this.runningLineNum;
		this.runningLineNum = runningLineNum;
		if(lastRunningLineNum != -1)
			lines[lastRunningLineNum].GetComponent<ScriptLine>().setHighlighted(false);
		if(runningLineNum != -1)
      lines[runningLineNum].GetComponent<ScriptLine>().setHighlighted(true);
	}

	// run at start of scene
	void Start(){
		//Debug.Log("Application file path: " + Application.persistentDataPath)

		selectedLineNum = 0;
		takenFuncNums = new bool[9];

		// get active scene
    scene = SceneManager.GetActiveScene();

		// load in saved script
		loadScript();
	}

	// load in saved script and set ScriptLine values to each line
	public void loadScript(){
		// read file for script
		string[] scriptArray = readFile();
		Debug.Log("script loaded, content: ");
		for(int i = 0; i < scriptArray.Length; i++){
			Debug.Log(scriptArray[i]);
		}
		//Debug.Log("finished loadScript");
		// put script in level
		for(int i = 0; i < scriptArray.Length; i++){
			Debug.Log("58");
			Debug.Log(scriptArray[i].Length);
			if(scriptArray[i].Length > 0 && scriptArray[i][scriptArray[i].Length-1] == '{'){
				Debug.Log("line 60");
				if(scriptArray[i].Length >= 4 && scriptArray[i].Substring(0,4) == "func"){
          Debug.Log("line 62");
					//Debug.Log("attempting to add function definition");
					attemptAddLine(scriptArray[i]);
					int funcNum = Convert.ToInt32(Char.GetNumericValue(scriptArray[i][4]));
					Debug.Log("funcNum = " + funcNum);
					takenFuncNums[funcNum-1] = true;
					refreshFollowingIndents();
				} else if(scriptArray[i].Length >= 2 && scriptArray[i].Substring(0,2) == "do"){
          Debug.Log("line 69");
					//Debug.Log("attempting to add do loop");
					attemptAddLine(scriptArray[i]);
					//Debug.Log("attempting refresh");
          refreshFollowingIndents();
					//Debug.Log("done refreshing indents");
				} else if(scriptArray[i].Length >= 2 && scriptArray[i].Substring(0, 2) == "if"){
					attemptAddLine(scriptArray[i]);

					refreshFollowingIndents();
				} else{
          Debug.Log("line 76");
					//Debug.Log("Unexpected value: " + scriptArray[i]);
				}
        Debug.Log("line 79");
        ////Debug.Log("line 65");
			} else if(scriptArray[i] == "}"){
        attemptAddLine(scriptArray[i]);
				refreshFollowingIndents();
			} else if(scriptArray[i] != ""){
        Debug.Log("line 82");
        ////Debug.Log("line 67");
				attemptAddLine(scriptArray[i]);
			}
      Debug.Log("line 86");
      ////Debug.Log("line 70");
		}
    Debug.Log("line 89");
		////Debug.Log("finished \"put script in level\"");
	}

	public string[] readFile(){
		string[] scriptArray;
		Debug.Log("89");
		// if file exists
		if(File.Exists(Application.persistentDataPath + "/" + scene.name + "Script.txt")){
			Debug.Log("File Exists: " + Application.persistentDataPath + "/" + scene.name + "Script.txt");
      int lineCount = 0;
      int counter = 0;
      string curLine;
      Debug.Log("96");

      System.IO.StreamReader file;
      file = new System.IO.StreamReader(Application.persistentDataPath + "/" + scene.name + "Script.txt");
      while ((curLine = file.ReadLine()) != null)
      {
				Debug.Log("curLine: " + curLine);
        if (curLine != "")
          lineCount++;
        counter++;
      }
      file.Close();
      scriptArray = new string[counter];
      Debug.Log("108");

      counter = 0;
      file = new System.IO.StreamReader(Application.persistentDataPath + "/" + scene.name + "Script.txt");
      while ((curLine = file.ReadLine()) != null)
      {
        Debug.Log("curLine: " + curLine);
        scriptArray[counter] = curLine;
        counter++;
      }
      ////Debug.Log("here");
      file.Close();
      Debug.Log("119");
		} else{
			scriptArray = new string[0];
		}
    Debug.Log("123");
		return scriptArray;
	}
	
	// return the text from each line as a string array
	public string[] getText(){
		string[] text = new string[lines.Length];
		for(int i = 0; i < lines.Length; i++){
			text[i] = lines[i].GetComponent<ScriptLine>().getText();
		}
		return text;
	}

	// attempt to add line to script, return bool representing success
	public bool attemptAddLine(string lineToAdd){
		// if selected line has content
		if(lines[selectedLineNum].GetComponent<ScriptLine>().getHasContent()){
			//Debug.Log("attemptAddLine");
			// if selected line is not the last line
			if(selectedLineNum < lines.Length - 1){
				// if the following line has content
				if(lines[selectedLineNum + 1].GetComponent<ScriptLine>().getHasContent()){
					if(attemptAddLineBefore(selectedLineNum + 1, lineToAdd)){
						onCodeChange();
						return true;
					}
				} else {
          int indent = lines[selectedLineNum].GetComponent<ScriptLine>().getNextLineIndent();
					setLine(selectedLineNum + 1, lineToAdd, true, indent);
					advanceSelected();

          onCodeChange();
					return true;
				}
			}
		// else selected line has no content
		} else {
      ////Debug.Log("attemptAddLine else");
			// not first line
			if(selectedLineNum > 0){
        ////Debug.Log("attemptAddLine else 150");
				int indent = lines[selectedLineNum - 1].GetComponent<ScriptLine>().getNextLineIndent();
        ////Debug.Log("attemptAddLine else 152");
        setLine(selectedLineNum, lineToAdd, true, indent);
        ////Debug.Log("attemptAddLine else 154");
        onCodeChange();
				return true;
        ////Debug.Log("attemptAddLine else 156");
			// first line
			} else{
        ////Debug.Log("attemptAddLine else 159");
				setLine(selectedLineNum, lineToAdd, true);
        ////Debug.Log("attemptAddLine else 161");
        onCodeChange();
				return true;
			}
		}
		return false;
	}

	public bool attemptAddFunctionDeclaration(){
		int index = 0;
		for(int i = 0; i < 9; i++){
			if(!takenFuncNums[i]){
				index = i + 1;
				break;
			}
		}	
		if(index > 0){
			if(attemptAddLineAndBracket("func" + index.ToString() + "(){")){
				takenFuncNums[index-1] = true;

        onCodeChange();
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	private void onCodeChange(){
    exportScript();
		if(liveInterpreter != null){
    	liveInterpreter.onCodeChange();
		} else if(liveInterpreterManager != null){
			liveInterpreterManager.onCodeChange();
		}
	}

	// for loop/function addition
	public bool attemptAddLineAndBracket(string lineToAdd){
		if(!attemptAddLine(lineToAdd))
			return false;
		if(attemptAddLine("}"))
    	setSelected(selectedLineNum - 1);
		refreshFollowingIndents();

		onCodeChange();
		return true;
	}

	// set the contents of a line
	private void setLine(int position, string text, bool hasContent){
		//Debug.Log(("setLine 200: " + lines[position].GetComponent<ScriptLine>()) + " " + position);
		lines[position].GetComponent<ScriptLine>().setText(text);
    ////Debug.Log("setLine 202");
		lines[position].GetComponent<ScriptLine>().setHasContent(hasContent);
    ////Debug.Log("setLine 204");
		if(position == 0){
      ////Debug.Log("setLine 206");
			lines[position].GetComponent<ScriptLine>().setIndent(0);
      ////Debug.Log("setLine 208");
		} else{
      ////Debug.Log("setLine 210");
			lines[position - 1].GetComponent<ScriptLine>().getNextLineIndent();
      ////Debug.Log("setLine 212");
		}
	}

	// set line contents and indentation
  private void setLine(int position, string text, bool hasContent, int indent){
    lines[position].GetComponent<ScriptLine>().setText(text);
    lines[position].GetComponent<ScriptLine>().setHasContent(hasContent);
    lines[position].GetComponent<ScriptLine>().setIndent(indent);
  }

	// attempt to add line before given line position
	private bool attemptAddLineBefore(int position, string lineToAdd){
		if(lines[lines.Length - 1].GetComponent<ScriptLine>().getHasContent()){
			return false;
		} else {
			int pushBackIndent = lines[position - 1].GetComponent<ScriptLine>().getNextLineIndent();
			if(pushBackIndent > 0 && lineToAdd == "}"){
				pushBackIndent--;
			} else if(lineToAdd.Length > 0 && lineToAdd[lineToAdd.Length - 1] == '}'){
				pushBackIndent++;
			}
			pushBackLineContent(position, pushBackIndent);
      int indent = lines[position - 1].GetComponent<ScriptLine>().getNextLineIndent();
			lines[position].GetComponent<ScriptLine>().setText(lineToAdd);
			lines[position].GetComponent<ScriptLine>().setHasContent(true);
      lines[position].GetComponent<ScriptLine>().setIndent(indent);
			advanceSelected();
			refreshFollowingIndents();
			return true;
		}
	}

	// this function assumes the list has enough capacity to
	// push line contents back. don't use without checking
	private void pushBackLineContent(int position, int newIndent){
		string lineContent = lines[position].GetComponent<ScriptLine>().getText();
		bool hasContent = lines[position].GetComponent<ScriptLine>().getHasContent();
    int indent = newIndent;
		lines[position].GetComponent<ScriptLine>().setText("");
		lines[position].GetComponent<ScriptLine>().setHasContent(false);
    lines[position].GetComponent<ScriptLine>().setIndent(0);
		for(int i = position + 1; i < lines.Length; i++){
			string tempLineContent = lines[i].GetComponent<ScriptLine>().getText();
			bool tempHasContent = lines[i].GetComponent<ScriptLine>().getHasContent();
      int tempIndent = lines[i-1].GetComponent<ScriptLine>().getNextLineIndent();
			setLine(i, lineContent, hasContent, indent);
			lineContent = tempLineContent;
			hasContent = tempHasContent;
			indent = tempIndent;
		}
	}

	// move all lines up from this position and down
	private void pullUpLineContent(int position){
		String text = lines[lines.Length-1].GetComponent<ScriptLine>().getText();
		bool hasContent = lines[lines.Length-1].GetComponent<ScriptLine>().getHasContent();
		for(int i = lines.Length - 2; i >= position; i--){
			String tempText = lines[i].GetComponent<ScriptLine>().getText();
			bool tempHasContent = lines[i].GetComponent<ScriptLine>().getHasContent();
			setLine(i, text, hasContent);
			text = tempText;
			hasContent = tempHasContent;
		}
		setLine(lines.Length - 1, "", false);
	}

  private void advanceSelected(){
    lines[selectedLineNum].GetComponent<ScriptLine>().deselect();
    selectedLineNum++;
    lines[selectedLineNum].GetComponent<ScriptLine>().select();
  }

  public void setSelected(int newSelectedElement){
    lines[selectedLineNum].GetComponent<ScriptLine>().deselect();
    selectedLineNum = newSelectedElement;
    lines[selectedLineNum].GetComponent<ScriptLine>().select();
  }

	// writes script contents to file: [Application.persistentDataPath]/script.txt
	public void exportScript(){
		string outputString = "";
    foreach(GameObject line in lines){
      outputString = outputString + line.GetComponent<ScriptLine>().getText() + "\n";
    }

    //Write some text to the script.txt file, the boolean is to append
    StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/script.txt", false);
    writer.Write(outputString);
    writer.Close();
	}

	public void backSpace(){
		// current line has content
		if(lines[selectedLineNum].GetComponent<ScriptLine>().getHasContent()){
			ScriptLine scriptLine = lines[selectedLineNum].GetComponent<ScriptLine>();
			if(scriptLine.getText().Length >= 4 && scriptLine.getText().Substring(0,4) == "func"
					&& scriptLine.getText()[scriptLine.getText().Length - 1] == '{'){
        String t = lines[selectedLineNum].GetComponent<ScriptLine>().getText();
				String n = t.Substring(4,1);
				int num = int.Parse(n);
				takenFuncNums[num - 1] = false;
			}
      scriptLine.setText("");
      scriptLine.setHasContent(false);
			scriptLine.select();

      //jump to previous line
			if(selectedLineNum != 0){
        setSelected(selectedLineNum - 1);
        pullUpLineContent(selectedLineNum + 1);
			} else{
				//pullUpLineContent(0);
				//setSelected(0);
			}

		// current line has no content and is not the first line
		} else if(selectedLineNum != 0){
      // jump to previous line:
      setSelected(selectedLineNum - 1);
      pullUpLineContent(selectedLineNum + 1);
			// jump to previous line and erase it:
			/*lines[selectedLineNum].GetComponent<ScriptLine>().deselect();
			selectedLineNum--;
			lines[selectedLineNum].GetComponent<ScriptLine>().setHasContent(false);
			lines[selectedLineNum].GetComponent<ScriptLine>().setText("");
			lines[selectedLineNum].GetComponent<ScriptLine>().select();*/

		// current line is line 0 and has no contents
		} else {
      pullUpLineContent(selectedLineNum);
		}
		refreshFollowingIndents();
    lines[selectedLineNum].GetComponent<ScriptLine>().select();

    onCodeChange();
	}

  // refresh indents on all lines after
  private void refreshFollowingIndents(){
    for (int i = selectedLineNum; i < lines.Length; i++){
      if (i > 0){
				//Debug.Log("refreshing: i = " + i);
        lines[i].GetComponent<ScriptLine>().setIndent(lines[i - 1].GetComponent<ScriptLine>().getNextLineIndent());
      }
    }
	}
}
