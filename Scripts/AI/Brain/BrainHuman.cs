﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Eniso.AI
{
	public class BrainHuman : Brain
	{
		public override void InitiateLogic ()
		{
			/*
			logic.Add ("UserMoveTo", (Intent)(new IntentMoveTo ().Initiate (this, this)));
			*/
			base.InitiateLogic ();
		}
		public override void ProcessStimulus ()
		{
			base.ProcessStimulus ();
		}
		public override void IssueCommands ()
		{
			base.IssueCommands ();
		}
		public override void Analyze ()
		{
			base.Analyze ();
		}
	}
}