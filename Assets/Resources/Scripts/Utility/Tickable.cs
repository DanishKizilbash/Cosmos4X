using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public abstract class Tickable:Exposable
	{
		public int Interval = 1;
		public bool TickRequired;
		public bool AddedToTickManager = false;
		public Tickable ()
		{
			TickManager.AddTicker (this);
		}
		public abstract void Tick ();
		public override void Destroy ()
		{
			base.Destroy ();
			TickRequired = false;
			TickManager.RemoveTicker (this);
		}
	}
}
