using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectIfButton : MonoBehaviour {
	[SerializeField]
	private GameObject objectToFollow = null;

  [SerializeField]
  private Vector3 addVector = new Vector3(-54f, 25f);

	void Start(){
		transform.position = objectToFollow.transform.position + addVector;
	}
	// Update is called once per frame
	void Update(){
		transform.position = objectToFollow.transform.position + addVector;
	}
}
