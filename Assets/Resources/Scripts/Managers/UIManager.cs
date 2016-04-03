using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class UIManager
	{
		public static Canvas UICanvas = GameObject.Find ("UICanvas").GetComponent<Canvas> ();
		public static Dictionary<string,Panel> UIPanels;
		public static bool hasMouseFocus;
		private static Text txtFps;
		public static void Init ()
		{
			CreatePanelTypes ();
			SetPanelPositions ();
			GameObject txtFpsGo = GameObject.Find ("txtFPS");
			if (txtFpsGo != null) {
				txtFps = txtFpsGo.GetComponent<Text> ();
			}
		}
		public static void Update ()
		{
			ClearPanelExposables ();
			ExposeSelectedExposables ();
			if (txtFps != null) {
				txtFps.text = "FPS : " + TickManager.trueFPS.ToString () + " | " + TickManager.tickSpeedMultiplier + "X";
			}
			//Update Panels
			if (UIPanels != null) {
				foreach (Panel panel in UIPanels.Values) {
					if (panel.isActive) {
						panel.Update ();
					}
				}
			}
			//Check if Mouse is Focused
			hasMouseFocus = false;
			foreach (Panel panel in UIPanels.Values) {
				if (panel.isActive) {
					if (MathI.PointInRectangle (Input.mousePosition, panel.panelElement.rect)) {
						hasMouseFocus = true;
						panel.UpdateMouseStatus ();

					}
				}
			}
		}
		public static Panel GetPanel (string name)
		{
			Panel panel = null;
			UIPanels.TryGetValue (name, out panel);
			return panel;
		}
		private static void CreatePanelTypes ()
		{
			UIPanels = new Dictionary<string,Panel> ();
			UIPanels.Add ("Entity", new EntityPanel ().Init ("Entity"));
			UIPanels.Add ("CelestialBody", new CelestialBodyPanel ().Init ("CelestialBody", GetPanel ("Entity")));
			UIPanels.Add ("Colony", new ColonyPanel ().Init ("Colony", GetPanel ("CelestialBody"), true));
			UIPanels.Add ("ActorShip", new ActorShipPanel ().Init ("ActorShip", GetPanel ("Entity")));
			UIPanels.Add ("Fleet", new FleetPanel ().Init ("Fleet"));
			UIPanels.Add ("Stockpile", new StockpilePanel ().Init ("Stockpile"));
		}
		private static void SetPanelPositions ()
		{
			float width = Camera.main.pixelWidth;
			float height = Camera.main.pixelHeight;
			Vector3 BL = new Vector3 (0, 0, 0);
			Vector3 BR = new Vector3 (width, 0, 0);
			Vector3 TL = new Vector3 (0, height, 0);
			Vector3 TR = new Vector3 (width, height, 0);
			//
			GetPanel ("Entity").MoveTo (BL);
			GetPanel ("Fleet").MoveTo (BR);
		}
		private static void ClearPanelExposables ()
		{
			foreach (Panel panel in UIPanels.Values) {
				panel.currentExposable = null;
				panel.SetVisibility (false);
			}
		}
		private static void ExposeSelectedExposables ()
		{
			if (Finder.selectedFleet != null) {
				SetPanelExposable ("Fleet", Finder.selectedFleet);
			}
			foreach (Entity entity in Finder.selectedEntities) {
				if (!entity.isDestroyed) {
					System.Type curType = entity.GetType ();
					bool success = false;
					while (curType!=typeof(object)) {
						string type = curType.Name.ToString ();
						if (SetPanelExposable (type, entity)) {
							curType = typeof(object);
							success = true;
						} else {
							curType = curType.BaseType;
						}
					}
					if (success) {
						break;
					}
				}
			}
		}
		public static bool SetPanelExposable (string TargetPanel, Exposable exposable)
		{
			Panel panel = null;
			UIPanels.TryGetValue (TargetPanel, out panel);
			if (panel != null) {
				if (panel.currentExposable == null) {
					panel.currentExposable = exposable;
					panel.isActive = true;
				}
				return true;
			} else {
				return false;
			}
		}
	}
}