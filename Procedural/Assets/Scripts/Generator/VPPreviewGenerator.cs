using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generator tha creates a map to preview all the pieces that are identified by its Voxel Piece Identifier.
/// </summary>
[ExecuteInEditMode]
public class VPPreviewGenerator : Generator
{
    public VPPreviewGenerator (List<VoxelPiece> pieces)
    {
        _pieces = pieces;
    }

    /// <summary>
    /// Generates a map comsisting of one row with one instance of each possible piece on it.
    /// </summary>
    protected override void Generate()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            _map[i,0,0] = i;
        }
    }

    /// <summary>
    /// Initialize the map array with the correct dimensions and assign the borders of the cube.
    /// </summary>
    protected override void InitMap()
    {
        _map = new int[_pieces.Count,1,1];
    }

}