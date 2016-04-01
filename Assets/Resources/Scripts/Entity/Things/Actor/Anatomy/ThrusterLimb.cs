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
		public override void SetAttributes ()
		{
			base.SetAttributes ();
			isRCS = Parser.StringToBool (def.GetAttribute ("IsRCS"));
		}
		public override string DefaultID ()
		{
			return "Limb_Ship_Thruster_BasicThruster";
		}
	}
}