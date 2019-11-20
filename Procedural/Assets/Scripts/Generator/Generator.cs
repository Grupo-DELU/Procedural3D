using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class to implemt different procedural generated map that are divided in voxels and use
/// a set of pieces to build the map.
/// </summary>
[ExecuteInEditMode]
public abstract class Generator
{
    public enum Type : int
    {
        WFC, VPPreview
    }

    protected List<VoxelPiece> _pieces = null;
    protected int[,,] _map = null;

    /// <summary>
    /// Gets the last map generated.
    /// </summary>
    /// <value></value>
    public int[,,] Map
    {
        get {
            if (_map == null)
                CreateMap(_pieces);

            return _map;
        }
    }

    /// <summary>
    /// Initialize the map array with the correct dimensions and its value in the most convinient way.
    /// </summary>
    protected abstract void InitMap();

    /// <summary>
    /// Generates the map, filling tht _map 3-dimensional array
    /// </summary>
    protected abstract void Generate();

    /// <summary>
    /// Returns true if the preconditions are all met;
    /// </summary>
    /// <returns></returns>
    protected virtual bool CheckPrecondition()
    {
        if (_pieces == null || _pieces.Count == 0)
        {
            Debug.Log("No pieces provided.");
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Check variables and call for the generation of the map
    /// </summary>
    public void CreateMap(List<VoxelPiece> pieces) 
    {
        _pieces = pieces;
        
        if (CheckPrecondition())
        {
            InitMap();
            Generate();
        }
    }
}