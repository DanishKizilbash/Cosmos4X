using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class FleetPanel:Panel
	{
		public override void AddUIElements ()
		{
			base.AddUIElements ();
			AddUIElement ("name", new TextBoxElement ().Init ("name", panelElement, "Name"));
			AddUIElement ("flagship", new TextBoxElement ().Init ("flagship", panelElement, "Flagship"));
			AddUIElement ("numships", new TextBoxElement ().Init ("numships", panelElement, "Num. of Ships"));
			AddUIElement ("homecolony", new TextBoxElement ().Init ("homecolony", panelElement, "Home Colony"));
			AddUIElement ("selectfleet", new ButtonElement ().Init ("selectfleet", panelElement, "Select Fleet", SelectFleet));

		}
		public override void UpdateValues ()
		{
			base.UpdateValues ();
			Fleet fleet = (Fleet)currentExposable;
			UpdateElement ("name", fleet.name);
			UpdateElement ("flagship", fleet.flagShip);
			UpdateElement ("numships", fleet.ships.Count);
			UpdateElement ("homecolony", fleet.homeColony);
			UpdateElement ("selectfleet");
		}
		private void SelectFleet ()
		{
			Fleet fleet = (Fleet)currentExposable;
			fleet.isSelected = true;
		}
	}
}