using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followObject : MonoBehaviour {
	[SerializeField]
	private GameObject objectToFollow = null;

	[SerializeField]
	private Vector3 addVector = new Vector3(-27.6f, 17.8f);

	void Start(){
    gameObject.GetComponent<RectTransform>().localPosition = objectToFollow.GetComponent<RectTransform>().localPosition + addVector;
		//transform.position = objectToFollow.transform.position + new Vector3(-27.6f, 17.8f);
	}
	// Update is called once per frame
	void Update(){
    gameObject.GetComponent<RectTransform>().position = objectToFollow.GetComponent<RectTransform>().position + addVector;
		//Debug.Log(gameObject.GetComponent<RectTransform>().position);
		//Debug.Log(objectToFollow.GetComponent<RectTransform>().position);
    //gameObject.GetComponent<RectTransform>().anchoredPosition = objectToFollow.GetComponent<RectTransform>().anchoredPosition;
    //transform.position = objectToFollow.transform.position + new Vector3(-27.6f, 17.8f);
  }
}
