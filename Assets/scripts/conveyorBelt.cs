using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorBelt : MonoBehaviour {
	[SerializeField]
	private string direction = "right";
	public string Direction{
		get{
			return direction;
		}
	}
}
