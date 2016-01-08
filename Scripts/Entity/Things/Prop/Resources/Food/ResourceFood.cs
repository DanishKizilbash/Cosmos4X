using UnityEngine;
using System.Collections;
namespace Eniso{
	public class ResourceFood :PropResource  {
		public float timeLastAccessed = TickManager.GetTime();
		public float hunger;
		public float thirst;
		private float cachedDecay;
		public float decay{
			get{	
				float newTime = TickManager.GetTime();
				cachedDecay -= newTime-timeLastAccessed;
				timeLastAccessed = newTime;
				if (cachedDecay<=0){
					cachedDecay=0;
				}
				return cachedDecay;
			}
			set{
				cachedDecay = value;
			}
		}

		public override void SetAttributes () {
			base.SetAttributes();
			hunger = Parser.StringToFloat(def.GetAttribute("Hunger"));
			thirst = Parser.StringToFloat(def.GetAttribute("Thirst"));
			decay = Parser.StringToFloat(def.GetAttribute("Decay"));
		}
	}
}