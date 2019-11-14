using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCreator : MonoBehaviour
{
    public GameObject grid;

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

                int randomVoxel = (int) UnityEngine.Random.Range(0, validVoxels.Count);

                GameObject block = (GameObject) Instantiate(validVoxels[randomVoxel], blockPos, Quaternion.identity);
                block.transform.parent = this.transform;
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
}
