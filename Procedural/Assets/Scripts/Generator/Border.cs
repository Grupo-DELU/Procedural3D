using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Represetns the border of a voxel piece.
/// </summary>
public class Border
{
    private List<Vector3> _vertPos;
    
    public Border(List<Vector3> vertPos)
    {
        this._vertPos = vertPos;
    }
}