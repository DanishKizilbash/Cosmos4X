using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace Cosmos
{
	public static class TickManager
	{
		public static float baseTPS = 60f;
		public static float trueFPS;
		private static float targetTickTime;
		private static float fpsTimeTracker;
		public static float tickSpeedMultiplier = 1f;
		public static bool isPaused;
		public static float processTime = 1f;
		private static float previousTime;
		private static float currentTime = Time.time;
		private static float tickTime = 0f;
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
		private static List<Action> persistantList = new List<Action> ();

		public static void Init ()
		{
			for (int i = 0; i<=60; i++) {
				TickerList.Add (i, new List<Tickable> ());
			}
		}
		public static void AddPersistant (Action action)
		{
			persistantList.Add (action);
		}
		public static void Update ()
		{
			if (!isPaused) {
				float timeDiff = Time.time - previousTime;
				currentTime += timeDiff;
				tickTime += timeDiff;
				fpsTimeTracker += timeDiff;
				//
				targetTickTime = (1000 / baseTPS / 1000) / tickSpeedMultiplier;	
				int tickPerFrameRequired = Mathf.FloorToInt (tickTime / targetTickTime);
				for (int t = 0; t<tickPerFrameRequired-2; t++) {
					if (tickTime > targetTickTime) {
						Tick ();
						tickTime -= targetTickTime;
					}
				}
			} 
			previousTime = Time.time;
		}
		private static void Tick ()
		{
			curTick++;
			MergeTickerLists ();
			CheckTickLists ();
			PerformTickables ();
			FinalizeRemoval ();
			PerformPersistantUpdates ();
			if (curTick >= maxTick) {
				curTick = 0;
				if (fpsTimeTracker == 0) {
					trueFPS = 1;
				} else {
					trueFPS = Mathf.Round (maxTick / fpsTimeTracker / tickSpeedMultiplier);
				}
				fpsTimeTracker = 0;
			}			
		}
		private static void PerformPersistantUpdates ()
		{
			foreach (Action action in persistantList) {
				action.Invoke ();
			}
		}
		private static void CheckTickLists ()
		{
			foreach (int i in TickerList.Keys) {
				if (MathI.isAMultiple (curTick, i)) {
					List<Tickable> currentList = TickerList [i];
					if (currentList.Count > 0) {
						foreach (Tickable tickable in currentList) {
							tickQue.Add (tickable);
							if (tickable.Interval != i) {
								RemoveTicker (tickable);
								AddTicker (tickable);
							}
						}
					}
				}
			}
		}
		private static void PerformTickables ()
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
				GameManager.curGameState = GameManager.GameState.GameRunning;
			} else {
				isPaused = true;
				GameManager.curGameState = GameManager.GameState.GamePaused;
			}
		}

		public static float GetTime ()
		{
			return GameTime;
		}
	}
}