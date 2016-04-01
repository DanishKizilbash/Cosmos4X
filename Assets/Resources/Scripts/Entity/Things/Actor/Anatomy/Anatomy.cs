using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class Anatomy
	{
		public bool Invincible = false;
		//
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
		public float GLimit = 10f;
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
		public Dictionary<string,List<Limb>> Limbs;

		public float Life {
			get {
				return GetLife ();
			}
		}

		public void Initiate (Actor parent, float maxLife)
		{
			Parent = parent;
			MaxLife = maxLife;
			Limbs = new Dictionary<string, List<Limb>> ();
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
			foreach (List<Limb> limbs in Limbs.Values) {
				foreach (Limb limb in limbs) {
					life += MaxLife * (limb.Integrity / MaxLimbLife);
				}
			}
			if (life <= 0) {
				Parent.QueForUpdate ();
			}
			return life;
		}

		public List<Limb> GetLimbs (string limb)
		{
			List<Limb> value = null;
			Limbs.TryGetValue (limb, out value);
			return value;
		}
		/*public bool ConsumeFood (ResourceFood food, int qty = 1)
		{
			int qtyConsumed = food.Remove (qty);
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
		}*/
		public void Damage (float damage, string limb="")
		{
			if (limb != "") {
				DamageLimb (damage, limb);
			} else {
				List<Limb> allLimbs = new List<Limb> ();
				foreach (List<Limb> limbs in Limbs.Values) {
					foreach (Limb tLimb in limbs) {
						allLimbs.Add (tLimb);
					}
				}

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
				List<Limb> limbs = Limbs [limb];
				Limb tLimb = limbs [(int)Random.Range (0, limbs.Count)];
				if (tLimb .Integrity > 0) {
					tLimb .Integrity -= damage;
				} else {
					tLimb .Integrity = 0;
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
		public float GetConstructionCost ()
		{
			float cost = 0;
			foreach (List<Limb> limbs in Limbs.Values) {
				foreach (Limb tLimb in limbs) {
					cost += tLimb.ConstructionCost;
				}
			}
			return cost;
		}

		public Limb AddLimb (string Category, string defID="", Limb inputLimb=null)
		{
			Limb newLimb;
			if (inputLimb == null) {
				newLimb = new Limb ().Init (defID);
			} else {
				newLimb = inputLimb;
			}

			if (!Limbs.ContainsKey (Category)) {
				Limbs.Add (Category, new List<Limb> ());
			}
			Limbs [Category].Add (newLimb);
			MaxLimbLife += newLimb.Integrity;
			return newLimb;
		}
		public override string ToString ()
		{
			return GetStatus ();
		}
	}
}