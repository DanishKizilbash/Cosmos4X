using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace Cosmos
{

	public abstract class CelestialBody:Entity
	{
		public Colony colony;
		public CelestialBodyType type;
		public CelestialBody parent {
			get {
				if (orbit == null || orbit.Parent == null) {
					return null;
				}
				return orbit.Parent.Body;
			}
			set {
				if (orbit != null) {
					orbit.Parent = value.orbit;
				}
			}
		}
		public OrbitEntity orbit;
		public Coord coord {
			get {
				return system.coord;
			}
		}
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
			ResourceManager.RandomizeStockpile (this);
			Name = system.name + " " + type.ToString () + " " + UnityEngine.Random.Range (0, 999);
			//colony = new Colony (this);

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
		public void Move ()
		{
			Vector3 offset = Vector3.zero;
			if (parent != null) {
				offset += parent.Position;
			}
			this.MoveTo (offset + orbit.currentPosition);
		}
		public override bool InRect (Rect rect)
		{
			float rad = scale.x > scale.y ? scale.x / 2 : scale.y / 2;
			return MathI.CircleRectIntersect (rad, Center, rect);
		}
		public override void OnSelected (bool selected)
		{
			base.OnSelected (selected);
			if (selected) {
				stockpile.Print ();
			}
		}
		public override void Destroy ()
		{
			if (orbit != null) {
				orbit.Destroy ();
			}
			if (colony != null) {
				colony.Destroy ();
			}
			system.RemoveEntity (this);
			base.Destroy ();
		}
	}
}