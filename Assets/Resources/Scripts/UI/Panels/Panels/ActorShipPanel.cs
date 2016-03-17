using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class ActorShipPanel:Panel
	{
		public override void AddPanelUIComponents ()
		{
			AddUIElement ("homecolony", new TextBoxElement ().Init ("homecolony", BasePanel));
			AddUIElement ("mass", new TextBoxElement ().Init ("mass", BasePanel));
			AddUIElement ("velocity", new TextBoxElement ().Init ("velocity", BasePanel));
		}
		public override void UpdateValues ()
		{
			base.UpdateValues ();
			ActorShip ship = (ActorShip)currentEntity;
			string homeColony = "None";
			if (ship.homeColony != null) {
				homeColony = ship.homeColony.name;
			}
			UpdateElement ("homecolony", "Home Colony : " + homeColony);
			UpdateElement ("mass", "Mass : " + ship.physicsObject.mass.ToString ());
			UpdateElement ("velocity", "Velocity : " + ship.physicsObject.velocity.magnitude.ToString ());
		}
	}
}