using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Cosmos
{
	public abstract class Exposable
	{
		public bool isDestroyed = false;
		public string name;

		public virtual void Destroy ()
		{
			isDestroyed = true;
		}
		public abstract void OnSelected (bool value);
	}
}