using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public static class ResourceManager
	{
		private static List<Resource> ResourceList;
		public static void Init ()
		{
			ResourceList = new List<Resource> ();
			ResourceList.Add (new Resource ("Steel"));
			ResourceList.Add (new Resource ("Lead"));
			ResourceList.Add (new Resource ("Gold"));
			ResourceList.Add (new Resource ("Carbon"));
		}
		public static void RandomizeStockpile (Entity entity, List<Resource> targetResources=null)
		{
			if (targetResources == null) {
				targetResources = ResourceList;
			}
			foreach (Resource res in ResourceList) {
				if (targetResources.Contains (res)) {
					entity.stockpile.Add (res, Random.Range (1000, 100000));
				}
			}
			entity.stockpile.Print ();
		}
	}
}
