using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour{
    private GameObject[] shoots;
    private LiveInterpreter liveInterpreter;
    private LiveInterpreterManager liveInterpreterManager;
    private GameObject[] robots;

    // Start is called before the first frame update
    void Start(){
        shoots = GameObject.FindGameObjectsWithTag("shoot");
        liveInterpreter = GameObject.FindGameObjectsWithTag("interpreter")[0].GetComponent<LiveInterpreter>();
        liveInterpreterManager = GameObject.FindGameObjectsWithTag("interpreter")[0].GetComponent<LiveInterpreterManager>();
        robots = GameObject.FindGameObjectsWithTag("robot");
    }

    // Update is called once per frame
    void Update(){
        
    }
 
    public void resetScene(){
        foreach (GameObject robot in robots){
            robot.GetComponent<robotD>().reset();
        }
        // stop the live interpreter
        if(liveInterpreter != null){
            liveInterpreter.reset();
        } else {
            liveInterpreterManager.reset();
        }
        // reset each package
        GameObject[] packages = GameObject.FindGameObjectsWithTag("package");
        foreach (GameObject package in packages){
            package.GetComponent<package>().reset();
        }
        // each shoot emit new package
        foreach (GameObject shoot in shoots){
            shoot.GetComponent<Shoot>().emitPackage();
        }
    }
}
