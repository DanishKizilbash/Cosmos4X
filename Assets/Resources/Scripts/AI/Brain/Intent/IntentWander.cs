using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos.AI
{
	public class IntentWander : Intent
	{
		public override Task[] PrepareChildren ()
		{
			selector = "Sequence";
			curChild = 0;
			List<Task> childList = new List<Task> ();
			/*
			childList.Add (new TaskGetFreeNeighbours ().Initiate (this, brain));
			childList.Add (new ModifierGetRandom ().Initiate (this, brain));
			childList.Add (new IntentMoveTo ().Initiate (this, brain));
			*/
			return childList.ToArray ();

		}
	}
}
