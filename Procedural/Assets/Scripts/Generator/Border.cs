using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Represetns the border of a voxel piece.
/// </summary>
[Serializable]
public class Border
{
    public List<Vector3> _vertPos;
    
    public Border(List<Vector3> vertPos)
    {
        this._vertPos = vertPos;
    }

    /// <summary>
    /// Indicates if two given borders are the same.
    /// Two border are the same if they have the same vertices, no matter the order of them in the arrays.
    /// </summary>
    /// <param name="a">First border to compare.</param>
    /// <param name="b">Second border to compare.</param>
    /// <returns>True if both border are the same. False otherwise.</returns>
    public static bool Compare(Border a, Border b)
    {
        List<Vector3> aVerts = new List<Vector3>();
        aVerts.AddRange(a._vertPos);

        if (b._vertPos.Count != a._vertPos.Count)
            return false;

        foreach(var vert in b._vertPos)
        {
            for (int i = 0; i < aVerts.Count; i++)
            {
                if (aVerts[i] == vert)
                {
                    aVerts.RemoveAt(i);
                    break;
                }
            }
        }

        return aVerts.Count == 0;
    }

    /// <summary>
    /// Indicates if two given borders are the same.
    /// Two border are the same if they have the vertices wic componentes are less diferent than a given t threshhold.
    /// </summary>
    /// <param name="a">First border to compare.</param>
    /// <param name="b">Second border to compare.</param>
    /// <param name="t">Gives a threshold difference bewtwenn vector values.</param>
    /// <returns>True if both border are the same. False otherwise.</returns>
    public static bool Compare(Border a, Border b, float t)
    {
        List<Vector3> aVerts = new List<Vector3>();
        aVerts.AddRange(a._vertPos);

        if (b._vertPos.Count != a._vertPos.Count)
            return false;

        foreach(var vert in b._vertPos)
        {
            for (int i = 0; i < aVerts.Count; i++)
            {
                if (Mathf.Abs(aVerts[i].x  - vert.x) > t)
                    continue;

                if (Mathf.Abs(aVerts[i].y  - vert.y) > t)
                    continue;

                if (Mathf.Abs(aVerts[i].z  - vert.z) > t)
                    continue;
                
                aVerts.RemoveAt(i);
                break;
            }
        }

        return aVerts.Count == 0;
    }
}