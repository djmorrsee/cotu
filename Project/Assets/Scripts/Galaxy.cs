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
	
	List<GeneratePlanet.PlanetData> planets;

	////////////////////////////////
	//// Mono Methods
	////////////////////////////////
	void Start ()
	{
		Random.seed = seed;
	
		planets = new List<GeneratePlanet.PlanetData> ();
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
		Vector2 newPlanetCoords = new Vector2 (Random.Range (-galaxyWidth, galaxyWidth), Random.Range (-galaxyWidth, galaxyWidth));
		float radius = Random.Range (minPlanetRadius, maxPlanetRadius);
		const int layers = 5;
		const int verts = 64;
		int mass = (int)radius;
		
		GeneratePlanet gp = GetComponent<GeneratePlanet> ();
		
		GeneratePlanet.PlanetData newPlanetData = gp.GeneratePlaneteData (newPlanetCoords, radius, layers, verts, layers);
		
		
		bool canPlace = true;
		foreach (GeneratePlanet.PlanetData pd in planets) {
			if (AreOverlapping (newPlanetData, pd)) {
				// Planet would collide, dont add
				canPlace = false;
			}
		}
		if (canPlace) {
			planets.Add (newPlanetData);
			gp.GenerateAPlanet (newPlanetData);
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
