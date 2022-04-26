using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour{

    [SerializeField]
    private GameObject codeWindow;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft)){
            //Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        if ((Input.deviceOrientation == DeviceOrientation.Portrait)){
            //Screen.orientation = ScreenOrientation.LandscapeRight;
        }
    }
}
