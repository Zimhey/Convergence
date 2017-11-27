using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawn : MonoBehaviour {

	//Drag desired character prefab to this public variable!
	public GameObject Character;

	// Use this for initialization
	void Start () {
		//Spawn a new boy at the position of this tile
		Instantiate (Character, this.transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
