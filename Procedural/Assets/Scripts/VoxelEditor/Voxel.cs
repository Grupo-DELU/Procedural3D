using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public int voxelID;

    // OLD
    //public GameObject[] neightbours = new GameObject[6];

    [Header("Neighbors")]
    public List<Voxel> top = new List<Voxel>();
    public List<Voxel> front = new List<Voxel>();
    public List<Voxel> right = new List<Voxel>();
    public List<Voxel> left = new List<Voxel>();
    public List<Voxel> back = new List<Voxel>();
    public List<Voxel> bottom = new List<Voxel>();

}
