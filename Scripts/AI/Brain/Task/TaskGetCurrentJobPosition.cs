using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Eniso.AI.Pathfinding;

namespace Eniso.AI {
	public class TaskGetCurrentJobPosition:Task {
		public override void Execute () {
			Job job = brain.actor.worker.currentJob;
			if (job != null) {
				output = job.targetThing.Position;
				brain.actor.Exposer.AddNote ("Current Job Position: " + output);
				state = State.Success;
			} else {
				state = State.Failure;
				brain.actor.Exposer.AddNote ("No jobs");
			}

		}
	}
}