using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class ActorShipPanel:Panel
	{
		public override void AddUIElements ()
		{
			base.AddUIElements ();
			AddUIElement ("homecolony", new TextBoxElement ().Init ("homecolony", panelElement, "Home Colony"));
			AddUIElement ("fleet", new TextBoxElement ().Init ("fleet", panelElement, "Fleet", SelectFleet), true);
			AddUIElement ("mass", new TextBoxElement ().Init ("mass", panelElement, "Mass"));
			AddUIElement ("velocity", new TextBoxElement ().Init ("velocity", panelElement, "Velocity"));

		}
		public override void UpdateValues ()
		{
			base.UpdateValues ();
			ActorShip ship = (ActorShip)currentExposable;
			UpdateElement ("homecolony", ship.homeColony);
			UpdateElement ("fleet", ship.fleet);
			UpdateElement ("mass", ship.physicsObject.mass);
			UpdateElement ("velocity", ship.physicsObject.velocity.magnitude);
		}
		public void SelectFleet ()
		{
			ActorShip ship = (ActorShip)currentExposable;
			Finder.selectedFleet = ship.fleet;
		}
	}
}