using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos.AI.Pathfinding
{
	public static class PathFinder
	{
		private static Dictionary<Vector3,node> Grid;
		private static float GridHeight;
		private static float GridWidth;
		private static float GridDepth;
		private static SortedDictionary<int,List<node>> open;
		public static List<Vector3> FindPath (Vector3 origin, Vector3 target, int maxDuration = 10000)
		{
			List<Vector3> tilePath = new List<Vector3> ();
			/*if (origin == target) {
				tilePath.Add (Finder.TileDatabase.GetTile (origin));
				return tilePath;
			}
			//float startTime = Time.realtimeSinceStartup;
			//Debug.Log ("______Pathfinder Start______ Origin: " +origin + " Target :"+target);
			createGrid (origin, target);
			List<node> nodePath = findPath (origin, target, maxDuration);

			//Debug.Log ("Path found:");
			if (nodePath != null && nodePath.Count > 0) {
				foreach (node n in nodePath) {
					tilePath.Add (Finder.TileDatabase.GetTile (n.pos));
					//Debug.Log (n.pos);
				}
			} else {
				//Debug.Log ("No path found");
			}
			//Debug.Log ("Pathfinder Complete Total Time (ms): " + ((Time.realtimeSinceStartup - startTime) * 1000).ToString ());
*/
			return tilePath;
		}

		private static node getNode (Vector3 position)
		{
			node value = null;
			Grid.TryGetValue (position, out value);
			return value;
		}
		private static void addNodeToOpen (node node)
		{
			int fCost = node.fCost;
			List<node> nodeList = null;
			if (open.TryGetValue (fCost, out nodeList)) {
				nodeList.Add (node);
				open [fCost] = nodeList;
			} else {
				nodeList = new List<node> ();
				nodeList.Add (node);
				open.Add (fCost, nodeList);
			}
		}
		private static void removeNodeFromOpen (node node)
		{
			int fCost = node.fCost;
			List<node> nodeList = null;
			if (open.TryGetValue (fCost, out nodeList)) {
				nodeList.RemoveAt (0);
				if (nodeList.Count == 0) {
					open.Remove (fCost);
				} else {
					open [fCost] = nodeList;
				}

			}
		}
		private static bool isNodeInOpen (node node)
		{
			int fCost = node.fCost;
			List<node> nodeList = null;
			if (open.TryGetValue (fCost, out nodeList)) {
				if (nodeList.Contains (node)) {
					return true;	
				}
			}
			return false;
		}
		private static List<node> findPath (Vector3 Origin, Vector3 Target, int durationLimit)
		{
			int duration = 0;

			open = new SortedDictionary<int,List<node>> ();
			node curNode = getNode (Origin);
			curNode.hCost = getCost (Origin, Target);
			curNode.gCost = 0;
			addNodeToOpen (curNode);


			
			while (duration<durationLimit && open.Count >0) {
				//Sort to find lowest fCost node
				node current = null;
				foreach (List<node> n in open.Values) {
					current = n [0];// set Current node to lowest fCost node in open list
					break;
				}
				if (current == null) {
					return null;
				}
				//open.Values [0] [0];
				//Debug.Log (current.fCost);
				removeNodeFromOpen (current);
				current.isClosed = true;
				Grid [current.pos].isClosed = true;

				if (current.pos == Target) {
					return getPathFromNode (current); // return current node and all parent nodes if I've arrived at the target
				}
				List<node> neighbours = getNeighbours (current, Origin, Target);
				foreach (node n in neighbours) {
					curNode = getNode (n.pos);
					if (curNode.walkable) {
						int newGCost = getCost (current.pos, curNode.pos) + current.gCost;
						if (curNode.isClosed) {
							int newFCost = newGCost + curNode.hCost;
							if (curNode.fCost <= newFCost) {
								continue;
							}
							curNode.isClosed = false;
							curNode.gCost = newGCost;
							curNode.parent = current;
							Grid [curNode.pos] = curNode;
						}
						if (!isNodeInOpen (curNode)) { //get rid of this and fix sortedlist
							curNode.hCost = getCost (curNode.pos, Target);
							curNode.gCost = newGCost;
							curNode.parent = current;
							addNodeToOpen (curNode);
							Grid [curNode.pos] = curNode;
						}
					}
				}
				//
				duration++;
				if (open.Count == 0) {
					return null;
				}
			}
			return null;

		}
		private static node getLowestfCost (Dictionary<Vector3,node> list)
		{
			//node lowestCostNode = list.Aggregate ((c,d) => c.Value.fCost < d.Value.fCost ? c : d).Value;
			node lowestCostNode = null;
			int lowestCost = int.MaxValue;
			foreach (node n in list.Values) {
				if (n.fCost < lowestCost) {
					lowestCost = n.fCost;
					lowestCostNode = n;
				}
			}
			return lowestCostNode;
		}
		private static void createGrid (Vector3 Origin, Vector3 Target)
		{/*
			List<List<List<Tile>>> Map = Finder.TileDatabase.TileMap;
			//int y1 = Origin.y < Target.y ? (int)Origin.y : (int)Target.y;
			//int y2 = Origin.y > Target.y ? (int)Origin.y : (int)Target.y;
			int x1 = Origin.x < Target.x ? (int)Origin.x : (int)Target.x;
			int x2 = Origin.x > Target.x ? (int)Origin.x : (int)Target.x;
			int z1 = Origin.z < Target.z ? (int)Origin.z : (int)Target.z;
			int z2 = Origin.z > Target.z ? (int)Origin.z : (int)Target.z;
			GridWidth = x2;
			GridDepth = z2;
			GridHeight = Map.Count;
			Vector3 gridOffset = new Vector3 (x1, 0, z1);
			Grid = new Dictionary<Vector3,node> ();
			for (int y =0; y< GridHeight; y++) {
				for (int x =0; x<= x2-x1; x++) {
					for (int z =0; z<= z2-z1; z++) {	
						Tile tile = Map [y + (int)gridOffset.y] [x + (int)gridOffset.x] [z + (int)gridOffset.z];
						bool isClearAbove = false;

						if (y == 0) {
							isClearAbove = true;
						} else {
							Tile aboveTile = Map [y + (int)gridOffset.y - 1] [x + (int)gridOffset.x] [z + (int)gridOffset.z];
							if (aboveTile == null || aboveTile.Children.Count == 0) {
								isClearAbove = true;
							}
						}
						if (tile != null) {
							Grid .Add (tile.Position, new node (tile.Position, tile.isWalkable && isClearAbove));
						} else {
							Grid .Add (new Vector3 (x, y, z) + gridOffset, null);
						}
					}
				}
			}
			  */
			//Debug.Log (Grid.Count * Grid [0].Count * Grid [0] [0].Count);
		}
		private static List<node> getPathFromNode (node Node)
		{
			node curNode = Node;
			List<node> list = new List<node> ();
			while (curNode.parent !=null) {
				list.Insert (0, curNode);
				//Debug.Log (curNode.pos);
				curNode = curNode.parent;
			}
			return list;
		}

		private static List<node> getNeighbours (node n, Vector3 Origin, Vector3 Target)
		{
			List<node> nList = new List<node> ();
			Vector3 tPos = n.pos;
			//
			for (int y =-1; y<= 1; y++) {
				for (int x =-1; x<= 1; x++) {
					for (int z =-1; z<= 1; z++) {
						int ny = (int)tPos.y + y;
						int nx = (int)tPos.x + x;
						int nz = (int)tPos.z + z;
						if (ny < 0 || ny > GridHeight)
							continue;
						if (nx < 0 || nx > GridWidth)
							continue;
						if (nz < 0 || nz > GridDepth)
							continue;
						if (y == 0 && x == 0 && z == 0)
							continue;
						//Debug.Log (new Vector3(x,y,z));
						node curNode = null;
						Grid.TryGetValue (new Vector3 (nx, ny, nz), out curNode);
						if (curNode != null) {	
							nList.Add (curNode);
							//Debug.Log (nList[nList.Count-1].gCost);						
						}
					}
				}
			}

			//Debug.Log ("");
			return nList;
		
		}
		private static int getCost (Vector3 orig, Vector3 targ)
		{
			int xDist = (int)Mathf.Abs (targ.x - orig.x);
			int yDist = (int)Mathf.Abs (targ.y - orig.y);
			int zDist = (int)Mathf.Abs (targ.z - orig.z);
			int cost = 10 * (xDist + yDist + zDist);
			/*if (xDist != 0 && zDist != 0) {
				cost += 0*(xDist+zDist);
			}*/
			if (yDist != 0) {
				cost += 4 * yDist;
			}
			return cost;
		}
	}
	public class node
	{
		public Vector3 pos; // Position
		public int hCost;//Distance from end node
		public int gCost;//Distance from starting node
		public int fCost{ get { return hCost + gCost; } }//Total cost
		public node parent; // Parent Node
		public bool walkable; // is it an obstacle?
		public bool isClosed = false;
		public node (Vector3 Position, bool Walkable, int HCost=int.MaxValue, int GCost=int.MaxValue, node Parent=null)
		{ 
			pos = Position;
			walkable = Walkable;
			hCost = HCost;
			gCost = GCost;
			parent = Parent;
		}

	}
}