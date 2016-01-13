﻿using UnityEngine;
using System.Collections;

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
		public Vector3 directionVector {
			get {
				float fixedRot = Mathf.Deg2Rad * ((thing.rotation + 90) % 360);
				float xComp = Mathf.Cos (fixedRot);
				float yComp = Mathf.Sin (fixedRot);
				return new Vector3 (xComp, yComp, 0);
			}
		}
		public PhysicsObject (Thing target, Vector3 Dimensions, float Mass =100f)
		{
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
		public void ApplyThrust (float x, float y, float z, Vector3 offset, bool relativeToRotation=true)
		{
			ApplyThrust (new Vector3 (x, y, z), offset, relativeToRotation);
		}
		public void ApplyThrust (Vector3 thrustVector, Vector3 offset, bool relativeToRotation=true)
		{			
			ComputeForceAndTorque (thrustVector, offset, relativeToRotation);
		}
		public void UpdatePosition ()
		{
			thing.Position += deltaPos;
			thing.rotation += deltaRot;
			deltaPos = Vector3.zero;
			deltaRot = 0f;
		}
		public void Update ()
		{
			ApplyFriction ();
			deltaPos += velocity;
			deltaRot += angularVelocity.z;
			UpdatePosition ();
			//Vel
			Debug.DrawLine (centerOfMass + thing.Position, thing.Position + (velocity * 100), Color.white);
			//COM
			Debug.DrawLine (centerOfMass + thing.Position + Vector3.up * 0.1f, centerOfMass + thing.Position - Vector3.up * 0.1f, Color.blue);
			Debug.DrawLine (centerOfMass + thing.Position + Vector3.left * 0.1f, centerOfMass + thing.Position - Vector3.left * 0.1f, Color.blue);
			//Debug.Log ("Velocity(x100) : " + (velocity * 100) + " || Angular Velocity(x100) " + (angularVelocity * 100));
		}
		void ComputeForceAndTorque (Vector3 force, Vector3 point, bool relativeToRotation)
		{		
			Vector3 torque = Vector3.Cross (point, force);
			//Debug.Log ("force(x100) : " + (force * 100) + ", Offset " + (point) + " and MOA : " + momentOfInertia + " = torque " + torque * 100);

			Vector3 tPt = MathI.RotateVector (centerOfMass + point + thing.Position, thing.Position + centerOfMass, thing.rotation);

			if (relativeToRotation) {
				force = MathI.RotateVector (force, Vector3.zero, thing.rotation);
				point = MathI.RotateVector (point, centerOfMass, thing.rotation);
			}
			velocity += force / mass;
			angularVelocity += torque / momentOfInertia;

			//Force

			Debug.DrawLine (tPt, tPt - (force * 50), Color.red);
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
			momentOfInertia = m * (w * w + h * h) / 12;
		}
		private void CalculateCenterOfMass ()
		{
			centerOfMass = new Vector3 (0.5f, -0.5f, 0.0f);
			thing.rotationPoint = centerOfMass;
		}
	}
}