using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LiveInterpreterManager : MonoBehaviour{
    [SerializeField]
    private robotD[] robots = null;

    [SerializeField]
	private ScriptText scriptText = null;

    [SerializeField]
    private int threeStarsLine = 1;

    [SerializeField]
    private int twoStarsLine = 2;

    [SerializeField]
    private int oneStarLine = 3;

    private LiveInterpreterParallel[] interpreters;

    void Start(){
        interpreters = new LiveInterpreterParallel[robots.Length];
        for(int i = 0; i < robots.Length; i++){
            interpreters[i] = new LiveInterpreterParallel(robots[i], scriptText);
            interpreters[i].Start();
        }
    }

    // temporary workaround to objects not having FixedUpdate due to not being MonoBehaviour's
    void FixedUpdate(){
        for(int i = 0; i < robots.Length; i++){
            interpreters[i].FixedUpdate();
        }
    }

    // called on play button press
	public void start(){
        for(int i = 0; i < robots.Length; i++){
            interpreters[i].start();
        }
	}

	public void stop(){
        for(int i = 0; i < robots.Length; i++){
            interpreters[i].stop();
        }
	}

    public void reset(){
        for(int i = 0; i < robots.Length; i++){
            interpreters[i].reset();
        }
        //Debug.Log("reset");
	}

    public String getStats(){
        int lineCount = interpreters[0].getLineCount();
        int operationCount = 0;
        for(int i = 0; i < robots.Length; i++){
            operationCount += interpreters[i].getOperationCount();
        }
		return "You completed the level with:\n" +
       "<color=#00FFFF>" + lineCount + "</color> lines\n" +
       "<color=#00FFFF>" + operationCount +"</color> operations";
	}

    public int getNumStars(){
        int lineCount = interpreters[0].getLineCount();
        if(lineCount <= threeStarsLine){
            return 3;
        } else if(lineCount <= twoStarsLine){
            return 2;
        } else if(lineCount <= oneStarLine){
            return 1;
        }
        return -1;
    }

    public void onCodeChange(){
        int lineCount = 0;
        int counter = 0;
        string curLine;

        System.IO.StreamReader file;
        file = new System.IO.StreamReader(Application.persistentDataPath + "/script.txt");
        while ((curLine = file.ReadLine()) != null){
            if (curLine != "")
                lineCount++;
            counter++;
        }
        file.Close();
        string[] script = new string[counter];

        counter = 0;
        file = new System.IO.StreamReader(Application.persistentDataPath + "/script.txt");
        while ((curLine = file.ReadLine()) != null){
            script[counter] = curLine;
            counter++;
        }
        file.Close();

        // copy script to level script, to save for next time.
        Scene scene = SceneManager.GetActiveScene();
        System.IO.StreamWriter writeFile;
        writeFile = new System.IO.StreamWriter(Application.persistentDataPath + "/" + scene.name + "Script.txt");
        foreach (string line in script){
            writeFile.WriteLine(line);
        }
        writeFile.Flush();
        writeFile.Close();
    }
}
