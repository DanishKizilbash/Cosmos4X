using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public class MinMax
	{
		public float min;
		public float max;
		public MinMax (float Min, float Max)
		{
			min = Min;
			max = Max;
		}
		public bool InRange (float value)
		{
			return (min <= value && value <= max);
		}
		public float Rand ()
		{
			return (Random.value * (max - min)) + min;
		}
	}
}
