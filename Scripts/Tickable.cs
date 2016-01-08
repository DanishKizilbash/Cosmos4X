using UnityEngine;
using System.Collections;

public abstract class Tickable {
	public int Interval = 60;
	public bool TickRequired;
	public bool AddedToTickManager = false;
	public abstract void Tick ();
}
