using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int width = 6;

    public int height = 6;

    public int depth = 6;
    
    public Grid grid;

    public List<GameObject> tiles;

    public bool[,,] currentGrid;

    void Start() {
        currentGrid = new bool[width,height,depth];

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                for(int z = 0; z < depth; z++) {
                    currentGrid[x,y,z] = false;
                }
            }
        }
    }
    /*
    public void GenerateWorld() {
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < depth; z++) {
                Debug.Log("POS " + x + " 1 " + z);
                bool spawned = false;
                int i = 0;

                while(!spawned) {
                    //int randomIndex = Random.Range(0, tiles.Count);
                    GameObject randomTile = tiles[i];
                    Voxel tileInfo = randomTile.GetComponent<Voxel>();
                    Debug.Log("Try number " + i + " with tile " + tileInfo.voxelID);
                    //bool canSpawn = true;

                    if(OnBoundary(x, z + 1)) {
                        if(currentGrid[x, 1, z + 1]) {
                            if(tileInfo.neightbours[4].GetComponent<Voxel>().voxelID != grid.currentVoxels[x, 1, z + 1].GetComponent<Voxel>().voxelID) {
                                i++;
                                continue;
                            }   
                        }
                    }
                    
                    if(OnBoundary(x + 1, z)) {
                        if(currentGrid[x + 1, 1, z]) {
                            if(tileInfo.neightbours[2].GetComponent<Voxel>().voxelID != grid.currentVoxels[x + 1, 1, z].GetComponent<Voxel>().voxelID) {
                                i++;
                                continue;
                            }   
                        }
                    }

                    if(OnBoundary(x, z - 1)) {
                        if(currentGrid[x, 1, z - 1]) {
                            if(tileInfo.neightbours[1].GetComponent<Voxel>().voxelID != grid.currentVoxels[x, 1, z - 1].GetComponent<Voxel>().voxelID) {
                                i++;
                                continue;
                            }   
                        }
                    }

                    if(OnBoundary(x - 1, z)) {
                        if(currentGrid[x - 1, 1, z]) {
                            if(tileInfo.neightbours[3].GetComponent<Voxel>().voxelID != grid.currentVoxels[x - 1, 1, z].GetComponent<Voxel>().voxelID) {
                                i++;
                                continue;
                            }   
                        }
                    }

                    
                    GameObject newTile = (GameObject) Instantiate(randomTile, new Vector3(x, 1, z), Quaternion.identity);
                    newTile.name = "Voxel " + x + " 1 " + z;
                    newTile.transform.parent = this.transform;
                    grid.currentVoxels[x, 1, z] = newTile;
                    currentGrid[x, 1, z] = true;
                    spawned = true;
                }
            }
        }
    }
    */
    private bool OnBoundary(int x, int z) {
        return (x >= 0 && x < width) && (z >= 0 && z < depth);
    }
}
