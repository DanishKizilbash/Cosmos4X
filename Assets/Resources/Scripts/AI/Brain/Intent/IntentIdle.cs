using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos.AI
{
	public class IntentIdle : Intent
	{
		public override Task[] PrepareChildren ()
		{
			selector = "Random";
			curChild = 0;
			List<Task> childList = new List<Task> ();
			TaskWait tempTask = (TaskWait)new TaskWait ().Initiate (this, brain);
			tempTask.waitTime = 5;
			for (int i = 0; i<2; i++) {
				childList.Add ((Task)tempTask);
			}
			childList.Add (new IntentWander ().Initiate (this, brain));

			return childList.ToArray ();
		}
	}
}
