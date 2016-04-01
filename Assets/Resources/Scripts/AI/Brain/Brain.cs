using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos.AI
{
	public abstract class Brain:Tickable
	{
		public Actor actor;
		public List<string> cachedstimulus;
		public List<string> stimulus;
		public List<string> cachedcommands;
		public List<string> commands;
		public Dictionary<string,Intent> logic;
		public Intent curIntent = null;
		//
		public int idleTime = 0;
		public int idleTimeMax = 10;
		//
		public virtual void Initiate (Actor iActor, int iSpeed)
		{
			stimulus = new List<string> ();
			commands = new List<string> ();
			cachedstimulus = new List<string> ();
			cachedcommands = new List<string> ();
			logic = new Dictionary<string, Intent> ();
			actor = iActor;
			Interval = iSpeed;
			InitiateLogic ();
			TickManager.AddTicker (this);
		}

		public virtual void InitiateLogic ()
		{
			/*
			logic.Add ("Idle", (Intent)(new IntentIdle ().Initiate (this, this)));			
			logic.Add ("Hunger", (Intent)(new IntentEat ().Initiate (this, this)));
			logic.Add ("Work", (Intent)(new IntentDoWork ().Initiate (this, this)));
			*/
		}

		public override void Tick ()
		{
			TickRequired = true;
			if (actor.isConstructed) {			
				Think ();
				if (curIntent != null) {
					//Debug.Log (curIntent.children [curIntent.curChild].ToString ());
				}
			}
		}

		public virtual void Think ()
		{
			ProcessStimulus ();
			Analyze ();
			ProcessCommands ();
			IssueCommands ();
		}

		public virtual State TickLogic (string logicString, object input=null)
		{
			if (logic.TryGetValue (logicString, out curIntent)) {
				if (curIntent.state == State.Idle) {
					//curIntent.Reset ();
					curIntent.input = input;
					curIntent.state = State.Running;
				}
				curIntent.Execute ();
				if (curIntent.state != State.Running) {
					//Debug.Log (curIntent.output);
					State tState = curIntent.state;
					curIntent.Reset ();
					return tState;

				}
			} else {
				// default state
			}
			if (curIntent != null) {
				return curIntent.state;
			} else {
				return State.Failure;
			}
		}

		public virtual void ProcessStimulus ()
		{
			stimulus.AddRange (cachedstimulus);
			cachedstimulus.Clear ();
		}
		public virtual void ProcessCommands ()
		{			
			commands.AddRange (cachedcommands);
			cachedcommands.Clear ();
		}

		public virtual void AddStimulus (string stim, int priority= int.MaxValue)
		{
			if (priority == int.MaxValue) {
				priority = cachedstimulus.Count;
			}
			if (!stimulus.Contains (stim) && !cachedstimulus.Contains (stim)) {
				cachedstimulus.Insert (priority, stim);
			}
		}

		public virtual void AddCommand (string command, int priority= int.MaxValue)
		{
			if (priority == int.MaxValue) {
				priority = cachedcommands.Count;
			}
			if (!commands.Contains (command) && !cachedcommands.Contains (command)) {
				cachedcommands.Insert (priority, command);
			}
		}

		private object ParseStringToObject (string str)
		{
			//Vector
			Vector3 vector = Parser.StringToVector3 (str);
			if (vector != new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue)) {
				return vector;
			}
			//
			return null;
		}

		public virtual void IssueCommands ()
		{
			if (commands.Count > 0) {
				string currentCommand = commands [0];
				string[] commandStrings = currentCommand.Split (new string[] {"|"}, System.StringSplitOptions.RemoveEmptyEntries);
				string commandString = commandStrings [0];
				string objectString = null;
				object parsedInput = null;
				if (commandStrings.Length > 1) {
					objectString = commandStrings [1];			
					parsedInput = ParseStringToObject (objectString);
				}
				State state = TickLogic (commandString, parsedInput);
				if (state != State.Running && state != State.Idle) {	
					commands.RemoveAt (0);							
				}
			} else {
				curIntent = null;
			}
		}
		public virtual void EndCurrentTasks ()
		{
			foreach (Intent intent in logic.Values) {
				intent.Reset ();
			}
			ClearAllProcesses ();
			curIntent = null;

		}
		public virtual void ClearAllProcesses ()
		{
			stimulus = new List<string> ();
			commands = new List<string> ();
			cachedstimulus = new List<string> ();
			cachedcommands = new List<string> ();
		}
		public virtual void Analyze ()
		{
			TryToWork ();
			if (NothingToDo ()) {
				if (idleTime > idleTimeMax) {
					AddCommand ("Idle");
					idleTime = 0;
				} else {
					idleTime++;
				}
			} else {
				if (stimulus.Count > 0) {
					idleTime = 0;
					AddCommand (stimulus [0]);
					stimulus.RemoveAt (0);
				}
			}
		}
		public virtual void TryToWork ()
		{
			actor.worker.Tick ();
		}
		public virtual bool NothingToDo ()
		{
			if (stimulus.Count == 0 && commands.Count == 0 && actor.worker.currentJob == null) {
				return true;
			}
			return false;
		}
	}
}