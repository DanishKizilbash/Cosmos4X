using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class Profiler
	{
		private static float startTime;
		private static float subTime;
		private static float elapsedLaps;
		private static float elapsedLapTime;
		private static List<string> debugStrings = new List<string> ();
		public static void Start ()
		{
			Reset ();
			ResetSub ();
			AddDebugString ("____________ Starting Profile ____________");
		}	
		public static void Report (string description = "")
		{		
			AddDebugString (description + ((Time.realtimeSinceStartup - startTime) * 1000));
		}
		public static void ReportAndReset (string description = "")
		{			
			Report (description);
			Reset ();
		}
		public static void ReportSub (string description = "")
		{			
			AddDebugString (description + ((Time.realtimeSinceStartup - subTime) * 1000));
			ResetSub ();
		}
		public static void ReportLaps (string description = "")
		{
			AddDebugString (description + " Total Laps : " + elapsedLaps + " Total Time : " + elapsedLapTime + " Avg Lap Time : " + (elapsedLapTime / elapsedLaps));
			ResetLaps ();
		}
		public static void ResetSub ()
		{
			subTime = Time.realtimeSinceStartup;
		}
		public static void ResetLaps ()
		{
			elapsedLaps = 0;
			elapsedLapTime = 0;
		}
		public static void Reset ()
		{
			startTime = Time.realtimeSinceStartup;
			ResetLaps ();
		}

		public static void AddLap ()
		{
			elapsedLaps++;
			elapsedLapTime += Time.realtimeSinceStartup - startTime;
		}
		public static void AddDebugString (string str)
		{
			debugStrings.Add (str);
		}
		public static void ExposeStrings ()
		{
			foreach (string str in debugStrings) {
				Debug.Log (str);
			}
			debugStrings = new List<string> ();
		}
		public static void Clear ()
		{
			debugStrings = new List<string> ();
			startTime = Time.realtimeSinceStartup;
			subTime = Time.realtimeSinceStartup;
		}
	}
}