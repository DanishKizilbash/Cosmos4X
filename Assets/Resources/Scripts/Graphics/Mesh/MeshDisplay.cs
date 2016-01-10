using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{
	public class MeshDisplay
	{
		public Entity entity;
		//
		public bool UpdateRequired = true;
		//
		public GameObject gameObject;
		private MeshFilter meshFilter;
		private MeshRenderer meshRenderer;
		//private BoxCollider collider;
		//mesh
		public Mesh mesh;
		private  List<Vector3> Vertices = new List<Vector3> ();
		private  List<int> Triangles = new List<int> ();
		private  List<Vector2> UV = new List<Vector2> ();
		private  Vector2[] cachedUV;
		public  Vector2 meshSize;
		//texture
		public SpriteAtlas spriteAtlas;
		//private float texLen;
		private float inset;
		private  float tRatio;
		public Vector2 textureVector;
		public Color defaultColor;

		public MeshDisplay (Vector2 iMeshSize, SpriteAtlas iSpriteAtlas, string name = "")
		{
			textureVector = Vector2.zero;
			meshSize = iMeshSize;
			gameObject = new GameObject ();
			gameObject.transform.parent = MeshManager.MeshesGameObject.transform;
			gameObject.name = name;
			setVisibility (false);
			meshFilter = gameObject.AddComponent<MeshFilter> ();
			meshRenderer = gameObject.AddComponent<MeshRenderer> ();
			defaultColor = meshRenderer.material.color;
			//collider = gameObject.AddComponent<BoxCollider> ();
			//collider.center = new Vector3 (0.5f, -0.5f, 0);
			//collider.size = new Vector3 (1f, 1f, 0.1f);
			meshRenderer.useLightProbes = false;
			UpdateSpriteAtlas (spriteAtlas);
			BuildMesh ();
		}
		public void UpdateSpriteAtlas (SpriteAtlas iSpriteAtlas)
		{
			if (iSpriteAtlas != null) {
				spriteAtlas = iSpriteAtlas;
				tRatio = spriteAtlas.tRatio;
				meshRenderer.sharedMaterial = spriteAtlas.material;
				//texLen = spriteAtlas.texture.width;
			} else {
				UpdateSpriteAtlas (TextureManager.DefaultAtlas);
			}
		}
		public void UpdateUV (int x, int y, Vector2 texture)
		{

			if (textureVector != texture) {
				int startV = (int)(4 * (y * meshSize.x + x));
				if (startV < cachedUV.Length - 1 && startV >= 0) {
					cachedUV [startV] = new Vector2 (tRatio * texture.x + inset, tRatio * texture.y + tRatio - inset);
					cachedUV [startV + 1] = new Vector2 (tRatio * texture.x + tRatio - inset, tRatio * texture.y + tRatio - inset);
					cachedUV [startV + 2] = new Vector2 (tRatio * texture.x + tRatio - inset, tRatio * texture.y + inset);
					cachedUV [startV + 3] = new Vector2 (tRatio * texture.x + inset, tRatio * texture.y + inset);
					UpdateRequired = true;
				}
			}
		}
		public void ApplyUpdate ()
		{
			if (UpdateRequired) {
				mesh.uv = cachedUV;
				UpdateRequired = false;
			}
		}
		private void BuildMesh ()
		{

			Vertices.Clear ();
			Triangles.Clear ();
			UV.Clear ();
			mesh = new Mesh ();
			Vector2 tDefault = spriteAtlas.defaultTile;
			for (int px=0; px<meshSize.x; px++) {
				for (int py=0; py<meshSize.y; py++) {
					GenSquare (px, py, tDefault);	
					//GenSquare (px, py, new Vector2 (Random.Range (0, 1 / tRatio), Random.Range (0, 1 / tRatio)));	
				}
			}
			//
			mesh.vertices = Vertices.ToArray ();
			mesh.triangles = Triangles.ToArray ();
			mesh.uv = cachedUV = UV.ToArray ();
			mesh.Optimize ();
			mesh.RecalculateNormals ();
			//
			meshFilter.mesh = mesh;
		}
		private void GenSquare (int x, int y, Vector2 texture)
		{
		
			Vertices.Add (new Vector3 (x, y, 0));
			Vertices.Add (new Vector3 (x + 1, y, 0));
			Vertices.Add (new Vector3 (x + 1, y - 1, 0));
			Vertices.Add (new Vector3 (x, y - 1, 0));
		
			Triangles.Add (0);
			Triangles.Add (1);
			Triangles.Add (3);
			Triangles.Add (1);
			Triangles.Add (2);
			Triangles.Add (3);
		
			UV.Add (new Vector2 (tRatio * texture.x, tRatio * texture.y + tRatio));
			UV.Add (new Vector2 (tRatio * texture.x + tRatio, tRatio * texture.y + tRatio));
			UV.Add (new Vector2 (tRatio * texture.x + tRatio, tRatio * texture.y));
			UV.Add (new Vector2 (tRatio * texture.x, tRatio * texture.y));
		
		
		}
		public void setVisibility (bool visible)
		{
			gameObject.SetActive (visible);
		}
		public void MoveTo (Vector3 Pos)
		{
			//if (gameObject.transform.position != Pos) {
			gameObject.transform.position = Pos;
			//}
		}
		public void setScale (float scale)
		{
			Vector3 scaleVec = new Vector3 (scale, scale, scale);
			if (gameObject.transform.localScale != scaleVec) {
				gameObject.transform.position = scaleVec;
			}
		}
		public void setColor (Color color)
		{
			meshRenderer.material.SetColor ("_Color", color);
		}
	}
}