using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class PlanetBody : CelestialBody
	{
		public PlanetClass planetClass;
		public override void Print ()
		{
			base.Print ();
			Debug.Log ("Planet Class: " + planetClass);
			orbit.Print ();
		}
		public override string DefaultID ()
		{
			return "CelestialBody_Planet_Ocean_Default";
		}
	}

}
