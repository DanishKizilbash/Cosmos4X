using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Eniso {
    public class Tile {
        public Chunk chunk;
        public int lightLevel;
        private LightLevelEntity lightLevelEntity;
        private bool walkable = true;
        private List<Tile> cachedNeighbours = null;
        public List<Tile> neighbours {
            get {
                if (cachedNeighbours == null) {
                    cachedNeighbours = Finder.TileDatabase.getNeighbouringTiles(this);
                }
                return cachedNeighbours;
            }
        }
        public bool isWalkable {
            get {
                if (Children.Count == 0) {
                    walkable = false;
                }
                return walkable;
            }
            set {
                walkable = value;
            }
        }
        public bool isVisible {
            get {
                bool check = false;
                foreach (Entity child in Children) {
                    if (child.isInitiated) {
                        if (child.isVisible) {
                            check = true;
                            break;
                        }
                    }
                }
                return check;
            }
            set {
                foreach (Entity child in Children) {
                    if (child.isInitiated) {
                        child.SetVisible(value);
                    }
                }
            }
        }
        public Vector3 Position = Vector3.zero;
        public List<Entity> Children = new List<Entity>();
        public Vector3 Center {
            get {
                Vector3 pos = Position;
                float scale = DrawManager.tileSize / 100;
                Vector3 tileCenter = new Vector3(0, 0, pos.z);
                tileCenter.x = pos.x + scale / 2f;
                tileCenter.y = pos.y + scale / 2f;
                return tileCenter;
            }
        }
        public Vector3 ScreenPosition {
            get {
                return MathI.IsoToWorld(Center);
            }
        }
        public Tile(Vector3 pos) {
            Position = pos;
            Finder.TileDatabase.AddTile(this);
        }
        public int ModifyLightLevel(int val) {
            lightLevel += val;
            if (lightLevel > 9) {
                lightLevel = 9;
            }
            if (lightLevel < 0) {
                lightLevel = 0;
            }
            setChildrenLightLevel();
            return lightLevel;
        }
        private void setChildrenLightLevel() {
            foreach (Entity child in Children) {
                if (child.isInitiated) {
                    child.QueForDraw();
                }
            }
        }

        public void AddLightLevelDisplay() {
            if (lightLevelEntity == null && isVisible) {
                lightLevelEntity = new LightLevelEntity();
                lightLevelEntity.Init("Lighting_LightDisplay_LightLevel_LightLevel");
                AddChild(lightLevelEntity);
                lightLevelEntity.ModifyLightLevel(5);
            }
        }
        public void UpdateChunk() {
            if (isVisible) {
                DrawManager.QueChunkForUpdate(this.chunk);
            }
        }
        public List<Entity> GetChildren() {
            return Children;
        }
        public void AddChild(Entity entity) {
            if (entity.adoptable) {
                entity.SetNewParentTile(this);
            }
        }
        public void MoveTo(Vector3 iposition) {
            Position = iposition;
        }
        public void MoveTo(float x, float y, float z) {
            Position = new Vector3(x, y, z);
        }
        public override string ToString() {
            return Position.ToString();
        }

    }
}