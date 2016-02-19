using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace Cosmos
{
	public class OrbitEntity
	{
		public CelestialBody Body;
		//
		public float Mass;
		public float EquatorialDiameter;
		public float Volume;
		public float Density;
		//
		public float OrbitalPeriod;
		public float RotationPeriod;
		public float OrbitalSpeed;
		public float Apoapsis;
		public Vector3 Barycenter;
		//
		private OrbitEntity parent;
		public OrbitEntity Parent {
			get {
				return parent;
			}
			set {
				if (value != parent) {
					CalculateOrbitProperties ();
					parent = value;
				}
			}
		}
		//
		public float CurAngle;
		//
		public Vector3 currentPosition {
			get {
				return getCurrentPosition ();
			}
		}
		//
		private VectorLine debugLine;
		//
		public OrbitEntity (CelestialBody body, float mass, float diameter, OrbitEntity iparent, Vector3 barycenter)
		{
			parent = iparent;
			Init (body, mass, diameter, Barycenter);
		}
		public OrbitEntity (CelestialBody body, float mass, float diameter, Vector3 barycenter)
		{
			Init (body, mass, diameter, Barycenter);
		}
		private void Init (CelestialBody body, float mass, float diameter, Vector3 barycenter)
		{
			Body = body;
			Mass = mass;
			EquatorialDiameter = diameter;
			Barycenter = barycenter;
			CalculateBaseProperties ();
			CalculateOrbitProperties ();		
			debugLine = new VectorLine ("Debug_Orbit_" + Body.Name, new Vector3[90], null, 1.0f);
		}
		private void CalculateBaseProperties ()
		{
			Volume = MathI.SphereVolume ((EquatorialDiameter / 2));
			Density = Mass / Volume;
		}
		private void CalculateOrbitProperties ()
		{
			if (Parent == null) {
				Apoapsis = 0;
			} else {
				Apoapsis = GalaxyManager.orbitApoapsis [Body.type].Rand () + EquatorialDiameter * 2 + parent.EquatorialDiameter * 2;
				Barycenter = Parent.Barycenter;
			}
			CurAngle = Random.Range (0, 360);
		
		}
		private Vector3 getCurrentPosition ()
		{
			return MathI.RotateVector (Barycenter + new Vector3 (0, Apoapsis, 0), Barycenter, CurAngle);
		}
		public void ShowOrbit (bool visible=true)
		{	
			debugLine.active = visible;
			if (visible) {
				debugLine.MakeCircle (Parent.Body.Center, Vector3.forward, Vector3.Magnitude (Parent.Body.Center - Body.Center));
				debugLine.Draw3DAuto ();
			}
		}
		public void Print ()
		{
			Debug.Log ("Mass:" + Mass);
			Debug.Log ("Diameter:" + EquatorialDiameter);
			Debug.Log ("Volume:" + Volume);
			Debug.Log ("Density:" + Density);
			Debug.Log ("Apoapsis:" + Apoapsis);
		}
		public void Destroy ()
		{
			VectorLine.Destroy (ref debugLine);
			Body = null;
			parent = null;
		}
	}
}
