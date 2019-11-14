using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width = 6;

    public int depth = 6;

    public int height = 12;

    public GameObject[,,] currentVoxels;

    public GameObject groundVoxel;

    void Start() {
        currentVoxels = new GameObject[width, height, depth];

        for(int x = 0; x < width; x++) {
            for(int z = 0; z < depth; z++) {
                GameObject voxel = (GameObject) Instantiate(groundVoxel, new Vector3(x, 0, z), Quaternion.identity);
                voxel.name = "Voxel " + x + " 0 " + z;
                voxel.transform.parent = this.transform;
                currentVoxels[x, 0, z] = voxel; 
            }
        }
    }
}
