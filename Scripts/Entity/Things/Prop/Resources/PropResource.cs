using UnityEngine;
using System.Collections;
namespace Eniso {
	public abstract class PropResource : Prop {
		public string function;
		public float value;
		public int qtyInStack;
		public int StackSize = 64;
		public override Entity Init (string defID) {
			base.Init (defID);
			Finder.ResourceDatabase.Add (this);
			return this;
		}
		public virtual int AddToStack (int qty) {
			int remainder = 0;
			qtyInStack += qty;
			if (qtyInStack > StackSize) {
				remainder = qtyInStack - StackSize;
				qtyInStack = StackSize;
			}
			return remainder;

		}
		public override void SetAttributes () {
			function = def.GetAttribute ("Function");
			value = Parser.StringToFloat (def.GetAttribute ("Value"));
			int tInt = Parser.StringToInt (def.GetAttribute ("StackSize"));
			if (tInt != 0) {
				StackSize = tInt;
			}
		}
		public override void OnSelected (bool selected) {
			base.OnSelected (selected);
			Exposer.AddNote ("Stack Size: " + qtyInStack);
		}
		public virtual int Consume (int qty) {
			int realQty = 0;
			if (qtyInStack > qty) {
				qtyInStack -= qty;
				realQty = qty;
			} else {
				realQty = qtyInStack;
				qtyInStack = 0;
			}
			if (qtyInStack <= 0) {
				Destroy ();
			}
			isSelected = true;
			return realQty;
		}
		public override void Tick () {
			base.Tick ();
		}
		public override string DefaultID () {
			return "Thing_Prop_Resource_Apple";
		}
	}
}