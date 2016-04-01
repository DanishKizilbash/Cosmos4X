using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI;
using Vectrosity;

namespace Cosmos
{
	public abstract class Actor:Thing
	{
		public Brain brain;
		public Anatomy anatomy;
		//
		public Worker worker;
		public Dictionary<string,Skill> skills;
		//
		public Vector3 targetPos;
		private VectorLine targetPosLine;
		public override string Name {
			get {
				return base.Name;
			}
			set {
				if (base.Name != null) {
					Finder.actorDatabase.UpdateKey (base.Name, value);
				}
				base.Name = value;
			}
		}
		public bool isHalted {
			get {
				bool velHalted = physicsObject.velocity.magnitude == 0;
				bool angVelHalted = physicsObject.angularVelocity.magnitude == 0;
				return velHalted && angVelHalted;
			}
		}

		public override Entity Init (string defID)
		{
			base.Init (defID);
			AddBrain ();
			Interval = brain.Interval;
			AddAnatomy ();
			SetLateAttributes ();
			constructionPointsRequired += anatomy.GetConstructionCost ();
			worker = new Worker (this);
			Finder.actorDatabase.Add (this);
			displayDepth = 10;
			targetPosLine = new VectorLine ("TargetPos_" + this.Name, new Vector3[2], null, 1.0f);
			targetPosLine.color = Color.red;
			return this;
		}
		public abstract void AddBrain ();
		public abstract void AddAnatomy ();
		public override void SetLateAttributes ()
		{
			anatomy.MaxHunger = Parser.StringToFloat (def.GetAttribute ("Hunger"));
			anatomy.MaxThirst = Parser.StringToFloat (def.GetAttribute ("Thirst"));
			anatomy.Hunger = anatomy.MaxHunger;
			anatomy.Thirst = anatomy.MaxThirst;
			skills = SkillsTemplate.GetRandomizedTemplate ();
		}

		public override void Tick ()
		{
			//Movement
			base.Tick ();
			if (isConstructed) {
				if (anatomy.Life <= 0) {
					Die ();
				}
			} 
			TickRequired = true;
		}
		public override void OnSelected (bool selected)
		{
			base.OnSelected (selected);
		}
		public virtual void drawTargetPosLine ()
		{
			targetPosLine.points3 [0] = Center;
			targetPosLine.points3 [1] = targetPos;
			targetPosLine.Draw ();
		}
		public virtual void ComeToHalt ()
		{
			if (!isHalted) {
				List<Limb> thrusterLimbs = anatomy.GetLimbs ("Thruster");
				physicsObject.ClearForces ();
				foreach (Limb thrusterLimb in thrusterLimbs) {
					ThrusterLimb limb = (ThrusterLimb)thrusterLimb;
					float newThrottle = 0f;
					if (limb.isRCS) {
						physicsObject.angularVelocity *= 0.98f;
					} else {
						physicsObject.velocity *= 0.98f;
					}
				}
			}
			if (Mathf.Abs (physicsObject.angularVelocity.magnitude) < 0.005f) {
				physicsObject.angularVelocity = Vector3.zero;
				if (Mathf.Abs (physicsObject.velocity.magnitude) < 0.005f) {
					physicsObject.velocity = Vector3.zero;
				}
			}
		}
		public override void MoveTo (Vector3 iposition)
		{
			base.MoveTo (iposition);
			if (isSelected && !isHalted) {
				drawTargetPosLine ();			
				targetPosLine.active = true;
			} else {
				targetPosLine.active = false;
			}
		}
		public virtual void MoveTowards (Vector3 target)
		{
			target.z = Position.z;
			Vector3 targetVector = GetThrustVector (target);
			ThrottleThrusters (targetVector, target);
		}
		public virtual void ThrottleThrusters (Vector3 targetThrust, Vector3 targetPos)
		{
			float throttleMaxAngle = 15.0f;
			GameManager.DrawLine (Position + physicsObject.centerOfMass, Position + physicsObject.centerOfMass + targetThrust * 200, Color.yellow);
			//
			//float angleToTarget = Mathf.Atan2 (target.y - pos.y, target.x - pos.x);
			//
			List<Limb> thrusterLimbs = anatomy.GetLimbs ("Thruster");
			physicsObject.ClearForces ();
			foreach (Limb thrusterLimb in thrusterLimbs) {
				ThrusterLimb limb = (ThrusterLimb)thrusterLimb;
				float newThrottle = 0f;
				float targetAngle = Mathf.Rad2Deg * Mathf.Atan2 (targetThrust.y, targetThrust.x);
				float myAngle = MathI.DegToQuat (rotation);
				float angleToTarget = Mathf.DeltaAngle (targetAngle, myAngle);
				if (limb.isRCS) {
					//Rotation control
					float currentAngVel = physicsObject.angularVelocity.z;
					float currentG = currentAngVel / PhysicsManager.GValue;
					Vector3 maxTorque = physicsObject.ComputeTorque (limb.force, limb.offset);
					float limbMaxAngVel = physicsObject.ComputeAngularVelocity (maxTorque).z;
					//
					bool accelerate = true;
					float velIntervalsToTarget = (angleToTarget / currentAngVel);
					float torqueIntervalsToNillVel = (currentAngVel / limbMaxAngVel);
					//If can deccel in time and and moving towards target angle
					if (Mathf.Abs (velIntervalsToTarget) < Mathf.Abs (torqueIntervalsToNillVel) + 30 && Mathf.Abs (angleToTarget + currentAngVel) < Mathf.Abs (angleToTarget)) {
						accelerate = false;
					}
					//
					float newVel = currentAngVel + limbMaxAngVel;
					if (accelerate) {
						if (Mathf.Abs (Mathf.DeltaAngle (myAngle + newVel, targetAngle)) < Mathf.Abs (Mathf.DeltaAngle (myAngle + currentAngVel, targetAngle))) {
							if (newVel / PhysicsManager.GValue < anatomy.GLimit) {
								float distThrottle = Mathf.Clamp (Mathf.Abs (angleToTarget / 90), 0.05f, 1);
								newThrottle = distThrottle;
							} else {
								newThrottle = limb.throttle / 2;
							}
						}
					} else {
						if (Mathf.Abs (newVel) < Mathf.Abs (currentAngVel)) {
							//newThrottle = Mathf.DeltaAngle (myAngle + newVel, targetAngle) / throttleMaxAngle;
							newThrottle = torqueIntervalsToNillVel / velIntervalsToTarget;
						}
					}
				} else {
					//Main Thrusters
					Vector3 maxThrust = physicsObject.ComputeRelativeForce (limb.force);
					angleToTarget = Mathf.Abs (angleToTarget);				
					float distToTarget = (targetPos - Position).magnitude;
					float distanceThrottle = 1;

					//Throttle based of angle offset from thrust vector
					if (angleToTarget > 0) {
						if (angleToTarget > 90f) {
							newThrottle = (throttleMaxAngle - (angleToTarget - 90f)) / throttleMaxAngle;
						} else if (angleToTarget < 90f) {
							newThrottle = (90 - (angleToTarget - (throttleMaxAngle - 90))) / throttleMaxAngle;
						} else {
							newThrottle = 1f;
						}
					}

					newThrottle *= distanceThrottle;
				}
				if (newThrottle < 0f) {
					newThrottle = 0f;
				}
				if (newThrottle > 1f) {
					newThrottle = 1f;
				}
				limb.throttle = newThrottle;
				if (newThrottle != 0) {
					physicsObject.AddConstantForce (new Force (limb.force * limb.throttle, limb.offset));
				}
			}
		}
		public virtual Vector3 GetThrustVector (Vector3 target)
		{
			Vector3 velVector = physicsObject.velocity;
			float vel = physicsObject.velocity.magnitude;
			Vector3 pos = Position + physicsObject.centerOfMass;
			//<Determine Thrust State>
			//Thrust state 0 = Deccelerate
			//Thrust state 1 = Accelerate
			float distToTarget = (target - pos).magnitude;
			Vector3 targetVector = (target - pos);
			targetVector.Normalize ();
			//
			List<Limb> thrusterLimbs = anatomy.GetLimbs ("Thruster");
			Vector3 totalThrustVector = new Vector3 (0, 0, 0);
			foreach (Limb thrusterLimb in thrusterLimbs) {
				ThrusterLimb limb = (ThrusterLimb)thrusterLimb;
				if (!limb.isRCS) {
					totalThrustVector += limb.force;
				}
			}
			//
			bool accelerate = true;
			int velIntervalsToTarget = Mathf.FloorToInt (distToTarget / vel);
			int accIntervalsToNillVel = Mathf.CeilToInt (vel / (totalThrustVector.magnitude / physicsObject.mass));
			float turningTimeBuffer = 50;
			//If can deccel in time and and moving towards target 
			float velAngleToTarget = Vector3.Angle (velVector, targetVector);
			// if it takes longer to slow down than it takes to reach the target (and moving towards target)
			if ((Mathf.Abs (velIntervalsToTarget) < 1.5f * Mathf.Abs (accIntervalsToNillVel) + turningTimeBuffer && Mathf.Abs ((target - pos + physicsObject.velocity).magnitude) > Mathf.Abs (distToTarget))) {
				accelerate = false; 
			}
			//if vel offset too much
			if ((velAngleToTarget > 45 && Mathf.Abs (vel) > 0.01f)) {
				accelerate = false;
			}

			if (accelerate) {
				return targetVector;
			} else {
				return -1 * velVector;				
			}			
		}
		
		public int GetSkillValue (string skillName)
		{
			Skill value = null;
			skills.TryGetValue (skillName, out value);
			if (value != null) {
				return value.level;
			}
			return 0;
		}
		public Skill GetSkill (string skillName)
		{
			Skill value = null;
			skills.TryGetValue (skillName, out value);
			return value;
		}
		public virtual void Die ()
		{
			if (!anatomy.Invincible) {
				Debug.Log (ToString () + " died");
				Destroy ();
			}
		}
		public override void Destroy ()
		{
			TickManager.RemoveTicker (this.brain);
			base.Destroy ();
		}
		public override string DefaultID ()
		{
			return "Thing_Actor_Ship_Escort";
		}
	}
}