using UnityEngine;
using System.Collections;
namespace Cosmos.AI
{
	public abstract class Task
	{
		//
		public Brain brain;
		public object parent;
		public State state = State.Idle;
		public object input = null;
		public object output = null;
		//
		public virtual Task Initiate (object iparent, Brain ibrain)
		{
			parent = iparent;
			brain = ibrain;
			state = State.Idle;
			return this;
		}
		public abstract void Execute ();
		public override string ToString ()
		{
			return this.GetType ().Name;
		}
	}

}