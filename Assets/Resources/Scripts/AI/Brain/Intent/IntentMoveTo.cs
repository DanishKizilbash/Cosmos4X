using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos.AI
{
	public class IntentMoveTo : Intent
	{
		public override Task[] PrepareChildren ()
		{
			selector = "Sequence";
			List<Task> childList = new List<Task> ();
			childList.Add (new TaskSetTargetPosition ().Initiate (this, brain));
			childList.Add (new TaskManueverToPosition ().Initiate (this, brain));
			return childList.ToArray ();
		}
	}
}
