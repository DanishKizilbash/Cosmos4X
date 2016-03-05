using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public abstract class Panel
	{
		public Canvas BaseCanvas = GameObject.Find ("UICanvas").GetComponent<Canvas> ();
		public Panel ()
		{
			GetChildren ();
		}
		public void GetChildren ()
		{
			AddBaseUIComponents ();
			AddPanelUIComponents ();
		}
		private void AddBaseUIComponents ()
		{

		}
		public abstract void AddPanelUIComponents ();
	}
}
