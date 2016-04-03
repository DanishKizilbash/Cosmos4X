using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class CelestialBodyPanel:Panel
	{
		public override void AddUIElements ()
		{
			base.AddUIElements ();
			AddUIElement ("mass", new TextBoxElement ().Init ("mass", panelElement, "Mass"));
		}
		public override void UpdateValues ()
		{
			base.UpdateValues ();
			CelestialBody cb = (CelestialBody)currentExposable;
			UpdateElement ("mass", cb.orbit.Mass.ToString ());
		}
	}
}
