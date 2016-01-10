using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class Job
	{
		public JobState state = JobState.Unassigned;
		public List<string> tasks;
		public string requiredSkill {
			get {
				return tasks [0];
			}
		}
		//public Thing targetThing;
		public Worker assignedWorker;
		public Job (List<string> RequiredTasks, Thing TargetThing)
		{
			tasks = RequiredTasks;
			//targetThing = TargetThing;
			//targetThing.assignedJob = this;
			//targetThing.Exposer.AddPersistent (this);
		}
		public void AssignWorker (Worker AssignedWorker)
		{
			assignedWorker = AssignedWorker;
			state = JobState.Waiting;
		}
		public void ClearWorker ()
		{
			assignedWorker = null;
			state = JobState.Unassigned;
		}
		public void Work (int Amount)
		{
			if (state != JobState.Success) {
				state = JobState.Working;
				//targetThing.Work (Amount);
			}
		}
		public void ReportWorkSuccess ()
		{
			state = JobState.Success;
			Destroy ();
		}
		public void Destroy ()
		{			
			assignedWorker.ReportSuccess ();
			assignedWorker = null;
			//targetThing.assignedJob = null;
			JobManager.AddJobToRemovalList (this);
			//targetThing.Exposer.RemovePersistent (this);
		}
		public override string ToString ()
		{
			string name = "None";
			if (assignedWorker != null) {
				name = assignedWorker.actor.Name;
			}
			return string.Format ("Job:   requiredSkill={0}      assignedWorker={1}", requiredSkill, name);
		}
	}
}