using UnityEngine;
using System.Collections;
namespace Cosmos.AI
{
	public class TaskWait:Task
	{
		public int waitTime = 5;
		public override void Execute ()
		{
			if (waitTime > 0) {
				waitTime--;
				//Debug.Log (waitTime);
			} else {
				state = State.Success;
			}
		}
	}
}