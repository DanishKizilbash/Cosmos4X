using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class ColonyPanel:Panel
	{
		public override void AddUIElements ()
		{		
			base.AddUIElements ();
			AddUIElement ("colonyheader", new TextBoxElement ().Init ("colonyheader", panelElement, "Colony"));
			AddUIElement ("population", new TextBoxElement ().Init ("population", panelElement, "Pop."));
			AddUIElement ("shipyards", new TextBoxElement ().Init ("shipyards", panelElement, "Ship Yards"));
			AddUIElement ("shipgarrison", new TextBoxElement ().Init ("shipgarrison", panelElement, "Ship Garrison"));			
			AddUIElement ("constructionyards", new TextBoxElement ().Init ("constructionyards", panelElement, "Cons. Yards"));
			AddUIElement ("constructionque", new TextBoxElement ().Init ("constructionque", panelElement, "Cons. Que"), true);
			AddUIElement ("constructionqueprogress", new TextBoxElement ().Init ("constructionqueprogress", panelElement, "Cons. Progress"), true);
			AddUIElement ("constructship", new ButtonElement ().Init ("constructship", panelElement, "Construct Ship", ConstructNewShip));
			AddUIElement ("constructcyard", new ButtonElement ().Init ("constructcyard", panelElement, "Construct Cons. Yard", ConstructNewConstructionYard));
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
			CelestialBody cb = (CelestialBody)currentExposable;
			;
			if (cb.colony != null) {
				isActive = true;
				UpdateElement ("colonyheader", cb.colony.name);
				UpdateElement ("population", cb.colony.population);
				//
				UpdateElement ("shipyards", cb.colony.shipYards);
				UpdateElement ("shipgarrison", cb.colony.garrisonedShips.Count);
				//
				UpdateElement ("constructionyards", cb.colony.constructionYards);
				UpdateElement ("constructionque", cb.colony.constructionJobs.Count);
				string constructionJobProgressString = "";
				if (cb.colony.constructionJobs.Count > 0) {
					constructionJobProgressString = cb.colony.constructionJobs [0].ToString ();
				}
				UpdateElementString ("constructionqueprogress", constructionJobProgressString);			
				UpdateElement ("constructship");
				UpdateElement ("constructcyard");
			} else {
				isActive = false;
			}
		}
		private void ConstructNewShip ()
		{
			CelestialBody cb = (CelestialBody)currentExposable;
			if (cb.colony != null) {
				cb.colony.BuildShip ();
			}
		}
		private void ConstructNewConstructionYard ()
		{
			CelestialBody cb = (CelestialBody)currentExposable;
			if (cb.colony != null) {
				cb.colony.BuildConstructionYard ();
			}
		}
	}
}
