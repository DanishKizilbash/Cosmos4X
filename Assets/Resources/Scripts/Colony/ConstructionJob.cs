using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public class ConstructionJob
	{
		public Thing thing;
		public Colony colony;
		private float _constructionPoints;
		public float constructionPoints {
			get {
				return _constructionPoints;
			}
			set {
				_constructionPoints = value;
				CheckProgress ();
			}
		}
		public ConstructionJob (Colony tColony, Thing tThing)
		{
			thing = tThing;
			colony = tColony;
			constructionPoints = thing.constructionPointsRequired;
		}
		private void CheckProgress ()
		{
			if (constructionPoints <= 0) {
				thing.isConstructed = true;
				thing.isVisible = true;
				Destroy ();
			}
		}
		public void Destroy ()
		{
			thing = null;
			colony.RemoveConstructionJob (this);
			colony = null;
		}
		public override string ToString ()
		{
			string str = thing.Name + " - " + constructionPoints;
			return str;
		}
	}
}
