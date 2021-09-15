using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopMovement : MonoBehaviour
{
    private string position = "down";

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("moveVertical", 1.5f, 1.5f);
    }

    void moveVertical(){
        if(position == "down"){
            transform.Translate(Vector3.up);
            position = "up";
        } else if(position == "up"){
            transform.Translate(Vector3.down);
            position = "down";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
