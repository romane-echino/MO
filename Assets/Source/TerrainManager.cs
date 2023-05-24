using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameObject[] TilePrefabs;

    public Terrain Terrain { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Load(){
        GameManager.Instance.Network.GetTerrain();
    }

    public void SetTerrain(Terrain remoteTerrain)
    {
        this.Terrain = remoteTerrain;

        var terrainRoot = new GameObject("terrainRoot");
        foreach (var chunkKey in this.Terrain.Chunks.Keys)
        {
            var chunkGO = new GameObject(chunkKey);
            chunkGO.transform.parent = terrainRoot.transform;

            foreach (var tileKey in this.Terrain.Chunks[chunkKey].Tiles.Keys)
            {
                var keySplit = tileKey.Split("_");
                var tileIndex = (int)this.Terrain.Chunks[chunkKey].Tiles[tileKey].TileType;

                var tileGO = Instantiate(this.TilePrefabs[tileIndex], new Vector3(int.Parse(keySplit[0]) - .5f, int.Parse(keySplit[1]) - .5f, 0), Quaternion.identity, chunkGO.transform);
                tileGO.name = tileKey;
            }
        }

    }
}

[System.Serializable]
public class Terrain
{
    public Dictionary<string, Chunk> Chunks;
}


[System.Serializable]
public class Chunk
{
    public Dictionary<string, ChunkTile> Tiles;
}

[System.Serializable]
public class ChunkTile
{
    public TileType TileType;
}

public enum TileType
{
    UNKNOWN = 0,
    GRASS = 1,
    WATER = 2,
}
