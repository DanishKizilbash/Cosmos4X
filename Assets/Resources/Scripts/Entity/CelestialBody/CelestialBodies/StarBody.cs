using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class StarBody : CelestialBody
	{
		public StellarClass stellarClass;
		public float luminosity;
		public StellarColor color;
		public override void Print ()
		{
			base.Print ();
			Debug.Log ("Stellar Class: " + stellarClass);
			Debug.Log ("Luminosity:" + luminosity);
			Debug.Log ("Color:" + color);
			orbit.Print ();
		}
		public override string DefaultID ()
		{
			return "CelestialBody_Star_M_Default";
		}
	}

}
