using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class StarBody : CelestialBody
	{
		public StellarClass stellarClass;

		public override void SetAttributes ()
		{
			base.SetAttributes ();
			attributes.Add (0, "Luminosity");
			attributes.Add ("", "Color");
		}
		public override string DefaultID ()
		{
			return "";
		}
	}

}
