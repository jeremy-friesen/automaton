using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLevelsPositionSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, -228f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
