using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public class OrbitEntity
	{
		public float Mass;
		public float EquatorialDiameter;
		public float Volume;
		public float Density;
		//
		public float OrbitalPeriod;
		public float RotationPeriod;
		public float OrbitalSpeed;
		public float OrbitalEccentricity;
		public float Apoapsis;
		public float Periapsis;
		public Coord Barycenter;
		public OrbitEntity Parent;
		//
		public OrbitEntity (float mass, float diameter, OrbitEntity parent, Coord Barycenter)
		{

			Mass = mass;
			EquatorialDiameter = diameter;
			Parent = parent;
			CalculateBaseProperties ();
			CalculateOrbitProperties ();
		}
		public OrbitEntity (float mass, float diameter, Coord Barycenter)
		{
		
		}
		private void CalculateBaseProperties ()
		{
			Volume = MathI.SphereVolume ((EquatorialDiameter / 2));
			Density = Mass / Volume;
		}
		private void CalculateOrbitProperties ()
		{
		
		}
	}
}
