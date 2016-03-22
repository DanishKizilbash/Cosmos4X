using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Force
	{
		public Vector3 vector;
		public Vector3 offset;
		public Force (Vector3 Vector, Vector3 Offset)
		{
			vector = Vector;
			offset = Offset;
		}
	}
}