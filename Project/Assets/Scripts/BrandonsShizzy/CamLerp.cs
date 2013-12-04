using UnityEngine;
using System.Collections;

public class CamLerp : MonoBehaviour {

	public Transform target;
	public float moveToSpeed, rotateToSpeed;
	void Start () {
	
	}
	void Update () {
		transform.position = Vector3.Lerp (transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), Time.deltaTime*moveToSpeed);
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.MoveTowardsAngle(transform.eulerAngles.z, target.eulerAngles.z, Time.deltaTime*rotateToSpeed));

	}
}
