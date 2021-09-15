using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class package : MonoBehaviour {
	[SerializeField]
	private float Speed = 14;
	[SerializeField]
	private float ySpeed = 18;
	private float moveX;
	private float moveY;

	private GameObject robot;
	private bool pickedUp;
	private bool beenPutDown;
	private float timePutDown;
	private float timeBeforeThrow = 0.15f;
	private Vector2 putDownDirection;

	private Transform tile;
	private bool tileCollision;

	private bool conveyorCollision;

	[SerializeField]
	private bool emitted = true;

	//for putting on conveyor belt
	private bool ignoreConveyorCollisions;
	private bool ignoreTileCollisions;

	private Vector2 initialPosition;
	private Transform initialTile;

	// package colour
	[SerializeField]
	private String colour = "yellow";
	public String Colour{
		get{
			return colour;
		}
		set{
			colour = value;
		}
	}

	void Start(){
		ignoreConveyorCollisions = false;
		ignoreTileCollisions = false;

		initialPosition = transform.position;
		initialTile = null;

		pickedUp = false;
		beenPutDown = false;
		tileCollision = false;
		GetComponent<Rigidbody2D>().freezeRotation = true;
		conveyorCollision = false;
	}

	void FixedUpdate(){
		// Debug.Log(moveY);
		// this block is likely the problem
		if(tileCollision && !ignoreTileCollisions){
			if(tile.position.y >= gameObject.transform.position.y){
				moveY = 0;
				moveX = 0;
				if(!pickedUp){
					transform.position = Vector3.MoveTowards(transform.position, tile.position, 0.5f);
				}
			}else if(!conveyorCollision){
				//EXPERIMENTAL:
				//removed due to "packages stuck on conveyor" bug
				//moveX = 0f;
				// or
				moveY = -2.5f;
			}
		}

		if(pickedUp){
			transform.position = Vector3.MoveTowards(transform.position, robot.transform.position, 30f);
			if(robot.GetComponent<Animator>().GetInteger("Direction") == 3){
				gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
			}else{
				gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
			}
		}else if(!conveyorCollision){
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
		}

		if(beenPutDown){
			//up
			if(putDownDirection == Vector2.up){
				if(Time.time >= (timePutDown + timeBeforeThrow) 
					&& Time.time <= (timePutDown + timeBeforeThrow + 0.55f)){
					moveY = 3.1f;
					ignoreConveyorCollisions = true;
				} else if(Time.time >= (timePutDown + timeBeforeThrow + 0.40f)){
					ignoreConveyorCollisions = false;
          moveY = -2.5f;
					if(conveyorCollision){
						beenPutDown = false;
						moveY = 0;
					}
          //beenPutDown = false;
        }
			}
			//down
			if(putDownDirection == Vector2.down && Time.time >= (timePutDown + timeBeforeThrow)){
				gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
				ignoreTileCollisions = true;
				moveY = -2.0f;
				if(conveyorCollision){
					moveY = 0;
					beenPutDown = false;
					ignoreTileCollisions = false;
				}
			}
			//right/left
			if(putDownDirection == Vector2.right || putDownDirection == Vector2.left){
				if((Time.time >= timePutDown + timeBeforeThrow)
					&& Time.time <= (timePutDown + 0.4f + timeBeforeThrow)){
					moveX = 2.5f * putDownDirection.x;
					moveY = 2.5f;
				} else if(Time.time >= (timePutDown + 0.4f + timeBeforeThrow)){
					moveX = 2.5f * putDownDirection.x;
					moveY = -2.5f;
					beenPutDown = false;
				}
			}
		}
		//Debug.Log("moveX: " + moveX + ", moveY: " + moveY + ", tileCollision: " + tileCollision);
		GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * Speed, moveY * ySpeed);
	}

	void OnTriggerEnter2D(Collider2D col){
    //Debug.Log("trigger");
		if(col.gameObject.tag == "conveyor"){
      //Debug.Log("conveyor collision, ignoreConveyorCollisions = " + ignoreConveyorCollisions);
			conveyorCollision = true;
			if(!ignoreConveyorCollisions){
				moveX = 0.85f;
				if(col.GetComponent<conveyorBelt>().Direction == "left"){
					moveX = -0.85f;
				}
				moveY = 0f;
			}
		}else if(col.gameObject.tag == "shoot"){
			moveX = 1;
			tileCollision = false;
		}else if(col.gameObject.tag.Substring(col.gameObject.tag.Length - 4, 4) == "Exit"){
			Debug.Log("here, colour = " + col.gameObject.tag.Substring(0, col.gameObject.tag.Length - 4) + "\nmyColour = " + colour);
			//eventuallly, expand above if to ensure yellow goes to yellow and red goes to red
			if(col.gameObject.tag.Substring(0,col.gameObject.tag.Length - 4) == colour){
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				moveX = 0f;
				moveY = 0f;
				GameObject[] packages = GameObject.FindGameObjectsWithTag("package");
				bool noPackages = true;
				foreach(GameObject package in packages){
					if(package.GetComponent<SpriteRenderer>().enabled == true){
						noPackages = false;
					} 
				}
				if(noPackages){
					GameObject.Find("EventSystem").GetComponent<LevelManager>().setNoPackages();
				}
			} else {
        moveX = 0f;
        moveY = 0f;
			}
		} else if(col.gameObject.tag == "Tile" && !beenPutDown){
			// first time package is triggered by tile -> tile is initialTile
			if(initialTile == null){
				initialTile = col.gameObject.transform;
			}
			tileCollision = true;
			tile = col.gameObject.transform;
      transform.position = Vector3.MoveTowards(transform.position, tile.position, 0.5f);
		} else {
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "Tile"){
			tileCollision = false;
		} else if(col.gameObject.tag == "package"){

		} else if(col.gameObject.tag != "robot" && !conveyorCollision){
			moveY = -1.0f;
		} else if(col.gameObject.tag == "conveyor"){
			conveyorCollision = false;
		} else {
			pickedUp = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
    //Debug.Log("collision");
		if(col.gameObject.tag == "conveyor" && !ignoreConveyorCollisions){
			conveyorCollision = true;
			moveX = 0.85f;
			moveY = 0f;
		}else if(col.gameObject.tag == "robot"){
			// idk either, man
		}else{
			//Debug.Log("Unrecognized collision");
			//moveX = 0;
			//moveY = -1;
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if(col.gameObject.tag == "conveyor"){
			conveyorCollision = false;
		}
		moveY = -1;
	}

	public void pickUp(GameObject robotObject){ // what to do when picked up by a robot
		Debug.Log(colour + "package pickedUp");
		robot = robotObject;
		pickedUp = true;
	}

	public void putDownX(Vector2 direction){ // thrown onto conveyor belt
		Debug.Log(colour + " package putDownX");
		if(direction == Vector2.up){
			putDownUp();
			return;
		}
		if(direction == Vector2.right){
			putDownRight();
			return;
		}
		if(direction == Vector2.down){
			putDownDown();
			return;
		}
		if(direction == Vector2.left){
			putDownLeft();
			return;
		}
		pickedUp = false;
		beenPutDown = true;
		timePutDown = Time.time;
		putDownDirection = direction;
	}

	public void putDownUp(){
		pickedUp = false;
		beenPutDown = true;
		timePutDown = Time.time;
		putDownDirection = Vector2.up;
	}

	public void putDownRight(){
		pickedUp = false;
		beenPutDown = true;
		timePutDown = Time.time;
		putDownDirection = Vector2.right;
	}

	public void putDownDown(){
		pickedUp = false;
		beenPutDown = true;
		timePutDown = Time.time;
		putDownDirection = Vector2.down;
	}

	public void putDownLeft(){
		pickedUp = false;
		beenPutDown = true;
		timePutDown = Time.time;
		putDownDirection = Vector2.left;
	}

	public void drop(){ // drop in place
		pickedUp = false;
		if(tile.gameObject.GetComponent<levelCompleteTile>() != null){
			tile.gameObject.GetComponent<levelCompleteTile>().packagePutDown();
		}
	}

	public void reset(){
		if(emitted){
			Destroy(gameObject);
		}else{
			pickedUp = false;
			beenPutDown = false;
			// might cause problems \/
      tileCollision = false;

			conveyorCollision = false;
      gameObject.GetComponent<BoxCollider2D>().enabled = true;
      gameObject.GetComponent<SpriteRenderer>().enabled = true;
			transform.position = initialPosition;
			moveX = 0f;
			moveY = 0f;
			if(initialTile != null){
				tile = initialTile;
			}
			
			
			//Conveyor belt 
			ignoreConveyorCollisions = false;
			ignoreTileCollisions = false;
		}
	}
}
