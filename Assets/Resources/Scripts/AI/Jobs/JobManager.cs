using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class JobManager
	{
		public static List<Job> jobWorkingList = new List<Job> ();
		public static List<Job> jobQueList = new List<Job> ();
		public static List<Job> jobRemovalList = new List<Job> ();
		public static List<Worker> workerList = new List<Worker> ();
		public static void Update ()
		{
			if (jobQueList.Count > 0) {
				AssignWorkerToNextJob ();
			}
			ClearRemovalList ();
		}
		public static void AssignWorkerToNextJob ()
		{
			Job nextJob = jobQueList [0];
			Worker availableWorker = GetWorker (nextJob.requiredSkill);
			if (availableWorker != null) {
				availableWorker.AddJob (nextJob);
				jobWorkingList.Add (nextJob);
				jobQueList.RemoveAt (0);
			} else {
				//Move to end of que;
				jobQueList.RemoveAt (0);
				jobQueList.Add (nextJob);
			}
		}
		public static Worker GetWorker (string skill)
		{
			Worker targetWorker = null;
			foreach (Worker worker in workerList) {
				if (worker.currentJob == null) {
					Skill requiredSkill = worker.GetSkill (skill);
					if (requiredSkill.enabled) {
						targetWorker = worker;
						break;
					}
				}
			} 
			return targetWorker;
		}
		public static void ClearRemovalList ()
		{
			foreach (Job job in jobRemovalList) {
				jobWorkingList.Remove (job);
			}
		}
		public static void AddJobToRemovalList (Job job)
		{
			jobRemovalList.Add (job);
		}
		public static void AddWorker (Worker newWorker)
		{
			workerList.Add (newWorker);
		}
		public static void RemoveWorker (Worker removedWorker)
		{
			workerList.Remove (removedWorker);
		}
		public static void ReportFailure (Job job)
		{
			//Debug.Log ("failed job");
		}
		public static void ReportSuccess (Job job)
		{
			//Debug.Log ("job completed");
		}
		public static void AddJob (Thing TargetThing, List<string> tasks)
		{
			/*
			if (TargetThing.assignedJob == null) {
				if (TargetThing.workRequired > 0) {
					jobQueList.Add (new Job (tasks, TargetThing));
				}
			}
			*/
		}
		public static void AddJob (Thing TargetThing, string task)
		{
			/*
			if (TargetThing.assignedJob == null) {
				if (TargetThing.workRequired > 0) {
					List<string> tasks = new List<string> ();
					tasks.Add (task);
					jobQueList.Add (new Job (tasks, TargetThing));
				}
			}
			*/
		}
	}
}