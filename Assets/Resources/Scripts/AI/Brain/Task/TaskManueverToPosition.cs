using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos.AI
{
	public class TaskManueverToPosition:Task
	{
		public float distanceBuffer;
		public override void Execute ()
		{
			distanceBuffer = brain.actor.scale.magnitude * 2;
			Vector3 targetPos = (Vector3)input;
			//Debug.Log (brain.actor.Position);
			if (Mathf.Abs ((brain.actor.Position - targetPos).magnitude) <= distanceBuffer) {
				brain.actor.ComeToHalt ();
				if (brain.actor.isHalted) {
					output = input;
					state = State.Success;	
				}
			} else {
				brain.actor.MoveTowards (targetPos);
			}

		}
	}
}