using UnityEngine;
using System.Collections;

public class CharacterControls : MonoBehaviour {

	public float movementForce, moveSpeed, jumpForce, crouchAmount, loopSensitivity;
	public SpringJoint2D rightLegMove, leftLegMove;
	public float legMovement, yMoveMultiplier, yDownDivider, standStillHelp;
	Vector2 startPosL, startPosR;
	bool forward;
	float wantPos, wantPosYR, wantPosYL;
	public HingeJoint2D rotationHinge;
	Transform nearestPlanet;
	bool ragdoll;
	void Start () {
		forward = true;
		wantPos = legMovement;
		startPosL = leftLegMove.connectedAnchor;
		startPosR = rightLegMove.connectedAnchor;
		InvokeRepeating("LookForPlanets", 0, 5);
	}

	void Update () {

		float deltaX = nearestPlanet.position.x - transform.position.x;
		float deltaY = nearestPlanet.position.y - transform.position.y;
		float standRot = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI + 90;

		JointAngleLimits2D limit = new JointAngleLimits2D();
		limit.max = standRot;
		limit.min = standRot;
		rotationHinge.limits = limit;

		float Axis1 = Input.GetAxis("Horizontal");
		Vector3 relativeForce = transform.TransformDirection(Vector3.right);
		Vector3 relativeUp = transform.TransformDirection(Vector3.up);
		rigidbody2D.AddForce(relativeForce*(movementForce*Axis1));

		if (rightLegMove.connectedAnchor.x > startPosR.x+legMovement-loopSensitivity && forward) {
			wantPos = -legMovement;
			forward = false;
		}
		else if (rightLegMove.connectedAnchor.x < startPosR.x-legMovement+loopSensitivity && !forward) {
			wantPos = legMovement;
			forward = true;
		}
		if (Axis1 < -0.05f && !Input.GetKey("space")) {
			if (forward && Axis1 < 0) {
				wantPosYL = (legMovement-(Mathf.Abs(startPosL.x-leftLegMove.connectedAnchor.x)))*yMoveMultiplier;
				wantPosYR = -wantPosYL/yDownDivider;
			}
			else {
				wantPosYR = (legMovement-(Mathf.Abs(startPosR.x-rightLegMove.connectedAnchor.x)))*yMoveMultiplier;
				wantPosYL = -wantPosYR/yDownDivider;
			}

			rightLegMove.connectedAnchor = new Vector2(Mathf.MoveTowards(rightLegMove.connectedAnchor.x, startPosR.x+wantPos, Time.deltaTime*(moveSpeed*-Axis1)), startPosR.y+wantPosYR);
			leftLegMove.connectedAnchor = new Vector2(Mathf.MoveTowards(leftLegMove.connectedAnchor.x, startPosL.x-wantPos, Time.deltaTime*(moveSpeed*-Axis1)), startPosL.y+wantPosYL);
		}
		else if (!Input.GetKey("space")) {
			rightLegMove.connectedAnchor = Vector2.MoveTowards(rightLegMove.connectedAnchor, startPosR-Vector2.up*standStillHelp, Time.deltaTime*moveSpeed);
			leftLegMove.connectedAnchor = Vector2.MoveTowards(leftLegMove.connectedAnchor, startPosL-Vector2.up*standStillHelp, Time.deltaTime*moveSpeed);
		}
		if (Input.GetKey ("space")) {
			rightLegMove.connectedAnchor = Vector2.MoveTowards(rightLegMove.connectedAnchor, new Vector2(startPosR.x-0.1f, startPosR.y+crouchAmount), Time.deltaTime*moveSpeed);
			leftLegMove.connectedAnchor = Vector2.MoveTowards(leftLegMove.connectedAnchor, new Vector2(startPosL.x-0.1f, startPosL.y+crouchAmount), Time.deltaTime*moveSpeed);
		}
		if (Input.GetKeyUp ("space")) {
			rigidbody2D.AddForce(relativeUp*jumpForce);
		}
		if(Input.GetKeyDown("r")) {
			RagdollToggle(standRot);
		}
	}

	void RagdollToggle (float rotation) {
		if (ragdoll) {
			//transform.eulerAngles = new Vector3(0, 0, rotation);
			rightLegMove.enabled = true;
			leftLegMove.enabled = true;
			rotationHinge.useLimits = true;
			ragdoll = false;
		}
		else {
			rightLegMove.enabled = false;
			leftLegMove.enabled = false;
			rotationHinge.useLimits = false;
			ragdoll = true;
		}

	}
	void LookForPlanets () {
		float planetDistance = Mathf.Infinity;
		foreach(GameObject planet in GameObject.FindGameObjectsWithTag("Planet")) {
			if (Vector3.Distance(transform.position, planet.transform.position) < planetDistance) {
				planetDistance = Vector3.Distance(transform.position, planet.transform.position);
				nearestPlanet = planet.transform;
			}
			
		}
	}
}
