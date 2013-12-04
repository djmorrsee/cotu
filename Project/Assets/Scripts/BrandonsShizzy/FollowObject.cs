using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	void Start () {
	
	}

	void Update () {
		transform.position = target.position+offset;
	}
}
