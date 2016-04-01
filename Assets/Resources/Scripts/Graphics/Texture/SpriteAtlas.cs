using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public class SpriteAtlas
	{
		public string name;
		public Material material;
		public int tileSize;
		public Texture2D texture;
		public float tRatio;
		private float texLen;
		public Vector2 tAtlasSize;
		public int buffer = 0;
		//
		public Vector2 defaultTile;
		private Dictionary<string,Vector2> texturePositions;
		private List<List<Texture2D>> childTextures;
		private List<Texture2D>mergedTextures;
		//

		public SpriteAtlas (int TileSize, string Name)
		{
			name = Name;
			tileSize = TileSize;
			defaultTile = new Vector2 (0, 0);
			childTextures = new List<List<Texture2D>> ();
			texturePositions = new Dictionary<string, Vector2> ();
			mergedTextures = new List<Texture2D> ();
			Finder.spriteAtlasDatabase.Add (this);

		}
		public void Init ()
		{
			MergeTextures ();
			CreateNewAtlas ();
			material = new Material (Shader.Find ("Transparent/Cutout/Diffuse"));	
			material.mainTexture = texture;
			tRatio = 1f / texLen;
		}

		public Vector2 GetTextureVector (string name)
		{
			Vector2 vec = Vector2.zero;
			texturePositions.TryGetValue (name, out vec);
			return vec;
		}

		//
		private void CreateNewAtlas ()
		{

			int numTextures = mergedTextures.Count;
			texLen = (int)Mathf.Ceil (Mathf.Sqrt (numTextures));
			int fixedTileSize = tileSize + buffer;
			texture = new Texture2D ((int)(texLen * fixedTileSize), (int)(texLen * fixedTileSize), TextureFormat.RGBA32, false);
			texture.wrapMode = TextureWrapMode.Clamp;
			//
			int i = 0; 
			for (int x = 0; x<texLen*fixedTileSize; x+=fixedTileSize) {
				for (int y = 0; y<texLen*fixedTileSize; y+=fixedTileSize) {
					if (i >= mergedTextures.Count) {
						break;
					}
					Color[] tColors = mergedTextures [i].GetPixels ();
					texture.SetPixels (x + buffer, y + buffer, mergedTextures [i].width, mergedTextures [i].height, tColors);
					texturePositions.Add (mergedTextures [i].name, new Vector2 (x / tileSize, y / tileSize));
					CreateTextureBuffer (new Vector2 (x, y));
					i++;
				}
			}
			texture.Apply ();
		}
		private void CreateTextureBuffer (Vector2 pos)
		{
			int tlen = (int)(tileSize + buffer);
			int x = (int)pos.x;
			int y = (int)pos.y;
			for (int a=x; a<tlen; a++) {
				texture.SetPixel (a, y, Color.clear);
				texture.SetPixel (a, y + tlen, Color.clear);
			}
			for (int b=y; b<tlen; b++) {
				
				texture.SetPixel (x, b, Color.clear);
				texture.SetPixel (x + tlen, b, Color.clear);
			}
		}
		public void AddTextureList (List<Texture2D> Textures)
		{
			childTextures.Add (Textures);
		}
		private void AddTransparentTexture ()
		{
			mergedTextures.Add (TextureManager.ClearTexture);
		}
		private void MergeTextures ()
		{
			AddTransparentTexture ();
			AddBaseTextures ();
			//AddMergedTextures ();
			//Debug.Log ("Merge texture count: " + mergedTextures.Count);
		}
		private void AddMergedTextures ()
		{
			for (int x =0; x<childTextures.Count; x++) {
				for (int y =0; y<childTextures.Count; y++) {
					if (x != y) {
						for (int a =0; a<childTextures[x].Count; a++) {
							for (int b =0; b<childTextures[y].Count; b++) {
								mergedTextures.Add (PerformTexMerge (childTextures [x] [a], childTextures [y] [b]));
							}
						}
					}
				}
			}
		}
		private void AddBaseTextures ()
		{
			for (int x =0; x<childTextures.Count; x++) {
				for (int y =0; y<childTextures[x].Count; y++) {
					mergedTextures.Add (childTextures [x] [y]);
				}
			}
		}
		private Texture2D PerformTexMerge (Texture2D Texture1, Texture2D Texture2)
		{

			int width1 = Texture1.width;
			int height1 = Texture1.height;
			
			int width2 = Texture2.width;
			int height2 = Texture2.height;
			Texture2D newTex = new Texture2D ((int)MathI.Max (width1, width2), (int)MathI.Max (height1, height2), TextureFormat.RGBA32, false);
			newTex.wrapMode = TextureWrapMode.Clamp;
			
			// loop through texture
			int y = 0;
			while (y <= height2) {
				int x = 0;
				while (x <= width2) {
					Color pixel1 = Texture1.GetPixel (x, y);
					Color pixel2 = Texture2.GetPixel (x, y);
					Color mergedPixel = pixel2.a != 0 ? pixel2 : pixel1;
					newTex.SetPixel (x, y, mergedPixel);
					x++;
				}
				y++;
			}
			newTex.Apply ();
			newTex.name = Texture1.name + "-" + Texture2.name;
			return newTex;
		}
		public override string ToString ()
		{
			return name;
		}
	}
}