using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class PlanetarySystem
	{
		List<CelestialBody> celestialBodies;
		public int systemSeed;
		//
		public float totalMass;
		public Coord systemBarycenter;
		//
		public int numStars;
		public PlanetarySystem (Coord position=null)
		{

			if (position == null) {
				RandomizeSystemCoord ();
			}
			GenerateSystem ();

		}
		private void RandomizeSystemCoord ()
		{
			systemBarycenter = new Coord (0, 0, 0, 0);
		}
		private void RandomizeSystemProperties (int numStars)
		{

		}

		private void GenerateSystem ()
		{
			//92%-97% stellar mass
			float stellarMass = (Random.Range (0, 5) / 100 + 0.92f) * totalMass;
			systemSeed = MathI.GenSeed (9);
			Random.seed = systemSeed;
			//Gen Stars - Probability: 1=37%, 2=33%, 3= 27%, 4=3%
			int starsStat = Mathf.FloorToInt (Mathf.Sqrt (Random.Range (15, 48)));
			numStars = 7 - starsStat;
			RandomizeSystemProperties (numStars);
			//Gen total Mass
			//sol mass = 1.989*10^30
			//totalMass = Mathf.Floor (Mathf.Pow (Random.Range (0.1f, 4f), 9) / 50);
			totalMass = GalaxyManager.stellarDie.NextValue ();
			//Debug.Log (totalMass);
			//star mass : Min = 10% of sol, Max= 15000% of sol.
			//

			//
		}

	}
}
