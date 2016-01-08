using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cosmos.AI;

namespace Cosmos
{
	public abstract class Actor:Thing
	{
		public Brain brain;
		public Anatomy anatomy;
		//
		public Worker worker;
		//		
		public Dictionary<string,Skill> skills;
		public override string Name {
			get {
				return base.Name;
			}
			set {
				if (base.Name != null) {
					Finder.ActorDatabase.UpdateKey (base.Name, value);
				}
				base.Name = value;
			}
		}

		public override Entity Init (string defID)
		{
			base.Init (defID);
			AddBrain ();
			Interval = brain.Interval;
			AddAnatomy ();
			SetLateAttributes ();
			worker = new Worker (this);
			Finder.ActorDatabase.Add (this);
			return this;
		}
		public abstract void AddBrain ();
		public abstract void AddAnatomy ();
		public override void SetLateAttributes ()
		{
			anatomy.MaxHunger = Parser.StringToFloat (def.GetAttribute ("Hunger"));
			anatomy.MaxThirst = Parser.StringToFloat (def.GetAttribute ("Thirst"));
			anatomy.Hunger = anatomy.MaxHunger;
			anatomy.Thirst = anatomy.MaxThirst;
			skills = SkillsTemplate.GetRandomizedTemplate ();
		}

		public override void Tick ()
		{
			//Movement

			base.Tick ();
			if (anatomy.Life <= 0) {
				Die ();
			}
			TickRequired = true;
		}
		
		public int GetSkillValue (string skillName)
		{
			Skill value = null;
			skills.TryGetValue (skillName, out value);
			if (value != null) {
				return value.level;
			}
			return 0;
		}
		public Skill GetSkill (string skillName)
		{
			Skill value = null;
			skills.TryGetValue (skillName, out value);
			return value;
		}
		public virtual void Die ()
		{
			Debug.Log (ToString () + " died");
			Destroy ();
		}
		public override void Destroy ()
		{
			TickManager.RemoveTicker (this.brain);
			base.Destroy ();
		}
		public override string DefaultID ()
		{
			return "Thing_Actor_Human_Male";
		}
	}
}