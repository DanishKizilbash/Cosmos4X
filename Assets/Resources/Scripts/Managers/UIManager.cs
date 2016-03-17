using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class UIManager
	{
		public static Canvas UICanvas = GameObject.Find ("UICanvas").GetComponent<Canvas> ();
		public static Dictionary<string,Panel> UIPanels;
		public static void Init ()
		{
			CreatePanelTypes ();
		}
		public static void Update ()
		{
			if (UIPanels != null) {
				foreach (Panel panel in UIPanels.Values) {
					if (panel.active) {
						panel.Update ();
					}
				}
			}
		}
		public static void GetPanels ()
		{

		}
		private static void CreatePanelTypes ()
		{
			UIPanels = new Dictionary<string,Panel> ();
			UIPanels.Add ("Entity", new EntityPanel ());
			UIPanels.Add ("CelestialBody", new CelestialBodyPanel ());
			UIPanels.Add ("ActorShip", new ActorShipPanel ());
		}
		public static bool SetPanelEntity (string TargetPanel, Entity entity, bool active)
		{
			Panel panel = null;
			UIPanels.TryGetValue (TargetPanel, out panel);
			if (panel != null) {
				panel.active = active;
				if (active) {
					panel.currentEntity = entity;
				}
				return true;
			} else {
				return false;
			}
		}
	}
}