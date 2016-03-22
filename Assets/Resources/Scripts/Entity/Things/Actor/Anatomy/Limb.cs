﻿using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Limb
	{
		public enum Condition
		{
			Normal,
			Damaged,
			Missing}
		;
		public Condition Status;
		public float Integrity;
		public Limb ()
		{
		
		}
		public Limb Init (Condition condition, float integrity)
		{
			Status = condition;
			Integrity = integrity;
			return this;
		}
	}
}