//// Created By Daniel Morrissey
////////////////////////////////

using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{

	////////////////////////////////
	//// Class Variables 
	////////////////////////////////
	// Public
	public float movementForce;


	// Private
	
	

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
	void Start ()
	{

	}

	void FixedUpdate ()
	{
		if (Input.GetKey (KeyCode.W)) {
			rigidbody2D.AddForce (Vector2.up * movementForce);
		} else if (Input.GetKey (KeyCode.S)) {
			rigidbody2D.AddForce (-Vector2.up * movementForce);
		}
		if (Input.GetKey (KeyCode.A)) {
			rigidbody2D.AddForce (-Vector2.right * movementForce);
		} else if (Input.GetKey (KeyCode.D)) {
			rigidbody2D.AddForce (Vector2.right * movementForce);
		}
	}
	/*
	void FixedUpdate () {

	}

	void OnGUI () {

	}
	*/
	
	////////////////////////////////
	//// Class Methods 
	////////////////////////////////
	// Public



	// Private



	
}
