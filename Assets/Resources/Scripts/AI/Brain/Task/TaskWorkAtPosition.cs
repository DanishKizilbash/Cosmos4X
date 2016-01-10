using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos.AI
{
	public class TaskWorkAtPosition:Task
	{
		private Thing targetThing;
		private Job currentJob;
		public override Task Initiate (object iparent, Brain ibrain)
		{
			targetThing = null;
			return base.Initiate (iparent, ibrain);
		}
		public override void Execute ()
		{
			if (targetThing == null) {
				currentJob = brain.actor.worker.currentJob;
				//targetThing = currentJob.targetThing;
			}
			state = State.Running;
			if (currentJob.state == JobState.Success || currentJob == null) {
				state = State.Success;
			} else if (currentJob.state == JobState.Failed) {
				state = State.Failure;
			} else {	
				currentJob.Work (brain.actor.GetSkillValue (currentJob.requiredSkill));
			}
		}
	}
}