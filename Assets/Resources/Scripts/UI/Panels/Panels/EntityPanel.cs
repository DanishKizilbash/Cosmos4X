using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Cosmos
{
	public class EntityPanel:Panel
	{
		public override void AddUIElements ()
		{
			base.AddUIElements ();
			headerText = (TextBoxElement)new TextBoxElement ().Init ("name", panelElement, "Name");
			AddUIElement ("header", headerText);
			AddUIElement ("position", new TextBoxElement ().Init ("pos", panelElement, "Position"));
			AddUIElement ("numentities", new TextBoxElement ().Init ("numentities", panelElement, "Num. Selected Entities"), true);
		}
		public override void UpdateValues ()
		{			
			base.UpdateValues ();
			if (currentExposable != null) {
				Entity currentEntity = null;
				try {
					currentEntity = (Entity)currentExposable;
				} catch (UnityException) {

				}
				if (currentEntity != null) {
					panelElement.UpdateElement ();
					UpdateElement ("header", currentEntity.Name);
					Vector2 vec2dPos = new Vector2 (currentEntity.Position.x, currentEntity.Position.y);
					UpdateElement ("position", vec2dPos);
					float numEntities = 0;
					if (Finder.selectedEntities.Count > 1) {
						numEntities = Finder.selectedEntities.Count;
					}
					UpdateElement ("numentities", numEntities);
				}
			}
		}


	}
}
