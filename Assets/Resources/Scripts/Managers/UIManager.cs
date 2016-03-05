using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class UIManager
	{
		public static Dictionary<string,List<object>> UIPanels;
		public static void Init ()
		{
			CreatePanelTypes ();
		}
		public static void Update ()
		{

		}
		public static void GetPanels ()
		{

		}
		private static void CreatePanelTypes ()
		{
			Dictionary<string,List<object>> UIPanels = new Dictionary<string,List<object>> ();
			UIPanels.Add ("Entity");
			UIPanels.Add ("CelestialBody");
			UIPanels.Add ("ActorShip");
		}
	}
}