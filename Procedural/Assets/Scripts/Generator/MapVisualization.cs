using UnityEngine;
using System.Collections.Generic;

public abstract class MapVisualization
{
    public enum Type : int
    {
        Mesh, Text
    }

    protected Transform _rootGO = null;

    /// <summary>
    /// Creates the Game objects associated with the representation.
    /// </summary>
    public abstract void CreateGO(int[,,] map, List<VoxelPiece> pieces);

    /// <summary>
    /// Destroy all gameobjects associated with the representation. These GO
    /// must be children of the GO having the MapRepresentation component.
    /// </summary>
    public void Clean()
    {
        if (!_rootGO)
            return;
        while (_rootGO.childCount > 0)
            Object.DestroyImmediate(_rootGO.GetChild(0).gameObject);
    }
}