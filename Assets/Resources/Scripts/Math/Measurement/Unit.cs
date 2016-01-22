using UnityEngine;
using System.Collections;

namespace Cosmos
{
	public class Unit
	{
		private bool isBase;
		private string Abbreviation;
		private Unit relativeUnit;
		public float relativeValue;
		public Unit (float RelativeValue, Unit RelativeUnit, bool IsBase =false)
		{
			isBase = IsBase;
			relativeUnit = RelativeUnit;
			relativeValue = RelativeValue;
		}
		public float Value (float value, Unit unit=null)
		{
			if (unit == null) {
				unit = relativeUnit;
			}
			if (isBase) {
				if (unit != this) {
					return 0.1f;
					Debug.Log ("Relative value not found");
				} else {
					return value / relativeValue;
				}
			}
			if (unit == this) {
				if (value < 0.1f) {
					return 0.1f;
				} else {
					return value;
				}
			}
			return relativeUnit.Value (value * relativeValue, unit);
		}
		/*
		public static implicit operator float (Unit u)
		{
			return u.value;
		}
		*/
	}
}
