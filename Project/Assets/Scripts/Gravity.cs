using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{
	
	public float gravityRadius;
	public float gravityForce;
	
	void FixedUpdate ()
	{
		foreach (GameObject physicsObject in GameObject.FindGameObjectsWithTag("Dynamic")) {
			float distance = Vector3.Distance (physicsObject.transform.position, transform.position);
			if (distance < gravityRadius) {
				float finalForce = gravityForce * ((gravityRadius - distance) / gravityRadius);
				Vector3 direction = physicsObject.transform.position - transform.position;
				direction = direction.normalized;
				physicsObject.rigidbody2D.AddForce (new Vector2 (direction.x * finalForce, direction.y * finalForce));
			}
		}
	}

	void ApplyForce ()
	{
//		GameObject[] moveableObjects = GameObject.FindGameObjectsWithTag ("Dynamic");
//		foreach (GameObject g in moveableObjects) {
//			Vector3 directionVector = (g.transform.position - transform.position);
//			if (directionVector.magnitude < gravityRadius) {
//				float force = gravityForce * (1 / directionVector.magnitude);
//				g.rigidbody2D.AddForce (directionVector * force);
//			}
//		}

	}
}
