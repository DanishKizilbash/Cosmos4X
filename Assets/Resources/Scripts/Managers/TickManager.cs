using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace Cosmos
{
	public static class TickManager
	{
		public static bool isPaused;
		public static float processTime = 1f;
		private static float previousTime;
		private static float currentTime = Time.time;
		private static float cachedGameTime;

		public static float GameTime {
			get {
				cachedGameTime = currentTime;
				return cachedGameTime;
			}
			set {
				cachedGameTime = value;
			}
		}

		public static int maxTick = 60;
		public static int curTick = 0;
		private static Dictionary<int,List<Tickable>> TickerList = new Dictionary<int,List<Tickable>> ();
		private static List<Tickable> cachedTickerList = new List<Tickable> ();
		private static List<Tickable> tickQue = new List<Tickable> ();

		public static void Init ()
		{
			for (int i = 0; i<=60; i++) {
				TickerList.Add (i, new List<Tickable> ());
			}
		}

		public static void Update ()
		{
			if (!isPaused) {
				currentTime += Time.time - previousTime;
				previousTime = Time.time;
				curTick++;
				MergeTickerLists ();
				CheckTickLists ();
				PerformTicks ();
				FinalizeRemoval ();
				if (curTick >= maxTick) {
					curTick = 0;
				}			
			} else {				
				previousTime = Time.time;
			}
		}
		private static void CheckTickLists ()
		{
			foreach (int i in TickerList.Keys) {
				if (MathI.isAMultiple (curTick, i)) {
					if (TickerList.ContainsKey (i)) {
						List<Tickable> currentList = TickerList [i];
						if (currentList.Count > 0) {
							foreach (Tickable tickable in currentList) {
								tickQue.Add (tickable);
							}
						}
					}
				}
			}
		}
		private static void PerformTicks ()
		{
			List<Tickable> cachedList = new List<Tickable> ();
			float timeElapsed = 0;
			for (int i = 0; i<tickQue.Count; i++) {
				float startTime = Time.realtimeSinceStartup;
				Tickable tickable = tickQue [i];
				if (timeElapsed > processTime) {	//Check for time and set as remaining
					cachedList.AddRange (tickQue.GetRange (i, tickQue.Count - i));
					break;
				} else {
					tickable.TickRequired = false;
					tickable.Tick ();
					if (!tickable.TickRequired) {
						RemoveTicker (tickable);
					}
				}
				timeElapsed += (Time.realtimeSinceStartup - startTime) * 1000;
			}
			tickQue = cachedList;

		}
		public static void MergeTickerLists ()
		{
			int cts = 0;
			foreach (Tickable tickable in cachedTickerList) {
				cts++;
				Dictionary<int,List<Tickable>> tempList;
				tempList = TickerList;
				int Interval = tickable.Interval;
				tickable.AddedToTickManager = true;
				tempList [Interval].Add (tickable);
			}
			cachedTickerList.Clear ();
		}

		public static void AddTicker (Tickable tickable)
		{
			if (!tickable.AddedToTickManager) {
				tickable.AddedToTickManager = true;
				cachedTickerList.Add (tickable);
			}
		}

		public static void RemoveTicker (Tickable tickable)
		{
			tickable.TickRequired = false;
			tickable.AddedToTickManager = false;
		}
		private static bool quedForRemoval (Tickable t)
		{
			return !t.AddedToTickManager;

		}
		private static void FinalizeRemoval ()
		{
			float elapsedTime = 0;
			foreach (List<Tickable> tickables in TickerList.Values) {
				float startTime = Time.realtimeSinceStartup;
				if (elapsedTime < processTime) {
					tickables.RemoveAll (quedForRemoval);
				} else {
					break;
				}
				elapsedTime += (Time.realtimeSinceStartup - startTime) * 1000;
			}
		}

		public static void SwitchPauseState ()
		{
			if (isPaused) {
				isPaused = false;
			} else {
				isPaused = true;
			}
		}

		public static float GetTime ()
		{
			return GameTime;
		}
	}
}