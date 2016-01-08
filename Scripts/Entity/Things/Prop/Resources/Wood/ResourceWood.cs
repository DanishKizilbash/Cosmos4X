using UnityEngine;
using System.Collections;
namespace Eniso {
	public class ResourceWood:PropResource {
		public float timeLastAccessed = TickManager.GetTime ();
		private float cachedDecay;
		public float decay {
			get {	
				float newTime = TickManager.GetTime ();
				cachedDecay -= newTime - timeLastAccessed;
				timeLastAccessed = newTime;
				if (cachedDecay <= 0) {
					cachedDecay = 0;
				}
				return cachedDecay;
			}
			set {
				cachedDecay = value;
			}
		}
		
		public override void SetAttributes () {
			base.SetAttributes ();
			decay = Parser.StringToFloat (def.GetAttribute ("Decay"));
		}
	}
}