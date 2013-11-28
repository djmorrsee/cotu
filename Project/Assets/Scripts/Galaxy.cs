//// Created By Daniel Morrissey
////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Galaxy : MonoBehaviour
{

	////////////////////////////////
	//// Class Variables 
	////////////////////////////////
	// Public
	public float galaxyWidth = 1000f;
	public float minPlanetRadius;
	public float maxPlanetRadius;
	
	public int numberOfPlanets;
	public int seed;

	// Private
	GameObject galaxy;
	List<GeneratePlanet.PlanetData> planets;

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
	void Start ()
	{
		Random.seed = seed;
	
		planets = new List<GeneratePlanet.PlanetData> ();
		galaxy = new GameObject ("Galaxy");
		FillGalaxy (numberOfPlanets);
	}

	void FixedUpdate ()
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

	
	
	
	bool AddPlanetToGalaxy ()
	{
		// Generate Random Planet Data
		Vector2 newPlanetCoords = new Vector2 (Random.Range (-galaxyWidth, galaxyWidth), Random.Range (-galaxyWidth, galaxyWidth));
		float radius = Random.Range (minPlanetRadius, maxPlanetRadius);
		const int layers = 2;
		int verts = 2 * (int)Mathf.PI * (int)radius;
		int mass = (int)radius;
	
		GeneratePlanet gp = GetComponent<GeneratePlanet> ();
		GeneratePlanet.PlanetData newPlanetData = gp.GeneratePlaneteData (newPlanetCoords, radius, mass, verts, layers);
		
		// Check for planetary overlaps with our new planet		
		bool canPlace = true;
		foreach (GeneratePlanet.PlanetData pd in planets) {
			if (AreOverlapping (newPlanetData, pd)) {
				// Planet would collide, dont add it to the universe
				canPlace = false;
			}
		}
		if (canPlace) {
			// Create the Planet
			planets.Add (newPlanetData);
			gp.GenerateAPlanet (newPlanetData).transform.parent = galaxy.transform;
		}
		return canPlace;
	}

	
	bool AreOverlapping (GeneratePlanet.PlanetData A, GeneratePlanet.PlanetData B)
	{
		float distance = (A.coordinates - B.coordinates).magnitude;
        
		return (A.radius + B.radius > distance);
	}
	
	int FillGalaxy (int numberOfPlanets)
	{
		int totalPlanets = 0;
		for (int i = 0; i < numberOfPlanets; ++i) {
			if (AddPlanetToGalaxy ()) {
				++totalPlanets;
			}
		}
		return totalPlanets;
	}
	
}
