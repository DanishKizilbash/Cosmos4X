using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class SkillsTemplate
	{
		public static Dictionary <string,Skill> skillsTemplate = new Dictionary <string,Skill> ();
		public static void AddSkill (string Name, int Value, string Category)
		{
			skillsTemplate.Add (Name, new Skill (Name, Value, Category));
		}
		public static Dictionary<string, Skill>  GetRandomizedTemplate (string priority="")
		{
			Dictionary<string, Skill> newTemplate = new Dictionary<string, Skill> (skillsTemplate);
			//Randomize
			return newTemplate;
		}
	}
}