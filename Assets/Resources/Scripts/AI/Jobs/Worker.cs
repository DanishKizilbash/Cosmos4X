using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class Worker
	{
		private List<Job> jobQue = new List<Job> ();		
		//private Thing targetThing = null;
		public Actor actor = null;
		public Job currentJob = null;
		public Worker (Actor myActor)
		{
			actor = myActor;
			actor.worker = this;
			JobManager.AddWorker (this);
		}
		public void Tick ()
		{
			if (currentJob == null) {
				SelectNextJob ();
			} else {
				if (currentJob.state == JobState.Success) {
					SelectNextJob ();
				} else if (currentJob.state == JobState.Failed) {
					ReportFailure ();
					StopCurrentJob ();
					SelectNextJob ();
				} else {
					if (currentJob != null) {
						actor.brain.AddStimulus ("Work");
					}

				}
			}
		}
		public Skill GetSkill (string skill)
		{
			return actor.GetSkill (skill);
		}
		public void StopCurrentJob ()
		{
			currentJob.ClearWorker ();
		}
		public void ReportSuccess ()
		{
			currentJob = null;
			JobManager.ReportSuccess (currentJob);
		}
		public void ReportFailure ()
		{
			JobManager.ReportFailure (currentJob);
		}
		public void SelectNextJob ()
		{
			if (jobQue.Count > 0) {
				currentJob = jobQue [0];
				jobQue.RemoveAt (0);
			} else {
				currentJob = null;
			}
		}
		public void AddJob (Job newJob)
		{
			jobQue.Add (newJob);
			newJob.AssignWorker (this);
		}
	}
}