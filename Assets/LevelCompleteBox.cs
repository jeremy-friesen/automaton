using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteBox : MonoBehaviour{

    public void setStars(int numStars){
        if(transform.Find("star 0") != null){
            for(int i = 0; i < 3; i++){
                if(numStars >= i + 1){
                    transform.Find("star " + i).GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("UI/starFilledWhite");
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
