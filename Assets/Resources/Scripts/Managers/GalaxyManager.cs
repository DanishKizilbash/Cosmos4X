using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public enum CelestialBodyType
	{
		Star,
		Planet,
		Moon,
		Asteroid
	}
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

	public static class GalaxyManager
	{		
		//Dice
		public static LoadedDie stellarDie;
		public static  LoadedDie planetDie;
		public static LoadedDie asteroidDie;
		public static LoadedDie moonDie;
		//


		public static void Init ()
		{
			CreateDice ();	
			EntityManager.AddPlanetarySystem (1000);
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
			
		}
	}
}
