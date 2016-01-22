using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class PlanetarySystem
	{
		public string name;
		public List<Entity> entities;
		public List<CelestialBody> celestialBodies;
		public List<StarBody> stars;
		public List<PlanetBody> planets;
		public int systemSeed;
		//
		public double totalMass;
		public Coord coord;
		//
		public int numStars;
		public PlanetarySystem (Coord position=null)
		{
			if (GameManager.currentGame.currentSystem == null) {
				GameManager.currentGame.currentSystem = this;
			}
			GameManager.currentGame.planetarySystems.Add (this);
			entities = new List<Entity> ();
			name = "system" + Random.Range (0, 100);
			if (position == null) {
				RandomizeSystemCoord ();
			} else {
				coord = position;
			}
			GenerateSystem ();

		}
		private void RandomizeSystemCoord ()
		{
			coord = new Coord (Random.Range (0, 365), Random.Range (0, 365), Random.Range (0, 10), Random.Range (0, 10));
		}

		private void GenerateSystem ()
		{
			celestialBodies = new List<CelestialBody> ();	
			stars = new List<StarBody> ();
			planets = new List<PlanetBody> ();
			systemSeed = MathI.GenSeed (9);
			Random.seed = systemSeed;
			//
			totalMass = 0;
			GenStars ();
			GenPlanets ();
			//
			OffsetCelestialBodyPosition ();
			Debug.Log (totalMass);
		}
		private void GenStars ()
		{
			numStars = GalaxyManager.numStarsPerSystem.NextValue () + 1;
			for (int s = 0; s<numStars; s++) {
				StellarClass starClass = (StellarClass)GalaxyManager.stellarDie.NextValue ();
				float mass = GalaxyManager.stellarMass [starClass].Rand ();
				float diameter = GalaxyManager.stellarDiameter [starClass].Rand ();
				float luminosity = GalaxyManager.stellarLuminosity [starClass].Rand ();
				float temperature = GalaxyManager.stellarTemperature [starClass].Rand ();
				StellarColor color = GalaxyManager.stellarColor [starClass];
				StarBody starBody = new StarBody ();
				starBody.Init ("", CelestialBodyType.Star, mass, diameter, this);
				starBody.stellarClass = starClass;
				starBody.luminosity = luminosity;
				starBody.temperature = temperature;
				starBody.color = color;
				starBody.Print ();
				starBody.scale *= UnitConversion.Distance.SolDiameter.Value (diameter);
				celestialBodies.Add (starBody);
				stars.Add (starBody);
				totalMass += UnitConversion.Mass.SolMass.Value (mass, UnitConversion.Mass.MegaTon);
			}
		}
		private void GenPlanets ()
		{
			foreach (StarBody star in stars) {
				int numPlanets = GalaxyManager.numPlanetsPerStar.NextValue () + 1;
				Debug.Log ("Generating " + numPlanets + " planets for star # " + stars.IndexOf (star));

				for (int p = 0; p<numPlanets; p++) {
					PlanetClass planetClass = (PlanetClass)GalaxyManager.planetDie.NextValue ();
					float mass = GalaxyManager.planetMass [planetClass].Rand ();
					float diameter = GalaxyManager.planetDiameter [planetClass].Rand ();
					PlanetBody planetBody = new PlanetBody ();
					planetBody.Init ("", CelestialBodyType.Planet, mass, diameter, this, star);
					planetBody.planetClass = planetClass;
					planetBody.scale *= UnitConversion.Distance.EarthDiameter.Value (diameter);
					planetBody.Print ();
					celestialBodies.Add (planetBody);
					planets.Add (planetBody);

				}
			}
		}
		private void OffsetCelestialBodyPosition ()
		{
			foreach (CelestialBody cb in celestialBodies) {
				cb.Move ();
			}
		}
		public void AddEntity (Entity entity)
		{
			if (!entities.Contains (entity)) {
				entities.Add (entity);
			}
		}
		public void RemoveEntity (Entity entity)
		{
			entities.Remove (entity);
		}
		public void SetEntityVisiblity (bool visiblity)
		{
			foreach (Entity entity in entities) {		
				entity.SetVisible (visiblity);
			}

		}

	}
}
