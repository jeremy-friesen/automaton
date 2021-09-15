using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsEnforcer : MonoBehaviour{

    private PlayerData data;

    // Start is called before the first frame update
    void Start(){
        data = SaveSystem.Load();
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
