using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

// Project Info:
// tile positions are 21 units apart
// the colliders are 5*5 boxes in the center of each tile

public class robotD : MonoBehaviour {
	[SerializeField]
	private String colour = "blue";
	public String Colour{
		get{
			return "\"" + colour + "\"";
		}
		set{
			colour = value;
		}
	}

	[SerializeField]
	private String startDirection = "Down";

	//default position
	private float defaultPosX;
	private float defaultPosY;
	private int defaultDirection;

	// animation/movement variables
	private Animator anim;
	private Vector3 pos;
	[SerializeField]
	private float speed = 0.0125f;

	// package interaction variables
	private bool packageCol;
	private bool pickedUp;
	private List<package> packages;

	// timed functions
	void Start(){
		packages = new List<package>();

		pickedUp = false;
		packageCol = false;

		anim = gameObject.GetComponent<Animator>();
		pos = transform.position; // Take the current position

		//default position and orientation
		defaultPosX = transform.position.x;
		defaultPosY = transform.position.y;
		switch(startDirection){
			case "Up":
				defaultDirection = 1;
				break;
			case "Right":
				defaultDirection = 2;
				break;
			case "Down":
				defaultDirection = 3;
				break;
			case "Left":
				defaultDirection = 4;
				break;
			default:
				Debug.LogError("robot: string startDirection value invalid");
				defaultDirection = 3;
				break;
		}
		anim.SetInteger("Direction", defaultDirection);
    //defaultDirection = anim.GetInteger("Direction");
	}

	public void reset(){
		transform.Translate(defaultPosX - transform.position.x, defaultPosY - transform.position.y, 0);
		pos = transform.position;
		anim.SetInteger("Direction", defaultDirection);
    anim.SetBool("Motion", false);
    anim.SetBool("Pick", false);
    anim.SetBool("Throw", false);
		//transform.position.x = defaultPosX;
		//transform.position.y = defaultPosY;
		pickedUp = false;
	}

	void Update(){
	}

	void FixedUpdate(){
		if(pos != transform.position){ // && play ?
    	transform.position = Vector2.MoveTowards(transform.position, pos, speed);
		}
	}

	public String getPackageColour(){
		if(pickedUp){
			if(packages[0] != null){
				return "\"" + packages[0].Colour + "\"";
			} else{
				Debug.Log("No reference to package");
				return "";
			}
		} else{
			return "";
		}
	}

  // trigger functions
  //GameObject.ReferenceEquals(firstGameObject, secondGameObject)
  void OnTriggerEnter2D(Collider2D trigger){
		if (trigger.gameObject.tag == "package"){
			packages.Add(trigger.gameObject.GetComponent<package>());
			packageCol = true;
		}
	}

	void OnTriggerExit2D(Collider2D trigger){
		if(trigger.gameObject.tag == "package"){
			packages.Remove(trigger.gameObject.GetComponent<package>());
			if(packages.Count == 0){
				packageCol = false;
			}
		}
	}

	// player call-able functions
	public void pickUp(){
		anim.SetBool("Pick", true);
		if(packageCol){
			if(packages[0] != null){
				pickedUp = true;
				packages[0].pickUp(gameObject);
			}
		}
	}

	public void putDown(){
		if(pickedUp == true){
			anim.SetBool("Pick", true);
			packages[0].drop();
			pickedUp = false;
		} else {
			anim.SetBool("Pick", true);
		}
    package p = packages[0];
    packages.RemoveAt(0);
    packages.Add(p);
	}

	public void throwItem(){
		if(pickedUp == true){
			if(conveyorDetect()){
				Debug.Log("if");
				anim.SetBool("Throw", true);
				pickedUp = false;
				packages[0].putDownX(direction());
			} else{
        //Debug.Log("else");
				putDown();
			}
		}
		package p = packages[0];
    packages.RemoveAt(0);
		packages.Add(p);
	}

	//Direction Legend:
	//Up = 1, Right = 2, Down = 3, Left = 4
	public void turnLeft(){
		if(anim.GetInteger("Direction") == 1){
			anim.SetInteger("Direction", 4);
		} else if(anim.GetInteger("Direction") == 4){
			anim.SetInteger("Direction", 3);
		} else if(anim.GetInteger("Direction") == 3){
			anim.SetInteger("Direction", 2);
		} else if(anim.GetInteger("Direction") == 2){
			anim.SetInteger("Direction", 1);
		}
	}

	public void turnRight(){
		if(anim.GetInteger("Direction") == 1){
			anim.SetInteger("Direction", 2);
		} else if(anim.GetInteger("Direction") == 2){
			anim.SetInteger("Direction", 3);
		} else if(anim.GetInteger("Direction") == 3){
			anim.SetInteger("Direction", 4);
		} else if(anim.GetInteger("Direction") == 4){
			anim.SetInteger("Direction", 1);
		}
	}

	public void moveForward(){
		//Debug.Log("moveForward() - bot");
		if(tileDetect(direction())){
			// set target position to next tile in front
			pos += new Vector3 (direction().x, direction().y, 0.0f) * 21f;
			anim.SetBool("Motion", true);
		}
	}

	// item detection
	bool tileDetect(Vector2 dir){
		RaycastHit2D[] hitArray = Physics2D.RaycastAll(transform.position + new Vector3(0f, -3f, 0f), dir, 27f, -2, 0f, 0f);
		for(int i = 0; i < hitArray.Length; i++){
			if(hitArray[i].distance > 6 && hitArray[i].distance < 25 && hitArray[i].transform.gameObject.tag == "Tile"){
				return true;
			}
		}
		return false;
	}

	bool conveyorDetect(){
		RaycastHit2D[] hitArray = Physics2D.RaycastAll(transform.position + new Vector3(0f, -3f, 0f), direction(), 30f, -1, 0f, 2f);
		for(int i = 0; i < hitArray.Length; i++){
			if(hitArray[i].distance > 6 && hitArray[i].distance < 25 && hitArray[i].transform.gameObject.tag == "conveyor"){
				return true;
			}
		}
		return false;
	}

	// properties
	Vector2 direction(){
		if(anim.GetInteger("Direction") == 1){
			return Vector2.up;
		} else if(anim.GetInteger("Direction") == 2){
			return Vector2.right;
		} else if(anim.GetInteger("Direction") == 3){
			return Vector2.down;
		} else if(anim.GetInteger("Direction") == 4){
			return Vector2.left;
		} else {
			Debug.Log("Error in robot script function: Vector2 direction()");
			return Vector2.zero;
		}
	}

	// Animation
	public void neutralAnim(){
		anim.SetBool("Motion", false);
		anim.SetBool("Pick", false);
		anim.SetBool("Throw", false);
	}
}