using UnityEngine;

public class VoxelPiece
{
    private Mesh _mesh;
    private int[] _borders;

    // Instancing transform values
    private Vector3 _rotOffset;
    private Vector3 _scaleOffset;

    /// <summary>
    /// Return the Mesh associated with the Voxel Piece
    /// </summary>
    public Mesh Mesh {
        get {
            return _mesh;
        }
    }

    /// <summary>
    /// Return the indicated border associated with the Voxel Piece.false Borders are ordered according to the driection Enum.
    /// </summary>
    public int this[int index] {
        get {
            return _borders[index];
        }
    }

    /// <summary>
    /// Return the Rotation offset vector associated with the Voxel Piece
    /// </summary>
    public Vector3 RotOfftset {
        get {
            return _rotOffset;
        }
    }

    /// <summary>
    /// Return the Scale offset vector associated with the Voxel Piece
    /// </summary>

    public Vector3 ScaleOffset {
        get {
            return _scaleOffset;
        }
    }

    public VoxelPiece(Mesh mesh, int[] borders, Vector3 rotationOffset, Vector3 _scaleOffset)
    {
        this._mesh = mesh;
        this._borders = borders;
        this._rotOffset = rotationOffset;
        this._scaleOffset = _scaleOffset;
    }

    /// <summary>
    /// Given two voxel pices and a side, returns if they connect by that side
    /// </summary>
    /// <param name="a">Voxel Piece to take side into account.</param>
    /// <param name="b">Voxel Piece to check if connection is possible.</param>
    /// <param name="s">Side to consider in Voxel a.</param>
    /// <returns>True if connection is possible by the given side. Otherwise, false.</returns>
    public static bool IsConnected(VoxelPiece a, VoxelPiece b, Side s)
    {
        switch (s)
        {
            case Side.back:
                return a[(int)Side.back] == b[(int)Side.front];
            case Side.down:
                return a[(int)Side.down] == b[(int)Side.up];
            case Side.front:
                return a[(int)Side.front] == b[(int)Side.back];
            case Side.left:
                return a[(int)Side.left] == b[(int)Side.right];
            case Side.right:
                return a[(int)Side.right] == b[(int)Side.left];
            case Side.up:
                return a[(int)Side.up] == b[(int)Side.down];
        }

        return false;
    }
}