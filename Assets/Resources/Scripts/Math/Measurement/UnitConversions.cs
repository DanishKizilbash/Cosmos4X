using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public static class UnitConversion
	{
		public static class Distance
		{
			public static readonly Unit  UnitDistance = new Unit (10, null, true);
			public static readonly Unit  Km = new Unit (10, UnitDistance);
			public static readonly Unit  Mm = new Unit (1000, Km);
			public static readonly Unit  Gm = new Unit (1000, Mm);
			public static readonly Unit  Au = new Unit (150, Gm);
			public static readonly Unit  EarthDiameter = new Unit (12.7f, Mm);
			public static readonly Unit  SolDiameter = new Unit (1400f, Mm);
			public static readonly Unit  LightDay = new Unit (173.15f, Au);
			public static readonly Unit  LightYear = new Unit (365, LightDay);
			public static readonly Unit  Sector = new Unit (100, LightYear);
		}
		public static class Mass
		{
			public static readonly Unit  UnitMass = new Unit (1, null, true);
			public static readonly Unit  Kg = new Unit (1, UnitMass);
			public static readonly Unit  Ton = new Unit (1000, Kg);
			public static readonly Unit  MegaTon = new Unit (1000000, Ton);
			public static readonly Unit  TeraTon = new Unit (1000000, MegaTon);
			public static readonly Unit  ExaTon = new Unit (1000000, TeraTon);
			//
			public static readonly Unit  EarthMass = new Unit (5972f, ExaTon);
			public static readonly Unit  JovianMass = new Unit (317.8f, EarthMass);
			public static readonly Unit  SolMass = new Unit (1048, JovianMass);
		}

	}
}
