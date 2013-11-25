//// Created By Daniel Morrissey
////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GeneratePlanet : MonoBehaviour
{
	////////////////////////////////
	//// Class Variables 
	////////////////////////////////
	// Public
	public Material mat;
	
	// Private
	
	public struct PlanetData
	{
		public Vector2 coordinates;
		public int verts;
		public float radius;
		public int layers;
		
		public int mass;
	};
	

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
	void Start ()
	{
	}

	void Update ()
	{
		
	}
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


	// Private
	public PlanetData GeneratePlaneteData (Vector2 coords, float radius, int mass, int numberOfVerts = 16, int layers = 5)
	{
		// Create Random Planet Data Based on Bounds
		return new PlanetData{
			coordinates = coords,
			verts = numberOfVerts,
			radius = radius, 
			layers = layers,
			mass = mass
		};
	}
	
	public GameObject GenerateAPlanet (PlanetData planetData)
	{
		GameObject newPlanet = new GameObject ("Planet");
		MeshFilter filter = newPlanet.AddComponent<MeshFilter> ();
		MeshRenderer renderer = newPlanet.AddComponent<MeshRenderer> ();
		Gravity gravity = newPlanet.AddComponent<Gravity> ();
		
		gravity.gravityRadius = planetData.radius * 3;
		gravity.gravityForce = -planetData.radius * planetData.mass;
		
		renderer.material = mat;
		
		PolygonCollider2D collider = newPlanet.AddComponent<PolygonCollider2D> ();
		
		// Vertices // 
		List<Vector3> newVertices = new List<Vector3> ();
	
		newVertices.Add (Vector3.zero);
		
		float degreesPerVertex = 360f / (planetData.verts);		
		float layerWidth = planetData.radius / planetData.layers;
		
		for (int i = 0; i < planetData.layers; ++i) {
			float layerRadius = planetData.radius - layerWidth * i;
			
			for (int j = 0; j < planetData.verts; ++j) {
				float angle = j * degreesPerVertex * 0.0174532f;
				float noise = Random.Range (1, 1 + 5f / planetData.radius);
				
				Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle)) * layerRadius;
				
				if (newVertices.Contains (position)) {
					Debug.LogError ("Duplicate Vertex");
				}
				newVertices.Add (position);
			}
		}
		
		List<int> newEdges = new List<int> ();
		
		int vertIndexA, vertIndexB;
		
		for (int j = 1; j < planetData.layers; ++j) {
			vertIndexA = 1;
			vertIndexB = 2;
			int layerOffset = planetData.verts * (j - 1);
			
			for (int i = 0; i < planetData.verts; ++i) {
				newEdges.Add (vertIndexA + planetData.verts + layerOffset);
				newEdges.Add (vertIndexB + layerOffset);
				newEdges.Add (vertIndexA + layerOffset);
			
				newEdges.Add (vertIndexB + planetData.verts + layerOffset);
				newEdges.Add (vertIndexB + layerOffset);
			
				newEdges.Add (vertIndexA + planetData.verts + layerOffset);
			
				vertIndexA++;
				vertIndexB++;
				if (vertIndexB > planetData.verts) {
					vertIndexB = 1;
				}
			}
		}			
		
		// Connect the lowest layer to the center
		vertIndexA = 1 + planetData.verts * (planetData.layers - 1);
		vertIndexB = 2 + planetData.verts * (planetData.layers - 1);
		for (int i = 0; i < planetData.verts; ++i) {

			// Add Edges
			newEdges.Add (0);
			newEdges.Add (vertIndexB);
			newEdges.Add (vertIndexA);

			// Update vert index
			vertIndexA++;
			vertIndexB++;
			if (vertIndexB > planetData.verts * planetData.layers)
				vertIndexB = 1 + planetData.verts * (planetData.layers - 1);
			if (vertIndexA > planetData.verts * planetData.layers)
				vertIndexA = 1 + planetData.verts * (planetData.layers - 1);
		}
		
		// UV's
		int uvi = 0;
		Vector2[] uvs = new Vector2[newVertices.Count];
		//		
		while (uvi < uvs.Length) {
			uvs [uvi] = new Vector2 (newVertices [uvi].x / (2f * planetData.radius) + 0.5f, newVertices [uvi].y / (2f * planetData.radius) + 0.5f);
			uvi++;
		}
		
		// Noise
		for (int i = 1; i <= planetData.verts; ++i) {
			newVertices [i] *= Random.Range (1, 1 + 0.75f / planetData.radius);
		}
		
		
		// Mesh
		Mesh newMesh = new Mesh ();
		newMesh.name = "PlanetMesh";
		
		newMesh.Clear ();
		newMesh.vertices = newVertices.ToArray ();
		newMesh.triangles = newEdges.ToArray ();
		
		
		
		
		newMesh.uv = uvs;
		
		newMesh.RecalculateNormals ();
		newMesh.RecalculateBounds ();
		
		filter.mesh = newMesh;
		
		List<Vector2> collisionMesh = new List<Vector2> ();
		for (int i = 1; i <= planetData.verts; ++i) {
			collisionMesh.Add (newVertices [i]);
		}	
		
		collider.points = collisionMesh.ToArray ();
		
		newPlanet.transform.position = planetData.coordinates;
		return newPlanet;
		
	}

	float previousScale = 1;
	private const float AccelLowPass = 0.5f;
	float LowPassFilter (float x)
	{
		float _x = (x * AccelLowPass) + (previousScale * (1.0f - AccelLowPass));
		previousScale = x;
		print (string.Format ("0: {0}", (x - _x) / x));
		return _x;
	}


	
}
