using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI;

namespace Cosmos
{
	public class ActorShip : Actor
	{
		public Colony homeColony;

		public override Entity Init (string defID)
		{
			return base.Init (defID);
		}
		public override void AddBrain ()
		{
			brain = new BrainShip ();
			brain.Initiate (this, 10);
		}
		public override void AddAnatomy ()
		{
			anatomy = new Anatomy ();
			anatomy.Initiate (this, Parser.StringToFloat (def.GetAttribute ("Life")));
			anatomy.AddLimb ("Bridge", 5);
			AddThruster (2, new Vector3 (0f, 0.005f, 0f), new Vector3 (-0.5f, -0.5f, 0f));
			AddThruster (2, new Vector3 (0f, 0.005f, 0f), new Vector3 (0.5f, -0.5f, 0f));
			//
			AddThruster (2, new Vector3 (0.05f, 0f, 0f), new Vector3 (-1f, 1f, 0f), true);
			AddThruster (2, new Vector3 (-0.05f, 0f, 0f), new Vector3 (1f, 1f, 0f), true);
			AddThruster (2, new Vector3 (0.05f, 0f, 0f), new Vector3 (-1f, -1f, 0f), true);
			AddThruster (2, new Vector3 (-0.05f, 0f, 0f), new Vector3 (1f, -1f, 0f), true);
			anatomy.Invincible = true;
			//Exposer.AddPersistent (anatomy);
		}
		public void AddThruster (float condition, Vector3 force, Vector3 offset, bool isRCS=false)
		{
			ThrusterLimb thrusterLimb = (ThrusterLimb)new ThrusterLimb ().Init (Limb.Condition.Normal, condition);
			anatomy.AddLimb ("Thruster", condition, thrusterLimb);
			thrusterLimb.force = force;
			thrusterLimb.offset = offset;
			thrusterLimb.isRCS = isRCS;			
		}
		public override void Tick ()
		{
			base.Tick ();
		}
		public override string DefaultID ()
		{
			return "Thing_Actor_Ship_Escort";
		}
	}
}