using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class StockpilePanel:Panel
	{
		public override void AddUIElements ()
		{
			base.AddUIElements ();
			AddUIElement ("parentcolony", new TextBoxElement ().Init ("parentcolony", panelElement, "Parent Colony"));
		}
		public override void UpdateValues ()
		{			
			base.UpdateValues ();
			if (currentExposable != null) {
				Stockpile stockpile = null;
				try {
					stockpile = (Stockpile)currentExposable;
				} catch (UnityException) {

				}
				if (stockpile != null) {
					UpdateElement ("parentcolony", "");
				}
			}
		}


	}
}
