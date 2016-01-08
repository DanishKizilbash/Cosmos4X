using UnityEngine;
using System.Collections;

namespace Eniso {
	public static class Parser {
		public static Vector3 StringToVector3 (string str) {
			if (!str.StartsWith ("(")) {
				return new Vector3 (float.MaxValue, float.MaxValue, float.MaxValue);
			}
			float x;
			float y;
			float z;

			//(0.0, 0.0, 0.0)
			string[] seperators = new string[] {",","(",")"};
			string[] strings = str.Split (seperators, System.StringSplitOptions.None);
			float.TryParse (strings [1], out x);
			float.TryParse (strings [2], out y);
			float.TryParse (strings [3], out z);
			//
			return new Vector3 (x, y, z);
		}
		public static int StringToInt (string str) {
			int val = 0;
			if (int.TryParse (str, out val)) {
				return val;
			}
			return 0;
		}
		public static float StringToFloat (string str) {
			float val = 0;
			if (float.TryParse (str, out val)) {
				return val;
			}
			return 0;
		}
	}
}