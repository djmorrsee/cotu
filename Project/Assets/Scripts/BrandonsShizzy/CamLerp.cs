using UnityEngine;
using System.Collections;

public class CamLerp : MonoBehaviour {

	public Transform target;
	public float moveToSpeed, rotateToSpeed;
	void Start () {
	
	}
	void Update () {
		transform.position = Vector3.Lerp (transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), Time.deltaTime*moveToSpeed);
		//transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, Vector3.forward*target.eulerAngles.z, Time.deltaTime*rotateToSpeed);

	}
}
