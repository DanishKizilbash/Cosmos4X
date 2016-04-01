using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public static class Parser
	{
		public static Vector3 StringToVector3 (string str)
		{
			if (str == null || !str.StartsWith ("(")) {
				return new Vector3 (Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
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
		public static Vector3 StringToVector2 (string str)
		{
			if (!str.StartsWith ("(")) {
				return new Vector2 (float.MaxValue, float.MaxValue);
			}
			float x;
			float y;
			
			//(0.0, 0.0, 0.0)
			string[] seperators = new string[] {",","(",")"};
			string[] strings = str.Split (seperators, System.StringSplitOptions.None);
			float.TryParse (strings [1], out x);
			float.TryParse (strings [2], out y);
			//
			return new Vector2 (x, y);
		}
		public static int StringToInt (string str)
		{
			int val = 0;
			if (int.TryParse (str, out val)) {
				return val;
			}
			return 0;
		}
		public static float StringToFloat (string str)
		{
			float val = 0;
			if (float.TryParse (str, out val)) {
				return val;
			}
			return 0;
		}
		public static bool StringToBool (string str)
		{
			str = str.ToLower ();
			if (str == "true") {
				return true;
			}
			return false;
		}
	}
}