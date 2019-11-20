using UnityEngine;

/// <summary>
/// Scriptable Object with the candidates to be a VoxelPiece with basic info to automate its initialiazation.
/// </summary>
[CreateAssetMenu(menuName = "PCG/Raw Voxel Piece")]
public class RawVoxelPiece : ScriptableObject 
{
    [SerializeField]
    private Mesh _mesh = null;

    [Tooltip("Consideres the same piece mirrored over the x-axis.")]
    [SerializeField]
    private bool _mirrorX = false;
    
    [Tooltip("Consideres the same piece mirrored over the y-axis.")]
    [SerializeField]
    private bool _mirrorY = false;
    
    [Tooltip("Consideres the same piece mirrored over the z-axis.")]
    [SerializeField]
    private bool _mirrorZ = false;

    [Tooltip("Consideres the same piece rotated over the x-axis.")]
    [SerializeField]
    private bool _rotateX = false;

    [Tooltip("Consideres the same piece rotated over the y-axis.")]
    [SerializeField]
    private bool _rotateY = false;

    [Tooltip("Consideres the same piece rotated over the z-axis.")]
    [SerializeField]
    private bool _rotateZ = false;

    [SerializeField]
    private bool _isGround = false;

    /// <summary>
    /// Gets the mesh of the piece.
    /// </summary>
    public Mesh mesh {
        get {
            return _mesh;
        }
    }

    /// <summary>
    /// Gets if piece has to considered rotated in x-axis.
    /// </summary>
    public bool rotateX {
        get {
            return _rotateX;
        }
    }

    /// <summary>
    /// Gets if piece has to considered rotated in y-axis.
    /// </summary>
    public bool rotateY {
        get {
            return _rotateY;
        }
    }

    /// <summary>
    /// Gets if piece has to considered rotated in z-axis.
    /// </summary>
    public bool rotateZ {
        get {
            return _rotateZ;
        }
    }

    /// <summary>
    /// Gets if the piecr has to be considered mirrored against the x-axis
    /// </summary>
    public bool mirrorX {
        get {
            return _mirrorX;
        }
    }

    /// <summary>
    /// Gets if the piecr has to be considered mirrored against the y-axis
    /// </summary>
    public bool mirrorY {
        get {
            return _mirrorY;
        }
    }

    /// <summary>
    /// Gets if the piecr has to be considered mirrored against the z-axis
    /// </summary>
    public bool mirrorZ {
        get {
            return _mirrorZ;
        }
    }

    /// <summary>
    /// Gets if the piecr has to be considered mirrored against the z-axis
    /// </summary>
    public bool IsGround {
        get {
            return _isGround;
        }
    }
}