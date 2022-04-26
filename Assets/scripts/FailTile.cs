using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailTile : MonoBehaviour
{
    private ResetButton resetButton;

    // Start is called before the first frame update
    void Start()
    {
        resetButton = GameObject.Find("Reset Button").GetComponent<ResetButton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "robot"){
            col.gameObject.GetComponent<robotD>().reset();
            resetButton.resetScene();
        }
    }
}
