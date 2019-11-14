using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    public int voxelID;
    
    // OLD
    public GameObject[] neightbours = new GameObject[6];

    [Header("Neightbours")]

    public List<GameObject> top = new List<GameObject>();
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> bot = new List<GameObject>();
}
