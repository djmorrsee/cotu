//// Created By Daniel Morrissey
////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetPlanet : MonoBehaviour
{

	////////////////////////////////
	//// Class Variables 
	////////////////////////////////
	// Public
	public GameObject[] planets;


	// Private
	private float[] radii = new float[7] { 1f, 1.5f, 2f, 3f, 4f, 6f, 12f };
	

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
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
	public GameObject PlanetForRadius (float planetRadius)
	{
		int i = 0;
		for (i = 0; i < 7; ++i) {
			if (planetRadius <= radii [i]) {
				break;
			}
		}
		i = Mathf.Clamp (i, 0, 6);
		GameObject pl = (GameObject)Instantiate (planets [i]);
		pl.transform.localScale = Vector3.one * (planetRadius / radii [i]);
		
		pl.GetComponent<MeshFilter> ().mesh.vertices = AddSurfaceNoise (pl.GetComponent<MeshFilter> ().mesh.vertices, radii [i]);
		
		
		Vector3[] verts = pl.GetComponent<MeshFilter> ().mesh.vertices;
		List<Vector2> outterVerts = new List<Vector2> ();
		for (int j = 0; j < verts.Length; ++j) {
			if (verts [j].magnitude > planetRadius * 0.8f)
				outterVerts.Add (verts [j]);
		}
		
		Vector2[] _verts = outterVerts.ToArray ();
		pl.GetComponent<PolygonCollider2D> ().points = _verts;
		
		
		
		
		return pl;
	}


	// Private
	
	float previousScale = 1;
	Vector3[] AddSurfaceNoise (Vector3[] verts, float planetRadius)
	{
		print (planetRadius);
		Vector3[] newVerts = verts;
		float distance = planetRadius * 0.95f;
		
		
		
		int vCount = newVerts.Length;
		for (int i = 0; i < vCount; ++i) {
			if (newVerts [i].sqrMagnitude > distance * distance) {
				float scale = Random.Range (1, 1 + 0.16f / planetRadius);
				scale = LowPassFilter (scale);
				
				
				for (int j = 0; j < vCount; ++j) { 
					if (Vector3.Distance (newVerts [i], newVerts [j]) < 0.05f) {
						newVerts [i] *= scale;
					}
				}
			}
		}
		verts = newVerts;
		return verts;
		
	}

	private const float AccelLowPass = 0.09f;
	float LowPassFilter (float x)
	{
		float _x = (x * AccelLowPass) + (previousScale * (1.0f - AccelLowPass));
		previousScale = x;
		return _x;
	}
    
}
