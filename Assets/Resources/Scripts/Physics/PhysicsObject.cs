using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class PhysicsObject
	{
		private Vector3 deltaPos;
		private float deltaRot;
		public Thing thing;
		public float mass;
		public Vector2 dimensions;
		public Vector3 velocity;
		public Vector3 angularVelocity;
		public Vector3 centerOfMass;//ratio 0,0,0 to 1,1,1
		public float localDrag;
		public float dragCoeff;
		public float momentOfInertia;
		//
		public List<Force> constantForces;
		public Vector3 directionVector {
			get {
				if (thing != null) {
					float fixedRot = Mathf.Deg2Rad * ((thing.rotation + 90) % 360);
					float xComp = Mathf.Cos (fixedRot);
					float yComp = Mathf.Sin (fixedRot);
					return new Vector3 (xComp, yComp, 0);
				}
				return Vector3.zero;
			}
		}
		public PhysicsObject (Thing target, Vector3 Dimensions, float Mass =100f)
		{
			ClearForces ();
			thing = target;
			deltaPos = new Vector3 (0, 0, 0);
			velocity = new Vector3 (0, 0, 0);
			angularVelocity = new Vector3 (0, 0, 0);
			centerOfMass = new Vector3 (0, 0, 0);
			localDrag = 0;
			dragCoeff = 1;
			mass = Mass;
			dimensions = Dimensions;
			CalculateCenterOfMass ();
			CalculateMomentOfInertia ();
			PhysicsManager.Add (this);
		}
		public void ApplyForce (float x, float y, float z, Vector3 offset, bool relativeToRotation=true)
		{
			ApplyForce (new Vector3 (x, y, z), offset, relativeToRotation);
		}
		public void ApplyForce (Vector3 thrustVector, Vector3 offset, bool relativeToRotation=true)
		{			
			if (thing != null) {
				Vector3 point = offset;
				Vector3 force = thrustVector;
				if (relativeToRotation) {
					ApplyForceAndTorque (ComputeRelativeForce (thrustVector), ComputeTorque (thrustVector, offset));
					force = ComputeRelativeForce (thrustVector);
				} else {
					ApplyForceAndTorque (thrustVector, ComputeTorque (thrustVector, offset));
				}
				Vector3 tPt = MathI.RotateVector (thing.Position + centerOfMass + point, thing.Position + centerOfMass, thing.rotation);
				GameManager.DrawLine (tPt, tPt - (force * 10), Color.red);
			}

		}
		public void UpdatePosition ()
		{
			if (thing != null) {
				thing.Position += deltaPos;
				thing.rotation += deltaRot;
				deltaPos = Vector3.zero;
				deltaRot = 0f;
			} else {
				Destroy ();
			}
		}
		public void Update ()
		{
			if (thing != null) {
				foreach (Force force in constantForces) {
					ApplyForce (force.vector, force.offset);
				}
				ApplyFriction ();
				deltaPos += velocity;
				deltaRot += angularVelocity.z;
				UpdatePosition ();
				//Vel
				if (velocity.magnitude > 0.01f) {
					GameManager.DrawLine (centerOfMass + thing.Position, centerOfMass + thing.Position + (velocity * 10), Color.white);
				}
				//COM
				GameManager.DrawLine (centerOfMass + thing.Position + Vector3.up * 0.1f, centerOfMass + thing.Position - Vector3.up * 0.1f, Color.blue);
				GameManager.DrawLine (centerOfMass + thing.Position + Vector3.left * 0.1f, centerOfMass + thing.Position - Vector3.left * 0.1f, Color.blue);
				//Debug.Log ("Velocity(x100) : " + (velocity * 100) + " || Angular Velocity(x100) " + (angularVelocity * 100));
			}
		}
		public Vector3 ComputeTorque (Vector3 force, Vector3 point)
		{		
			Vector3 torque = Vector3.Cross (point, force);
			return torque;
		}
		public Vector3 ComputeRelativeForce (Vector3 force)
		{
			if (thing != null) {
				return MathI.RotateVector (force, Vector3.zero, thing.rotation);
			}
			return Vector3.zero;
		}
		public Vector3 ComputeAngularVelocity (Vector3 torque)
		{
			return  torque / momentOfInertia;
		}
		private void ApplyForceAndTorque (Vector3 force, Vector3 torque)
		{
			velocity += force / mass;
			angularVelocity += ComputeAngularVelocity (torque);

		}

		private void ApplyFriction ()
		{
			if (localDrag > 1) {
				localDrag = 1;
			}
			//velocity *= dragCoeff * localDrag;
			//angularVelocity *= localDrag;
		}
		private void CalculateMomentOfInertia ()
		{
			float m = mass;
			float w = dimensions.x;
			float h = dimensions.y;
			momentOfInertia = m * (w * w + h * h) / 24;//12
		}
		private void CalculateCenterOfMass ()
		{
			centerOfMass = new Vector3 (0.5f, -0.5f, 0.0f);
			thing.rotationPoint = centerOfMass;
		}
		public void AddConstantForce (Force force)
		{
			constantForces.Add (force);			
		}
		public void ClearForces ()
		{
			constantForces = new List<Force> ();
		}
		public void Destroy ()
		{
			PhysicsManager.Remove (this);
			thing = null;
		}
	}
}