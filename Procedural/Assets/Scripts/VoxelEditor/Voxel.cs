using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel : MonoBehaviour
{
    // 0 top, 1 front, 2 right, 3 left, 4 back, 5 bottom
    public GameObject[] neightbours = new GameObject[6];

    private GameObject[] currentNeightbours = new GameObject[6];
}
