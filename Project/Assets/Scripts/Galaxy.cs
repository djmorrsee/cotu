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
	List<Planet> planets;

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
	void Start ()
	{
		Random.seed = seed;
	
		planets = new List<Planet> ();
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
		Planet newPlanet = gp.GeneratePlaneteData (newPlanetCoords, radius, mass);
		
		// Check for planetary overlaps with our new planet		
		bool canPlace = true;
		foreach (Planet pl in planets) {
			if (AreOverlapping (newPlanet, pl)) {
				// Planet would collide, dont add it to the universe
				canPlace = false;
			}
		}
		if (canPlace) {
			// Create the Planet
			planets.Add (newPlanet);
			gp.GenerateAPlanet (newPlanet).transform.parent = galaxy.transform;
		}
		return canPlace;
	}

	
	bool AreOverlapping (Planet A, Planet B)
	{
		float distance = (A.Coordinates - B.Coordinates).magnitude;
        
		return (A.Radius + B.Radius > distance);
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
