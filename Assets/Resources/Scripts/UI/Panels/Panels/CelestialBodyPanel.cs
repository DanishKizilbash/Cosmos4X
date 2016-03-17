using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class CelestialBodyPanel:Panel
	{
		Vector2 ColonyPanelSize = new Vector2 (50, 50);
		List<UIElement> ColonyElements;
		public override void AddPanelUIComponents ()
		{
			AddUIElement ("mass", new TextBoxElement ().Init ("mass", BasePanel));
			AddColonyUIComponents ();
		}
		private void AddColonyUIComponents ()
		{
			ColonyElements = new List<UIElement> ();
			ColonyElements.Add (AddUIElement ("colonyheader", new TextBoxElement ().Init ("colonyheader", BasePanel)));
			ColonyElements.Add (AddUIElement ("population", new TextBoxElement ().Init ("population", BasePanel)));
			ColonyElements.Add (AddUIElement ("constructionyards", new TextBoxElement ().Init ("constructionyards", BasePanel)));
			ColonyElements.Add (AddUIElement ("shipyards", new TextBoxElement ().Init ("shipyards", BasePanel)));
			/*
			  public Stockpile stockpile;
		public float population; //in millions
		public float constructionYards;
		public float shipYards;
			 */
		}
		public override void UpdateValues ()
		{
			base.UpdateValues ();
			CelestialBody cb = (CelestialBody)currentEntity;
			UpdateElement ("mass", cb.orbit.Mass.ToString ());
			//
			if (cb.colony != null) {
				ColonyValues (cb);
				BasePanel.Resize (ColonyPanelSize);
			} else {
				foreach (UIElement element in ColonyElements) {
					element.Hide ();
				}
				BasePanel.Resize (basePanelSize);
			}
		}
		private void ColonyValues (CelestialBody cb)
		{
			UpdateElement ("colonyheader", "Colony : " + cb.colony.name);
			UpdateElement ("population", cb.colony.population.ToString ());
			UpdateElement ("constructionyards", cb.colony.constructionYards.ToString ());
			UpdateElement ("shipyards", cb.colony.shipYards.ToString ());
		}
	}
}
