using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI;

namespace Cosmos
{
	public class ActorHuman : Actor
	{
		public override void AddBrain ()
		{
			brain = new BrainHuman ();
			brain.Initiate (this, 10);
		}
		public override void AddAnatomy ()
		{
			anatomy = new Anatomy ();
			anatomy.Initiate (this, Parser.StringToFloat (def.GetAttribute ("Life")));
			anatomy.AddLimb ("Head", 5);
			anatomy.AddLimb ("Torso", 15);
			anatomy.AddLimb ("LeftArm", 5);
			anatomy.AddLimb ("RightArm", 5);
			anatomy.AddLimb ("LeftLeg", 5);
			anatomy.AddLimb ("RightLeg", 5);
			//Exposer.AddPersistent (anatomy);
		}
	}
}