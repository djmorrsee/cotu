using UnityEngine;
using System.Collections;

public class TwoDLookAt : MonoBehaviour {

	public Transform target;
	public float rotOffset;
	void Start () {
	
	}

	void Update () {
		float xPos = target.position.x - transform.position.x;
		float yPos = target.position.y - transform.position.y;
		float angle = Mathf.Atan2(yPos, xPos)*Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0, 0, angle+rotOffset);
	}
}
