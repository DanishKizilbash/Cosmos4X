using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public static class DepthManager
	{
		private static int currentDepth = 0; 
		private static int previousDepth = 0;
		public static int checkDepth = 0;
		public static int CurrentDepth {
			get {
				return currentDepth;
			}
			set {
				currentDepth = value;
			}
		}

		public static void Update ()
		{

			previousDepth = currentDepth;
		}



	}
}