using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI.Pathfinding;

namespace Cosmos.AI
{
	public class TaskSetTargetPosition:Task
	{
		public float randomness = 4;
		public override void Execute ()
		{
			if (input == null) {
				state = State.Failure;
			} else {
				try {
					float rndOffset = randomness / 2;
					Vector3 tVec = (Vector3)input;
					Vector3 inp = new Vector3 (tVec.x + UnityEngine.Random.Range (-rndOffset, rndOffset), tVec.y + UnityEngine.Random.Range (-rndOffset, rndOffset), brain.actor.Position.z);
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