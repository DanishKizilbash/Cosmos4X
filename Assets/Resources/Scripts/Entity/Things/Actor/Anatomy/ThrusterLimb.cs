using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class ThrusterLimb:Limb
	{
		public Vector3 force;
		public Vector3 offset;
		public float throttle;
		public bool isRCS;
	}
}