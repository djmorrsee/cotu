//// Created By Daniel Morrissey
////////////////////////////////
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;


public class GeneratePlanet : MonoBehaviour
{
	////////////////////////////////
	//// Class Variables 
	////////////////////////////////
	// Public
	public int layerRadius = 1;
	public Material mat;
	
	// Private

	

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
	void Start ()
	{
//		GenerateBPlanet (GeneratePlaneteData (Vector2.zero, 10, 5));
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
	public Planet GeneratePlaneteData (Vector2 coords, float radius, int mass)
	{
		// Create Random Planet Data Based on Bounds
		return new Planet{
			Coordinates = coords,
			Radius = radius, 
			Mass = mass
		};
	}
	
	public GameObject GenerateAPlanet (Planet planetData)
	{
		GameObject newPlanet = new GameObject ("Planet");
		AddComponentsToPlanetObject (ref newPlanet, planetData);
		
		int layers = (int)(planetData.Radius / layerRadius);
		
		List<Vector3> newVertices = GeneratePlanetVertices (layers);
		List<int> newFaces = GeneratePlanetEdges (layers);
		Vector2[] uvs = GenerateUVs (newVertices, planetData.Radius);
		
		// Outter Layer Verts 
		int outterLayerVertCount = layers * 4;
		int lastVertOnSurface = newVertices.Count - 1;
		int firstVertOnSurface = lastVertOnSurface - outterLayerVertCount + 1;
		
		// Noise  
		PlanetDeformation (ref newVertices, layers, firstVertOnSurface, lastVertOnSurface);
		
		// Collision		
		List<Vector2> collisionMesh = new List<Vector2> ();
		for (int i = firstVertOnSurface; i <= lastVertOnSurface; ++i) {
			collisionMesh.Add (newVertices [i]);
		}	
		
		newPlanet.GetComponent<PolygonCollider2D> ().points = collisionMesh.ToArray ();
		
		// Mesh
		Mesh newMesh = GenerateMesh (newVertices, newFaces, uvs);
		newPlanet.GetComponent<MeshFilter> ().mesh = newMesh;
				
		// Position	
		newPlanet.transform.position = planetData.Coordinates;
		return newPlanet;
	}
	
	Mesh GenerateMesh (List<Vector3> newVertices, List<int> newFaces, Vector2[] uvs)
	{
		// Mesh
		Mesh newMesh = new Mesh ();
		newMesh.name = "PlanetMesh";
		
		newMesh.Clear ();
		newMesh.vertices = newVertices.ToArray ();
		newMesh.triangles = newFaces.ToArray ();
		
		newMesh.uv = uvs;
		
		newMesh.RecalculateNormals ();
		newMesh.RecalculateBounds ();
		return newMesh;
	
	}
	
	void AddComponentsToPlanetObject (ref GameObject go, Planet _planetData)
	{
		MeshFilter filter = go.AddComponent<MeshFilter> ();
		MeshRenderer renderer = go.AddComponent<MeshRenderer> ();
		Gravity gravity = go.AddComponent<Gravity> ();
		
		Planet planetData = (Planet)go.AddComponent (_planetData.GetType ());
		
		foreach (FieldInfo f in _planetData.GetType().GetFields()) {
			f.SetValue (planetData, f.GetValue (_planetData));
		}
		
		gravity.gravityRadius = 3 * planetData.Radius;
		//The gravity force makes the character physics a big wonky, lets make it a bit lower. this is just a temp fix
		gravity.gravityForce = -planetData.Mass/20;
		
		renderer.material = mat;
		
		PolygonCollider2D collider = go.AddComponent<PolygonCollider2D> ();
		CircleCollider2D atmosphere = go.AddComponent<CircleCollider2D> ();
		atmosphere.isTrigger = true;
		atmosphere.radius = planetData.Radius * 3f;
		//Add a planet tag to the planet, so the character can find the nearest one
		go.tag = "Planet";
	}

	List<Vector3> GeneratePlanetVertices (int layers)
	{
		// Vertices // 
		List<Vector3> newVertices = new List<Vector3> ();
		
		newVertices.Add (Vector3.zero);

		
		for (int thisLayer = 1; thisLayer <= layers; thisLayer++) {
			
			int verticesForLayer = thisLayer * 4;
			float degreesPerVertex = 360f / verticesForLayer;
			float thisLayerRadius = thisLayer * layerRadius;
			
			for (int thisVertex = 0; thisVertex < verticesForLayer; thisVertex++) {
				float vertexAngle = thisVertex * degreesPerVertex * 0.0174532f;
				Vector3 vertexPosition = new Vector3 (Mathf.Cos (vertexAngle), Mathf.Sin (vertexAngle)) * thisLayerRadius;
				newVertices.Add (vertexPosition);
			}
		}
		return newVertices;
	}	
	
	List <int> GeneratePlanetEdges (int layers)
	{
		// Edges //
		List<int> newFaces = new List<int> ();
		
		int totalVerts = 0;
		for (int thisLayer = 0; thisLayer < layers; ++thisLayer) {
			int verticesThisLayer = 4 * (thisLayer + 1);
			int verticesLastLayer = 4 * thisLayer;
			
			for (int thisQuadrent = 0; thisQuadrent < 4; ++thisQuadrent) {
				int quadrentOffset = thisQuadrent * verticesThisLayer / 4;
				
				int startVertIndex = totalVerts + 1 + quadrentOffset;
				
				for (int innerFaceIndex = 0; innerFaceIndex < thisLayer; ++innerFaceIndex) {
					int v1 = startVertIndex + innerFaceIndex;					
					int v2 = v1 + 1;				
					int v3 = v1 - verticesLastLayer - 1 * thisQuadrent;			
					int v4 = v3 + 1;
					
					if (v4 > totalVerts) {
						v4 -= 4 * thisLayer;
					}
					
					newFaces.Add (v3);
					newFaces.Add (v2);
					newFaces.Add (v1);
					
					newFaces.Add (v4);
					newFaces.Add (v2);
					newFaces.Add (v3);
				}	
				
				// End Face
				if (thisLayer > 0) {
					
					int x1 = startVertIndex + thisLayer;
					int x2 = x1 + 1;
					int x3 = x1 - verticesLastLayer - thisQuadrent;
					
					if (x2 > totalVerts + verticesThisLayer) {
						x2 -= 4 * (thisLayer + 1);
						x3 = x2 - 4 * (thisLayer);
					}
					if (x3 == x1) {
						x3 = 0;
					}
					
					newFaces.Add (x3);
					newFaces.Add (x2);
					newFaces.Add (x1);
				}
				
			}
			totalVerts += verticesThisLayer;
			
		}		
		return newFaces;	
	}
	
	Vector2[] GenerateUVs (List<Vector3> vertices, float radius)
	{
		// UV's
		int uvi = 0;
		Vector2[] uvs = new Vector2[vertices.Count];
		//		
		while (uvi < uvs.Length) {
			uvs [uvi] = new Vector2 (vertices [uvi].x / (2f * radius) + 0.5f, vertices [uvi].y / (2f * radius) + 0.5f);
			uvi++;
		}
		return uvs;
	}
	
	void PlanetDeformation (ref List<Vector3> vertices, int layers, int start, int end)
	{
		for (int i = start; i <= end; ++i) {
			vertices [i] *= Random.Range (1, 1.04f);
		}	
	
	}
}
