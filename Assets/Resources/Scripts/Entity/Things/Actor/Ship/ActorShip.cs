﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI;

namespace Cosmos
{
	public class ActorShip : Actor
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
			anatomy.AddLimb ("Bridge", 5);
			anatomy.Invincible = true;
			//Exposer.AddPersistent (anatomy);
		}
		public override string DefaultID ()
		{
			return "Thing_Actor_Ship_Escort";
		}
	}
}