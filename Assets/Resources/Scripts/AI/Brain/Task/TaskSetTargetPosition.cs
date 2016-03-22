using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI.Pathfinding;

namespace Cosmos.AI
{
	public class TaskSetTargetPosition:Task
	{
		public override void Execute ()
		{
			if (input == null) {
				state = State.Failure;
			} else {
				try {
					Vector3 inp = ((Vector3)input);
					state = State.Success;
					output = inp;
					brain.actor.targetPos = inp;
				} catch (InvalidCastException) {
					state = State.Failure;
				}
			}
		}
	}
}