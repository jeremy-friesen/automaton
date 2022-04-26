using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class IfCreationBar : MonoBehaviour{
    [SerializeField]
    GameObject propertyButton = null;
    [SerializeField]
    GameObject operatorButton = null;
    [SerializeField]
    GameObject valueButton = null;

    private String property;
    public String Property{
        get{
            return property;
        }
        set{
            propertyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + value + " button");
            propertyButton.transform.GetChild(0).gameObject.SetActive(false);
            property = value;
        }
    }
    private String op;
    public String Op{
        get{
            return op;
        }
        set{
            operatorButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + value + " button");
            operatorButton.transform.GetChild(0).gameObject.SetActive(false);
            op = value;
        }
    }
    private String cmpValue;
    public String Value{
        get{
            return cmpValue;
        }
        set{
            valueButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + value + " button");
            valueButton.transform.GetChild(0).gameObject.SetActive(false);
            cmpValue = "\"" + value + "\"";
        }
    }

    [SerializeField]
    ButtonManager buttonManager = null;

    // Start is called before the first frame update
    void Start(){
        property = "package.colour";
        op = "==";
        cmpValue = "\"yellow\"";
    }

    public void togglePropertyBar(){
        if(propertyButton.transform.GetChild(0).gameObject.activeSelf){
            propertyButton.transform.GetChild(0).gameObject.SetActive(false);
        } else{
            // activate property bar
            propertyButton.transform.GetChild(0).gameObject.SetActive(true);
            // deactivate other bars
            operatorButton.transform.GetChild(0).gameObject.SetActive(false);
            valueButton.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void toggleOperatorBar(){
        if(operatorButton.transform.GetChild(0).gameObject.activeSelf){
            operatorButton.transform.GetChild(0).gameObject.SetActive(false);
        } else{
            // activate operator bar
            operatorButton.transform.GetChild(0).gameObject.SetActive(true);
            // deactivate other bars
            propertyButton.transform.GetChild(0).gameObject.SetActive(false);
            valueButton.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void toggleValueBar(){
        if(valueButton.transform.GetChild(0).gameObject.activeSelf){
            valueButton.transform.GetChild(0).gameObject.SetActive(false);
        } else{
            // activate value bar
            valueButton.transform.GetChild(0).gameObject.SetActive(true);
            // deactivate other bars
            operatorButton.transform.GetChild(0).gameObject.SetActive(false);
            propertyButton.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void insertIfStatement(){
        buttonManager.ifButton(property, op, cmpValue);
    }

    // Update is called once per frame
    void Update(){
        
    }
}
