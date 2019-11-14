﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCreator : MonoBehaviour
{
    public Grid grid;

    public List<GameObject> validVoxels;
    
    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Debug.Log("Shoot");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 1000f)){
                Vector3 blockPos = hit.point + hit.normal/2.0f;
                
                blockPos.x = (float) Math.Round(blockPos.x, MidpointRounding.AwayFromZero);
                blockPos.y = (float) Math.Round(blockPos.y, MidpointRounding.AwayFromZero);
                blockPos.z = (float) Math.Round(blockPos.z, MidpointRounding.AwayFromZero);
                Debug.Log(blockPos);
                Debug.Log(grid.ocuppiedVoxels[(int) blockPos.x, (int) blockPos.y, (int) blockPos.z]);

                if(grid.ocuppiedVoxels[(int) blockPos.x, (int) blockPos.y, (int) blockPos.z]) {
                    return;
                }

                int randomVoxel = (int) UnityEngine.Random.Range(0, validVoxels.Count);

                Voxel voxelInfo = validVoxels[randomVoxel].GetComponent<Voxel>();
                if(CheckNeighbors(voxelInfo, (int) blockPos.x, (int) blockPos.y, (int) blockPos.z)) {
                    GameObject block = (GameObject) Instantiate(validVoxels[randomVoxel], blockPos, Quaternion.identity);
                    block.transform.parent = grid.transform;
                    block.name = "Voxel " + block.transform.position.x + " " + block.transform.position.y + " " + block.transform.position.z;
                    block.transform.parent = grid.transform;
                    grid.currentVoxels[(int) blockPos.x, (int) blockPos.y, (int) blockPos.z] = block;
                    grid.ocuppiedVoxels[(int) blockPos.x, (int) blockPos.y, (int) blockPos.z] = true;
                }
            }
        } else if (Input.GetMouseButtonDown(1)) {
            Debug.Log("Shoot");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 1000f)){
                if(hit.transform.tag == "Voxel") {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private bool CheckNeighbors(Voxel voxel, int x, int y, int z) {
        // Check top voxel
        if(OnBoundary(x, y + 1, z)) {
            if(grid.ocuppiedVoxels[x, y+1, z]) {
                Voxel neighbor = grid.currentVoxels[x, y+1, z].GetComponent<Voxel>();

                bool containsVoxel = neighbor.bottom.Any(item => item.voxelID == voxel.voxelID);

                if(!containsVoxel) {
                    return false;
                }
            }
        }

        // Check front voxel
        if(OnBoundary(x, y, z - 1)) {
            if(grid.ocuppiedVoxels[x, y, z - 1]) {
                Voxel neighbor = grid.currentVoxels[x, y, z - 1].GetComponent<Voxel>();

                bool containsVoxel = neighbor.back.Any(item => item.voxelID == voxel.voxelID);

                if(!containsVoxel) {
                    return false;
                }
            }
        }

        // Check right voxel
        if(OnBoundary(x + 1, y, z)) {
            if(grid.ocuppiedVoxels[x + 1, y, z]) {
                Voxel neighbor = grid.currentVoxels[x + 1, y, z].GetComponent<Voxel>();

                bool containsVoxel = neighbor.left.Any(item => item.voxelID == voxel.voxelID);

                if(!containsVoxel) {
                    return false;
                }
            }
        }

        // Check left voxel
        if(OnBoundary(x - 1, y, z)) {
            if(grid.ocuppiedVoxels[x - 1, y, z]) {
                Voxel neighbor = grid.currentVoxels[x - 1, y, z].GetComponent<Voxel>();

                bool containsVoxel = neighbor.right.Any(item => item.voxelID == voxel.voxelID);

                if(!containsVoxel) {
                    return false;
                }
            }
        }

        // Check back voxel
        if(OnBoundary(x, y, z + 1)) {
            if(grid.ocuppiedVoxels[x, y, z + 1]) {
                Voxel neighbor = grid.currentVoxels[x, y, z + 1].GetComponent<Voxel>();

                bool containsVoxel = neighbor.front.Any(item => item.voxelID == voxel.voxelID);

                if(!containsVoxel) {
                    return false;
                }
            }
        }

        // Check bottom voxel
        if(OnBoundary(x, y - 1, z)) {
            if(grid.ocuppiedVoxels[x, y-1, z]) {
                Voxel neighbor = grid.currentVoxels[x, y-1, z].GetComponent<Voxel>();

                bool containsVoxel = neighbor.top.Any(item => item.voxelID == voxel.voxelID);
                bool containsNeighbor = voxel.bottom.Any(item => item.voxelID == neighbor.voxelID);

                if(!containsVoxel || !containsNeighbor) {
                    return false;
                }
            }
        }

        return true;
    }

    private bool OnBoundary(int x, int y, int z) {
        return (x >= 0 && x < grid.width) && (y >= 0 && y < grid.height) &&(z >= 0 && z < grid.depth);
    }
}
