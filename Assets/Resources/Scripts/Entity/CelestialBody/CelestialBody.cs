using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace Cosmos
{

	public abstract class CelestialBody:Entity
	{
		public CelestialBodyType type;
		public OrbitEntity orbit;
		//
		public Database attributes;
		public float temperature;
		public Entity Init (string defID, CelestialBodyType Type, float Mass, float Diameter, CelestialBody parent=null, Coord coord=null)
		{
			type = Type;
			if (parent == null) {
				orbit = new OrbitEntity (Mass, Diameter, coord);
			} else {
				orbit = new OrbitEntity (Mass, Diameter, parent.orbit, coord);
			}
			return base.Init (defID);
		}
		public override void SetAttributes ()
		{
			base.SetAttributes ();
			attributes.Add (0, "temperature");
		}
		public override string DefaultID ()
		{
			return "";
		}


	}
}