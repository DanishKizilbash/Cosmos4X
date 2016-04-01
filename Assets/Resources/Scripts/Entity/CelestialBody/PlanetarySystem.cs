using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Vectrosity;
namespace Cosmos
{
	public class PlanetarySystem:Tickable
	{
		public int ID;
		public List<Entity> entities;
		public List<CelestialBody> celestialBodies;
		public List<StarBody> stars;
		public List<PlanetBody> planets;
		//
		public double totalMass;
		public Coord coord;
		//
		public bool isVisible;
		private VectorLine debugLine;
		//

		public PlanetarySystem (Coord position=null)
		{
			if (GameManager.currentGame.currentSystem == null) {
				GameManager.currentGame.currentSystem = this;
			}
			GameManager.currentGame.planetarySystems.Add (this);
			entities = new List<Entity> ();
			name = "system" + UnityEngine.Random.Range (0, 100);
			debugLine = new VectorLine ("Debug_" + name, new List<Vector3> (), null, 2.0f, LineType.Discrete, Joins.None);
			if (position == null) {
				RandomizeSystemCoord ();
			} else {
				coord = position;
			}
			GenerateSystem ();
			Interval = 30;
			isVisible = false;
			SetVisiblity (false);
		}
		public override void Tick ()
		{
			drawDebugLines ();
			TickRequired = true;

		}
		private void drawDebugLines ()
		{
			debugLine.active = isVisible;
			if (isVisible) {
				debugLine.points3.Clear ();
				foreach (CelestialBody cb in celestialBodies) {
					if (cb.parent != null) {
						debugLine.points3.Add (cb.Center);
						debugLine.points3.Add (cb.parent.Center);
						cb.orbit.ShowOrbit ();
					}
				}
				debugLine.Draw3DAuto ();
			} else {
				foreach (CelestialBody cb in celestialBodies) {
					cb.orbit.ShowOrbit (false);
				}
			}
		}
		private void RandomizeSystemCoord ()
		{
			coord = new Coord (UnityEngine.Random.Range (0, 365), UnityEngine.Random.Range (0, 365), UnityEngine.Random.Range (0, 10), UnityEngine.Random.Range (0, 10));
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
			try {
				celestialBodies.Remove ((CelestialBody)entity);
				planets.Remove ((PlanetBody)entity);
				stars.Remove ((StarBody)entity);
			} catch (Exception) {

			}
			
		}
		public void SetVisiblity (bool visiblity)
		{
			isVisible = visiblity;
			foreach (Entity entity in entities) {		
				entity.SetVisible (visiblity);
			}

		}
		public override void OnSelected (bool value)
		{

		}
		#region Gen
		private void GenerateSystem ()
		{
			celestialBodies = new List<CelestialBody> ();	
			stars = new List<StarBody> ();
			planets = new List<PlanetBody> ();
			//
			totalMass = 0;
			GenStars ();
			GenPlanets ();
			//
			OffsetCelestialBodyPosition ();
			CleanUp ();
			Debug.Log (totalMass);
		}
		private void GenStars ()
		{
			float starMassRatio = 0.8f;
			
			int numStars = GalaxyManager.numStarsPerSystem.NextValue () + 1;
			float[] masses = new float[numStars];
			StellarClass[] starClasses = new StellarClass[numStars];
			for (int s = 0; s<numStars; s++) {
				StellarClass starClass = (StellarClass)GalaxyManager.stellarDie.NextValue ();
				starClasses [s] = starClass;
				masses [s] = GalaxyManager.stellarMass [starClass].Rand ();
			}
			for (int i = 0; i<numStars; i++) {
				for (int s = 1; s<numStars; s++) {
					if (masses [s] > masses [s - 1]) {
						float m = masses [s];
						StellarClass st = starClasses [s];
						masses [s] = masses [s - 1];
						starClasses [s] = starClasses [s - 1];
						masses [s - 1] = m;
						starClasses [s - 1] = st;
						//adjust mass value to force decreasing mass
						float newMass = starMassRatio * masses [s];
						masses [s] = newMass;
						foreach (StellarClass sc in GalaxyManager.stellarMass.Keys) {
							MinMax scMinMax = GalaxyManager.stellarMass [sc];
							if (scMinMax.max > newMass && scMinMax.min < newMass) {
								starClasses [s] = sc;
								break;
							}
						}
					}
				}
			}
			for (int s = 0; s<numStars; s++) {
				StellarClass starClass = starClasses [s];
				float mass = masses [s];
				float diameter = GalaxyManager.stellarDiameter [starClass].Rand ();
				float luminosity = GalaxyManager.stellarLuminosity [starClass].Rand ();
				float temperature = GalaxyManager.stellarTemperature [starClass].Rand ();
				StellarColor color = GalaxyManager.stellarColor [starClass];
				StarBody starBody = new StarBody ();
				if (s == 0) {
					starBody.Init ("", CelestialBodyType.Star, mass, diameter, this);
				} else {
					starBody.Init ("", CelestialBodyType.Star, mass, diameter, this, stars [s - 1]);
				}
				starBody.stellarClass = starClass;
				starBody.luminosity = luminosity;
				starBody.temperature = temperature;
				starBody.color = color; 
				starBody.Print ();
				starBody.scale *= UnitConversion.Distance.SolDiameter.Value (diameter);
				celestialBodies.Add (starBody);
				totalMass += UnitConversion.Mass.SolMass.Value (mass, UnitConversion.Mass.MegaTon);
				
				stars.Add (starBody);
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
					//
				}
			}
			
		}
		private void CleanUp ()
		{
			List<CelestialBody> clearedBodies = new List<CelestialBody> ();
			for (int i =0; i<planets.Count; i++) {
				for (int v =0; v<planets.Count; v++) {
					PlanetBody planet1 = planets [i];
					PlanetBody planet2 = planets [v];
					if (planet1 != planet2) {
						if (MathI.CirclesIntersect (planet1.orbit.Apoapsis, planet1.parent.Position, planet2.orbit.Apoapsis, planet2.parent.Position)) {
							if (planet1.orbit.Parent.Mass > planet2.orbit.Parent.Mass) {
								clearedBodies.Add (planet2);
							} else {
								clearedBodies.Add (planet1);
							}
						}
					}
				}
			}
			for (int i =0; i<clearedBodies.Count; i++) {
				clearedBodies [i].Destroy ();
			}
		}
		#endregion

	}
}
