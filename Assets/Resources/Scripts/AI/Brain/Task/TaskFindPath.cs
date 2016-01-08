using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI.Pathfinding;

namespace Cosmos.AI
{
	public class TaskFindPath:Task
	{
		public override void Execute ()
		{
			if (input == null) {
				state = State.Failure;
			} else {
				Vector3 inp = ((Vector3)input);
				Vector3 target = new Vector3 (inp.x, inp.y, inp.z);
				List<Vector3> VectorList = PathFinder.FindPath (brain.actor.Position, target);
				if (VectorList.Count > 0) {
					state = State.Success;
					output = target;
					//brain.actor.MovementPath = tileList;
				} else {
					state = State.Failure;
				}
			}
		}
	}
}