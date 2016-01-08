using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class Skill
	{
		public bool enabled;
		public string category;
		public string name;
		public int level;
		public Skill (string Name, int Level, string Category = "General")
		{
			name = Name;
			level = Level;
			category = Category;
			enabled = true;
		} 
		public void Disable ()
		{
			enabled = false;
		}
		public void Enable ()
		{
			enabled = true;
		}
	}
}
