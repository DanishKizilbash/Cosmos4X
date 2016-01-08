using UnityEngine;
using System.Collections;
namespace Eniso{
	public class Limb {
		public enum Condition{Normal,Damaged,Missing};
		public Condition Status;
		public float Integrity;
		public Limb(Condition condition, float integrity){
			Status = condition;
			Integrity = integrity;
		}
	}
}