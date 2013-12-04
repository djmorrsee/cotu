using UnityEngine;
using System.Collections;

public class CharacterControls2 : MonoBehaviour {

	public float movementForce, legLength, standStrength, standDamping, velocityDamping, legWidth, maxWalkSpeed, maxRunSpeed, legMoveSpeed, stepDistance;
	public Vector3 rFootPosition, lFootPosition;
	public Transform rightLegMove, leftLegMove;
	public int groundLayer;
	Vector3 relativeRight, relativeUp, currentRFootPos, currentLFootPos, currentPos;
	bool rFoot;
	void Start () {
		relativeRight = transform.TransformDirection(Vector3.right);
		relativeUp = transform.TransformDirection(Vector3.up);

		RaycastHit2D hitFootL = Physics2D.Raycast(transform.position+rFootPosition, -relativeUp, 100, 1<<groundLayer);
		RaycastHit2D hitFootR = Physics2D.Raycast(transform.position+lFootPosition, -relativeUp, 100, 1<<groundLayer);

		currentRFootPos = hitFootR.point;
		currentLFootPos = hitFootL.point;

		currentPos = transform.position;

	}
	

	void FixedUpdate () {
		bool running = false;

		if (Input.GetKey(KeyCode.LeftShift)) {
			running = true;
		}

		float Axis1 = Input.GetAxis("Horizontal");
		relativeRight = transform.TransformDirection(Vector3.right);
		relativeUp = transform.TransformDirection(Vector3.up);

		if ((!running && Mathf.Abs(rigidbody2D.velocity.x) < maxWalkSpeed) || (running && Mathf.Abs(rigidbody2D.velocity.x) < maxRunSpeed)) {
			rigidbody2D.AddForce(relativeRight*(movementForce*Axis1));
		}


		RaycastHit2D hit1 = Physics2D.Raycast(transform.position+(relativeRight*legWidth), -relativeUp, 100, 1<<groundLayer);
		RaycastHit2D hit2 = Physics2D.Raycast(transform.position-(relativeRight*legWidth), -relativeUp, 100, 1<<groundLayer);
		addForce(hit1, transform.position+(relativeRight*legWidth));
		addForce(hit2, transform.position-(relativeRight*legWidth));

		if (Vector3.Distance(transform.position, currentPos) > stepDistance) {
		if (transform.TransformDirection(rigidbody2D.velocity).x > 0) {
				relativeRight = -relativeRight;
		}
		if (rFoot) {
				RaycastHit2D hitFootR = Physics2D.Raycast(transform.position+transform.TransformDirection(rFootPosition)-(relativeRight*(stepDistance-0.05f)), -relativeUp, 100, 1<<groundLayer);
			currentRFootPos = hitFootR.point;
			rFoot = false;
		}
		else {
			RaycastHit2D hitFootL = Physics2D.Raycast(transform.position+transform.TransformDirection(lFootPosition)-(relativeRight*(stepDistance-0.05f)), -relativeUp, 100, 1<<groundLayer);
			currentLFootPos = hitFootL.point;
			rFoot = true;
		}
			currentPos = transform.position;
		}


		Debug.DrawLine(currentRFootPos+Vector3.up*2, currentRFootPos, Color.red);
		Debug.DrawLine(currentLFootPos+Vector3.up*2, currentLFootPos, Color.green);

		//rightLegMove.position = rFootPosition;
		//leftLegMove.position = lFootPosition;

		rightLegMove.position = Vector3.Lerp(rightLegMove.position, currentRFootPos, Time.deltaTime*legMoveSpeed);
		leftLegMove.position = Vector3.Lerp(leftLegMove.position, currentLFootPos, Time.deltaTime*legMoveSpeed);

	}
	void addForce (RaycastHit2D hit, Vector3 startPos) {
		float distance = Vector2.Distance (startPos, hit.point);
		if (hit != null && distance < legLength) {
			Debug.DrawLine(startPos, hit.point);
			distance = Vector2.Distance (startPos+(relativeUp*standDamping), hit.point);
			distance = distance/legLength;
			distance = Mathf.Clamp01(distance);
			float standForce = standStrength*distance - (Mathf.Abs(rigidbody2D.velocity.y) * velocityDamping);
			rigidbody2D.AddForceAtPosition(relativeUp*standForce, startPos);
		}
	}
}