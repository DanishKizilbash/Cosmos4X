using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class Anatomy
	{
		public Actor Parent;
		public float MaxLife;
		public float MaxLimbLife;
		//
		public float Hunger;
		public float Thirst;
		public float MaxHunger;
		public float MaxThirst;
		//
		public float HungerDamage = 0.25f;
		public float ThirstDamage = 0.5f;
		//
		public enum HungerState
		{
			Satisfied,
			Peckish,
			Hungry,
			Starving,
			Dying
		}

		public enum ThirstState
		{
			Satisfied,
			Thirsty,
			Dehydrated,
			Dessicated,
			Dying
		}

		public HungerState curHungerState {
			get {
				if (Hunger <= 0) {
					return HungerState.Dying;
				}
				if (Hunger <= MaxHunger * 0.2f) {
					return HungerState.Starving;
				}
				if (Hunger <= MaxHunger * 0.45f) {
					return HungerState.Hungry;
				}
				if (Hunger <= MaxHunger * 0.6f) {
					return HungerState.Peckish;
				}
				return HungerState.Satisfied;				
			}
		}
		public ThirstState curThirstState {
			get {
				if (Thirst <= 0) {
					return ThirstState.Dying;
				}
				if (Thirst <= MaxThirst * 0.2f) {
					return ThirstState.Dessicated;
				}
				if (Thirst <= MaxThirst * 0.45f) {
					return ThirstState.Dehydrated;
				}
				if (Thirst <= MaxThirst * 0.6f) {
					return ThirstState.Thirsty;
				}
				return ThirstState.Satisfied;				
								
			}
		}
		//		
		private float timeSinceLastTick = 0;
		private float lastTime = 0;
		public Dictionary<string,Limb> Limbs;

		public float Life {
			get {
				return GetLife ();
			}
		}

		public void Initiate (Actor parent, float maxLife)
		{
			Parent = parent;
			MaxLife = maxLife;
			Limbs = new Dictionary<string, Limb> ();
		}

		public float GetLife ()
		{
			float currentTime = TickManager.GetTime ();
			timeSinceLastTick = currentTime - lastTime;
			lastTime = currentTime;
			float life = 0;
			//Hunger
			Hunger -= timeSinceLastTick;
			if (curHungerState != HungerState.Satisfied) {
				Parent.brain.AddStimulus ("Hunger");
			}
			if (Hunger <= 0) {
				Hunger = 0;
				Damage (HungerDamage * timeSinceLastTick);
			}
			//Thirst
			Thirst -= timeSinceLastTick;
			if (Thirst <= 0) {
				Thirst = 0;
				Damage (ThirstDamage * timeSinceLastTick);
			}
			//Limb Status
			foreach (Limb limb in Limbs.Values) {
				life += MaxLife * (limb.Integrity / MaxLimbLife);
			}
			if (life <= 0) {
				Parent.QueForUpdate ();
			}
			return life;
		}

		public Limb GetLimb (string limb)
		{
			Limb value = null;
			Limbs.TryGetValue (limb, out value);
			return value;
		}
		public bool ConsumeFood (ResourceFood food, int qty = 1)
		{
			int qtyConsumed = food.Consume (qty);
			Hunger += food.hunger * qtyConsumed;
			Thirst += food.thirst * qtyConsumed;
			if (Thirst > MaxThirst) {
				Thirst = MaxThirst;
			}
			if (Hunger > MaxHunger) {
				Hunger = MaxHunger;
				return true;
			}

			return false;
		}
		public void Damage (float damage, string limb="")
		{
			if (limb != "") {
				DamageLimb (damage, limb);
			} else {
				List<Limb> allLimbs = new List<Limb> ();
				allLimbs.AddRange (Limbs.Values);
				List<Limb> intactLimbs = allLimbs.FindAll (
					delegate(Limb ilimb) {
					return ilimb.Integrity > 0;
				});
				if (intactLimbs.Count > 0) {
					Limb randomLimb = intactLimbs [Random.Range (0, intactLimbs.Count)];
					if (randomLimb.Integrity > 0) {
						randomLimb.Integrity -= damage;
					} else {
						randomLimb.Integrity = 0;
						Damage (damage);
					}
				}
			}
		}

		public void DamageLimb (float damage, string limb="")
		{
			if (Limbs.ContainsKey (limb)) {
				if (Limbs [limb].Integrity > 0) {
					Limbs [limb].Integrity -= damage;
				} else {
					Limbs [limb].Integrity = 0;
				}
			}
			/*else{
				Damage (damage)
			}*/
		}

		public string GetStatus ()
		{
			string str = "";
			str += "Life : " + (int)Life + System.Environment.NewLine;
			str += "Hunger : " + curHungerState + System.Environment.NewLine;
			str += "Thirst : " + curThirstState + System.Environment.NewLine;
			return str;
		}

		public void AddLimb (string name, float integrity, Limb.Condition condition = Limb.Condition.Normal)
		{
			if (!Limbs.ContainsKey (name)) {
				Limbs.Add (name, new Limb (condition, integrity));
				MaxLimbLife += integrity;
			}
		}

		public override string ToString ()
		{
			return GetStatus ();
		}
	}
}