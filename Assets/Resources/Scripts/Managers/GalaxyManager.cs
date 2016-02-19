using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public enum CelestialBodyType
	{
		Star,
		Planet,
		Moon,
		Asteroid
	}
	//
	public enum StellarClass
	{
		O,
		B,
		A,
		F,
		G,
		K,
		M
	}
	public enum StellarColor
	{
		Blue,
		BlueWhite,
		White,
		YellowWhite,
		Yellow,
		Orange,
		Red
	}
	//
	public enum PlanetClass
	{
		GasPlanet,
		DesertPlanet,
		IcePlanet,
		OceanPlanet,
		LavaPlanet,
		TerrestrialPlanet
	}

	public static class GalaxyManager
	{		
		//Dice
		public static LoadedDie stellarDie;
		public static  LoadedDie planetDie;
		public static LoadedDie asteroidDie;
		public static LoadedDie moonDie;
		public static LoadedDie numStarsPerSystem;
		public static LoadedDie numPlanetsPerStar;
		//MinMax Dictionaries
		//Orbit
		public static Dictionary<CelestialBodyType,MinMax> orbitApoapsis;
		//Star
		public static Dictionary<StellarClass,MinMax> stellarMass;
		public static Dictionary<StellarClass,MinMax> stellarDiameter;
		public static Dictionary<StellarClass,MinMax> stellarLuminosity;
		public static Dictionary<StellarClass,MinMax> stellarTemperature;
		public static Dictionary<StellarClass,StellarColor> stellarColor;
		//Planet
		public static Dictionary<PlanetClass,MinMax> planetMass;
		public static Dictionary<PlanetClass,MinMax>  planetDiameter;
		//

		public static void Init ()
		{
			CreateDice ();	
			PopulateMinMax ();
		}
		private static void PopulateMinMax ()
		{
			GenOrbitMinMax ();
			GenStarMinMax ();
			GenPlanetMinMax ();
		}
		private static void GenOrbitMinMax ()
		{
			orbitApoapsis = new Dictionary<CelestialBodyType, MinMax> ();
			orbitApoapsis.Add (CelestialBodyType.Star, new MinMax (10000f, 50000f));
			orbitApoapsis.Add (CelestialBodyType.Planet, new MinMax (20f, 10000f));
			orbitApoapsis.Add (CelestialBodyType.Moon, new MinMax (0.1f, 30f));
			orbitApoapsis.Add (CelestialBodyType.Asteroid, new MinMax (0.001f, 0.1f));
		}
		private static void GenStarMinMax ()
		{
			//Stars
			//Mass
			stellarMass = new Dictionary<StellarClass, MinMax> ();
			stellarMass.Add (StellarClass.M, new MinMax (0.08f, 0.45f));
			stellarMass.Add (StellarClass.K, new MinMax (0.45f, 0.8f));
			stellarMass.Add (StellarClass.G, new MinMax (0.8f, 1.04f));
			stellarMass.Add (StellarClass.F, new MinMax (1.04f, 1.4f));
			stellarMass.Add (StellarClass.A, new MinMax (1.4f, 2.1f));
			stellarMass.Add (StellarClass.B, new MinMax (2.1f, 16f));
			stellarMass.Add (StellarClass.O, new MinMax (16f, 150f));
			//Diameter
			stellarDiameter = new Dictionary<StellarClass, MinMax> ();
			stellarDiameter.Add (StellarClass.M, new MinMax (0.3f, 0.7f));
			stellarDiameter.Add (StellarClass.K, new MinMax (0.7f, 0.96f));
			stellarDiameter.Add (StellarClass.G, new MinMax (0.96f, 1.15f));
			stellarDiameter.Add (StellarClass.F, new MinMax (1.15f, 1.4f));
			stellarDiameter.Add (StellarClass.A, new MinMax (1.4f, 1.8f));
			stellarDiameter.Add (StellarClass.B, new MinMax (1.8f, 6.6f));
			stellarDiameter.Add (StellarClass.O, new MinMax (6.6f, 1000f));
			//Luminosity
			stellarLuminosity = new Dictionary<StellarClass, MinMax> ();
			stellarLuminosity.Add (StellarClass.M, new MinMax (0.01f, 0.08f));
			stellarLuminosity.Add (StellarClass.K, new MinMax (0.08f, 0.6f));
			stellarLuminosity.Add (StellarClass.G, new MinMax (0.6f, 1.5f));
			stellarLuminosity.Add (StellarClass.F, new MinMax (1.5f, 5.0f));
			stellarLuminosity.Add (StellarClass.A, new MinMax (5.0f, 25.0f));
			stellarLuminosity.Add (StellarClass.B, new MinMax (25.0f, 30000.0f));
			stellarLuminosity.Add (StellarClass.O, new MinMax (30000.0f, 100000f));
			//Temperature
			stellarTemperature = new Dictionary<StellarClass, MinMax> ();
			stellarTemperature.Add (StellarClass.M, new MinMax (2400f, 3700f));
			stellarTemperature.Add (StellarClass.K, new MinMax (3700f, 5200f));
			stellarTemperature.Add (StellarClass.G, new MinMax (5200f, 6000f));
			stellarTemperature.Add (StellarClass.F, new MinMax (6000f, 7500f));
			stellarTemperature.Add (StellarClass.A, new MinMax (7500f, 10000f));
			stellarTemperature.Add (StellarClass.B, new MinMax (10000f, 30000f));
			stellarTemperature.Add (StellarClass.O, new MinMax (30000.0f, 100000f));
			//Color
			stellarColor = new Dictionary<StellarClass, StellarColor> ();
			stellarColor.Add (StellarClass.M, StellarColor.Red);
			stellarColor.Add (StellarClass.K, StellarColor.Orange);
			stellarColor.Add (StellarClass.G, StellarColor.Yellow);
			stellarColor.Add (StellarClass.F, StellarColor.YellowWhite);
			stellarColor.Add (StellarClass.A, StellarColor.White);
			stellarColor.Add (StellarClass.B, StellarColor.BlueWhite);
			stellarColor.Add (StellarClass.O, StellarColor.Blue);
		}
		private static void GenPlanetMinMax ()
		{
			planetMass = new Dictionary<PlanetClass, MinMax> ();
			planetMass.Add (PlanetClass.GasPlanet, new MinMax (2f, 8000f));
			planetMass.Add (PlanetClass.DesertPlanet, new MinMax (0.5f, 7f));
			planetMass.Add (PlanetClass.IcePlanet, new MinMax (0.1f, 10f));
			planetMass.Add (PlanetClass.LavaPlanet, new MinMax (0.05f, 8f));
			planetMass.Add (PlanetClass.OceanPlanet, new MinMax (0.5f, 7f));
			planetMass.Add (PlanetClass.TerrestrialPlanet, new MinMax (0.05f, 9f));

			planetDiameter = new Dictionary<PlanetClass, MinMax> ();
			planetDiameter.Add (PlanetClass.GasPlanet, new MinMax (2f, 20f));
			planetDiameter.Add (PlanetClass.DesertPlanet, new MinMax (0.3f, 2f));
			planetDiameter.Add (PlanetClass.IcePlanet, new MinMax (0.1f, 2f));
			planetDiameter.Add (PlanetClass.LavaPlanet, new MinMax (0.1f, 2f));
			planetDiameter.Add (PlanetClass.OceanPlanet, new MinMax (0.2f, 2f));
			planetDiameter.Add (PlanetClass.TerrestrialPlanet, new MinMax (0.1f, 2f));
		}

		private static void CreateDice ()
		{
			/*
				O=0.00003f,
				B=0.13f,
				A=0.6f,
				F=3.0f,
				G=7.6f,
				K=12.1f,
				M=76.5f
				*/
			stellarDie = new LoadedDie (new int[]{3,1300,6000,300000,760000,1210000,7650000});
			planetDie = new LoadedDie (new int[]{30,15,15,15,15,15});
			numStarsPerSystem = new LoadedDie (new int[]{39, 20, 30, 10, 1});
			numPlanetsPerStar = new LoadedDie (new int[]{10, 20, 30, 40, 50,60,70,80,90,100,60,40,20,10,8,5,4,3,2,1});
			
		}


	}
}
