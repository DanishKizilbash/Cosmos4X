using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public static class MathI
	{
		public static Vector3 InfinityVector = new Vector3 (Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
		public static bool PointInRectangle (Vector3 point, Rect rectangle)
		{
			if (point.x < rectangle.xMax && point.x > rectangle.xMin) {
				if (point.y < rectangle.yMax && point.y > rectangle.yMin) {
					return true;
				}
			}
			return false;
		}
		public static bool RectIntersect (Rect rect1, Rect rect2)
		{

			return (rect2.xMax >= rect1.x && rect2.x <= rect1.xMax) && (rect2.yMax >= rect1.y && rect2.y <= rect1.yMax);
		}
		public static bool CircleRectIntersect (float circRadius, Vector3 circCenter, Rect rect)
		{
			// Find the closest point to the circle within the rectangle
			float closestX = Mathf.Clamp (circCenter.x, rect.x, rect.xMax);
			float closestY = Mathf.Clamp (circCenter.y, rect.y, rect.yMax);
			
			// Calculate the distance between the circle's center and this closest point
			float distanceX = circCenter.x - closestX;
			float distanceY = circCenter.y - closestY;
			// If the distance is less than the circle's radius, an intersection occurs
			float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
			return distanceSquared < (circRadius * circRadius);
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
		public static int GenSeed (int size)
		{
			int seed = 0;
			string seedArray = "";
			for (int i=0; i<size; i++) {
				seedArray += Mathf.Ceil (Random.value * 9).ToString ();
			}
			seed = Parser.StringToInt (seedArray);
			return seed;
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

			return new Vector3 (xP, yP, z);
		}
		public static Vector3 RotateVector (Vector3 point, Vector3 rotationPoint, float rotation)
		{
			return RotateVector (point.x, point.y, point.z, rotationPoint, rotation);			
		}
		public static float SphereVolume (float radius)
		{
			return 4 / 3 * Mathf.PI * Mathf.Pow (radius, 3);
		}
		public static float RationalizeFloat (float value, int digits)
		{
			float magnitude = Mathf.Pow (10, digits);
			float result = Mathf.Floor (value * magnitude);
			result /= magnitude;
			return result;
		}
		public static Rect RectFromPoints (Vector3 pt1, Vector3 pt2)
		{
			return RectFromPoints (new Vector2 (pt1.x, pt1.y), new Vector2 (pt2.x, pt2.y));
		}
		public static Rect RectFromPoints (Vector2 pt1, Vector2 pt2)
		{
			float minX = Min (pt1.x, pt2.x);
			float minY = Min (pt1.y, pt2.y);
			float width = Mathf.Abs (pt2.x - pt1.x);
			float height = Mathf.Abs (pt2.y - pt1.y);

			return new Rect (new Vector2 (minX, minY), new Vector2 (width, height));
		}
		public static  bool CirclesIntersect (float radius1, Vector3 center1, float radius2, Vector3 center2)
		{
			//(R0-R1)^2 <= (x0-x1)^2+(y0-y1)^2 <= (R0+R1)^2
			float min = (radius1 - radius2);
			min *= min;
			float max = (radius1 + radius2);
			max *= max;
			float x = center1.x - center2.x;
			x *= x;
			float y = center1.y - center2.y;
			y *= y;
			return (min <= x + y) && (x + y <= max);
		}
		public static float DegToQuat (float rotation)
		{
			rotation = rotation % 360;
			if (rotation >= 270 || rotation < 90) {
				rotation += 90;
			} else if (rotation >= 90 && rotation < 270) {
				rotation -= 270;
			}
			return rotation % 360;
		}
	}

}