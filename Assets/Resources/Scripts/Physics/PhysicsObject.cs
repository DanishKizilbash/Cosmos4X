using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public class PhysicsObject
	{
		public Thing thing;
		private Vector3 deltaPos;
		public Vector3 velocity;
		public Vector3 angularVelocity;
		public Vector3 centerOfMass;//ratio 0,0,0 to 1,1,1
		public float localDrag;
		public float dragCoeff;
		public PhysicsObject (Thing target)
		{
			thing = target;
			deltaPos = new Vector3 (0, 0, 0);
			velocity = new Vector3 (0.1f, 0, 0);
			angularVelocity = new Vector3 (0, 0, 0);
			centerOfMass = new Vector3 (0, 0, 0);
			localDrag = 0;
			dragCoeff = 1;
			PhysicsManager.Add (this);
		}
		public void ApplyThrust (float x, float y, float z, Vector3 offset)
		{
			ApplyThrust (new Vector3 (x, y, z), offset);
		}
		public void ApplyThrust (Vector3 thrustVector, Vector3 offset)
		{
			//float angularPercentage = centerOfMass - offset;
			velocity += thrustVector;
		}
		public void UpdatePosition ()
		{
			thing.Position += deltaPos;
			deltaPos = Vector3.zero;
		}
		public void Update ()
		{
			ApplyFriction ();
			deltaPos += velocity;
			UpdatePosition ();
		}
		private void ApplyFriction ()
		{
			if (localDrag > 1) {
				localDrag = 1;
			}
			//velocity *= dragCoeff * localDrag;
			//angularVelocity *= localDrag;
		}
	}
}