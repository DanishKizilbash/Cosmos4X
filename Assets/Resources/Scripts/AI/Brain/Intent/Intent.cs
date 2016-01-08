using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos.AI
{
	public abstract class Intent : Task
	{
		//
		public string selector = "Sequence";
		public Task[] children;
		public int curChild;
		//
		public override Task Initiate (object iparent, Brain ibrain)
		{
			parent = iparent;
			brain = ibrain;
			Reset ();
			return base.Initiate (iparent, ibrain);
		}

		public override void Execute ()
		{
			//brain.actor.Exposer.AddNote (this.ToString ());
			if (state != State.Idle && state != State.Running) {
				Reset ();
			}
			ExecuteChildren ();
		}

		public virtual void ExecuteChildren ()
		{
			state = State.Running;
			Task child = null;
			if (selector == "Sequence") {
				child = children [curChild];
				if (curChild == 0) {
					child.input = input;
				}
				child.Execute ();
				if (child.state == State.Success) {
					curChild++;
					if (curChild > children.Length - 1) {
						output = child.output;
						state = State.Success;
					} else {
						children [curChild].input = child.output;
					}
				} else if (child.state == State.Failure) {
					state = State.Failure;
				}
			}
			if (selector == "Satisfy") {
				child = children [curChild];
				if (curChild == 0) {
					child.input = input;
				}
				child.Execute ();
				if (child.state == State.Success) {
					output = child.output;
					state = State.Success;
				} else if (child.state == State.Failure) {
					curChild++;
					if (curChild > children.Length - 1) {
						output = null;
						state = State.Failure;
					} else {
						children [curChild].input = child.output;
					}
				}
			}
			if (selector == "Random") {
				child = children [curChild];
				child.input = input;
				child.Execute ();
				if (child.state != State.Running) {
					curChild = Random.Range (0, children.Length);
					state = child.state;
					output = child.output;
				}
			}

			//brain.actor.Exposer.AddNote (child.ToString ());
			//Debug.Log (state);
		}

		public virtual void Reset ()
		{
			children = PrepareChildren ();
			if (selector == "Random") {
				curChild = Random.Range (0, children.Length);
			} else {
				curChild = 0;
			}
			children [curChild].input = input;
			state = State.Idle;
		}

		public abstract Task[] PrepareChildren ();
	}
}
