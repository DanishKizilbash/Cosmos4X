using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace Cosmos
{

	public abstract class CelestialBody:Entity
	{
		public CelestialBodyType type;
		public CelestialBody parent;
		public OrbitEntity orbit;
		public Coord coord {
			get {
				return system.coord;
			}
		}
		//
		public float temperature;
		public virtual Entity Init (string defID, CelestialBodyType Type, float Mass, float Diameter, PlanetarySystem System, CelestialBody parent=null)
		{

			type = Type;
			if (parent == null) {
				orbit = new OrbitEntity (this, Mass, Diameter, Vector3.zero);
			} else {
				orbit = new OrbitEntity (this, Mass, Diameter, parent.orbit, parent.orbit.Barycenter);
			}
			Entity entity = base.Init (defID);
			ChangeSystem (System);
			return entity;
		}
		public override void Print ()
		{
			base.Print ();
			Debug.Log ("Body Type: " + type);
			Debug.Log ("Temperature:" + temperature);
		}
		public override string DefaultID ()
		{
			return "CelestialBody_Star_M_Default";
		}
		public override Vector3 Position {
			get {
				return Center;
			}
			set {
				MoveTo (value);
			}
		}
		public void Move ()
		{
			Vector3 offset = Vector3.zero;
			if (parent != null) {
				offset += parent.Position;
			}
			this.MoveTo (offset + orbit.currentPosition);
		}

	}
}