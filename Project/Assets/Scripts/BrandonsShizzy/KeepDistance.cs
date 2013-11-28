using UnityEngine;
using System.Collections;

public class KeepDistance : MonoBehaviour {

	Vector3 startPos;
	public Transform positionTarget;
	public float distance;
	public bool useLookAt;
	public Transform lookTarget;
	public float rotOffset;
	public bool useRelativeRotLimit;
	public Transform relativeObject;
	public float rotMin;
	public float rotMax;
	float relativeRot;
	float setRot;
	public bool useRelativeObjectRotationFix;
	public TwoDLookAt RelativeObjectLookAt;
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = (transform.position - positionTarget.transform.position).normalized * distance + positionTarget.transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z);
		if (useLookAt) {
		float xPos = lookTarget.position.x - transform.position.x;
		float yPos = lookTarget.position.y - transform.position.y;
		float angle = Mathf.Atan2(yPos, xPos)*Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0, 0, angle+rotOffset);
		}
		if (useRelativeRotLimit) {
			relativeRot = relativeObject.eulerAngles.z - transform.eulerAngles.z;
			if (relativeRot < rotMax && relativeRot > rotMin) {
				setRot = relativeRot;
			}
			transform.eulerAngles = new Vector3 (0, 0, relativeObject.eulerAngles.z-setRot);
		}
		if (useRelativeObjectRotationFix) {
			if (Vector3.Distance(transform.position, positionTarget.position) < distance) {
				//RelativeObjectLookAt.rotOffset
			}
		}
	}
}
