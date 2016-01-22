using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		public OrbitEntity Parent;
		//
		public float CurAngle;

		public Vector3 currentPosition {
			get {
				return getCurrentPosition ();
			}
		}
		public OrbitEntity (CelestialBody body, float mass, float diameter, OrbitEntity parent, Vector3 barycenter)
		{
			Parent = parent;
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
				Apoapsis = GalaxyManager.orbitApoapsis [Parent.Body.type].Rand ();
			}
			CurAngle = Random.Range (0, 360);
		
		}
		private Vector3 getCurrentPosition ()
		{
			return MathI.RotateVector (Barycenter + new Vector3 (0, Apoapsis, 0), Barycenter, CurAngle);
		}
		public void Print ()
		{
			Debug.Log ("Mass:" + Mass);
			Debug.Log ("Diameter:" + EquatorialDiameter);
			Debug.Log ("Volume:" + Volume);
			Debug.Log ("Density:" + Density);
			Debug.Log ("Apoapsis:" + Apoapsis);
		}
	}
}
