using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public static class MathI
	{

		public static bool PointInRectangle (Vector3 point, Rect rectangle)
		{
			if (point.x < rectangle.xMax && point.x > rectangle.xMin) {
				if (point.y < rectangle.yMax && point.y > rectangle.yMin) {
					return true;
				}
			}
			return false;
		}

		public static int ClampInt (int input, int min, int max)
		{
			int output = input;
			if (output < min) {
				output = min;
			}
			if (output > max) {
				output = max;
			}
			return output;
		}

		public static Rect ClampRect (Rect inputRect, Rect clampRect)
		{
			Rect outputRect = inputRect;
			if (outputRect.x < clampRect.x) {
				outputRect.x = clampRect.x;
			}
			if (outputRect.y < clampRect.y) {
				outputRect.y = clampRect.y;
			}
			if (outputRect.width > clampRect.width) {
				outputRect.width = clampRect.width;
			}
			if (outputRect.height > clampRect.height) {
				outputRect.height = clampRect.height;
			}
			return outputRect;
		}
		public static bool isAMultiple (float value, float multiple)
		{
			return value / multiple == Mathf.Floor (value / multiple);
		}	
		public static float Min (float a, float b)
		{
			return a < b ? a : b;
		}
		public static float Max (float a, float b)
		{
			return a > b ? a : b;
		}
		public static Vector3 RotateVector (float x, float y, float z, Vector3 rotationPoint, float rotation)
		{
			float rot = (rotation % 360) * Mathf.Deg2Rad;
			float sin = Mathf.Sin (rot);
			float cos = Mathf.Cos (rot);
			x -= rotationPoint.x;
			y -= rotationPoint.y;
			float xP = x * cos - y * sin;
			float yP = x * sin + y * cos;
			xP += rotationPoint.x;
			yP += rotationPoint.y;

			return new Vector3 (xP, yP, 0);
		}
		public static Vector3 RotateVector (Vector3 point, Vector3 rotationPoint, float rotation)
		{
			return RotateVector (point.x, point.y, point.z, rotationPoint, rotation);
			
		}
	}
}