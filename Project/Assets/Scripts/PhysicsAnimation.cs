using UnityEngine;
using System.Collections;

public class PhysicsAnimation : MonoBehaviour {

	public Transform[] animationCharacterJoints;
	public ConfigurableJoint[] physicsCharacterJoints;
	public float[] limbStrengths;
	public float spring, damper, force;
	//public Vector3 rotationFix;
	//public bool swapXY, swapXZ, swapYZ;
	Quaternion[] startRot;

	void Start(){
		startRot = new Quaternion[animationCharacterJoints.Length];
		for (int i = 0; i < animationCharacterJoints.Length; i++) {
			startRot[i] = animationCharacterJoints[i].localRotation;
			
			physicsCharacterJoints[i].transform.localRotation = animationCharacterJoints[i].localRotation;
			JointDrive jointStrength = new JointDrive();
			jointStrength.mode = JointDriveMode.Position;
			jointStrength.positionSpring = spring;
			jointStrength.positionDamper = damper;
			jointStrength.maximumForce = force;
			physicsCharacterJoints[i].angularXDrive = jointStrength;
			physicsCharacterJoints[i].angularYZDrive = jointStrength;
			
		}
	}
	
	
	void Update () {
		for (int i = 0; i < physicsCharacterJoints.Length; i++) {
			//Vector3 local = new Vector3 (animationCharacterJoints[i].localEulerAngles.x-startRot[i].x, animationCharacterJoints[i].localEulerAngles.y-startRot[i].y, animationCharacterJoints[i].localEulerAngles.z-startRot[i].z);
			//Quaternion temp = Quaternion.Euler(local);
			//print (temp);
			//Vector3 local = animationCharacterJoints[i].localEulerAngles;
			//Vector3 origin = new Vector3 (local.x * 0, local.y * 0, local.z * 1);
			//Vector3 final = startRot[i] * Vector3.forward;
			//Quaternion temp = Quaternion.FromToRotation(origin, final);
			
			/*if (swapXY) {
				temp = new Vector3 (temp.y, temp.x, temp.z);
			}
			if (swapXZ) {
				temp = new Vector3 (temp.z, temp.y, temp.x);
			}
			if (swapYZ) {
				temp = new Vector3 (temp.x, temp.z, temp.y);
			}*/
			//physicsCharacterJoints[i].targetRotation = temp;
			//physicsCharacterJoints[i].targetRotation = Quaternion.Euler(new Vector3(temp.x-(temp.x*2*rotationFix.x),temp.y-(temp.y*2*rotationFix.y),temp.z-(temp.z*2*rotationFix.z)));
			physicsCharacterJoints[i].SetTargetRotationLocal (animationCharacterJoints[i].localRotation, startRot[i]);

			JointDrive jointStrength = new JointDrive();
			jointStrength.mode = JointDriveMode.Position;
			jointStrength.positionSpring = spring*limbStrengths[i];
			jointStrength.positionDamper = damper*limbStrengths[i];
			jointStrength.maximumForce = force;
			physicsCharacterJoints[i].angularXDrive = jointStrength;
			physicsCharacterJoints[i].angularYZDrive = jointStrength;

		}
	}
}
