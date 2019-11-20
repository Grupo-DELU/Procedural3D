using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that given an array of meshes, identifies ther borders of each.
/// </summary>
[Serializable]
public class VoxelPieceIdentifier : MonoBehaviour
{
    private enum Axis{
        x,y,z
    }

    [SerializeField]
    private RawVoxelPiece[] _candidates = null;
    [SerializeField]
    private float _voxelSize = 1;
    [SerializeField]
    private float _threshold = 0;

    private List<VoxelPiece> _voxelPieces = null; 
    private List<Border> _borders = null;

    /// <summary>
    /// Returns a list with the voxel pieces identified.
    /// </summary>
    /// <value></value>
    public List<VoxelPiece> Pieces
    {
        get {
            if (_voxelPieces == null || (_voxelPieces.Count == 0 && _candidates.Length > 0))
                IdentifyPieces();
            return _voxelPieces;
        }
    }

    /// <summary>
    /// Creates the voxel pieces based on the raw voxel pieces, doing any post processing necesarry to the pieces, as well as creating
    /// all the needed aditional data.
    /// </summary>
    public void IdentifyPieces()
    {
        _voxelPieces = new List<VoxelPiece>();
        _borders = new List<Border>();

        // Empty Space, the fisrt piece is always an empty voxel.
        Mesh emptyMesh = new Mesh();
        _voxelPieces.Add(
            new VoxelPiece(
                emptyMesh,
                IdentifyPieceBorders(emptyMesh, false),
                Vector3.one,
                Vector3.zero
            )
        );

        int[] cBorders;
        Vector3 cRotOffset, cSclOffset;


        foreach (var p in _candidates)
        {
            List<VoxelPiece> cVps = new List<VoxelPiece>();
            List<VoxelPiece> vpsToAdd = new List<VoxelPiece>();
            
            // Consider base piece
            cBorders = IdentifyPieceBorders(p.mesh, p.IsGround);
            vpsToAdd.Add(new VoxelPiece(p.mesh, cBorders, Vector3.zero, Vector3.one));
            
            if (p.mirrorX)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = MirrorMesh(vp.Mesh, Axis.x);
                    cBorders = IdentifyPieceBorders(cMesh, p.IsGround);
                    cSclOffset = Vector3.Scale(vp.ScaleOffset, new Vector3(-1,1,1));
                    cVps.Add(
                        new VoxelPiece(
                            vp.Mesh, 
                            cBorders, 
                            vp.RotOfftset,
                            cSclOffset
                        )
                    );
                }
                vpsToAdd.AddRange(cVps);    
            }

            if (p.mirrorY)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = MirrorMesh(vp.Mesh, Axis.y);
                    cBorders = IdentifyPieceBorders(cMesh, p.IsGround);
                    cSclOffset = Vector3.Scale(vp.ScaleOffset, new Vector3(1,-1,1));
                    cVps.Add(
                        new VoxelPiece(
                            vp.Mesh, 
                            cBorders, 
                            vp.RotOfftset,
                            cSclOffset
                        )
                    );
                }
                vpsToAdd.AddRange(cVps);    
            }

            if (p.mirrorZ)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = MirrorMesh(vp.Mesh, Axis.z);
                    cBorders = IdentifyPieceBorders(cMesh, p.IsGround);
                    cSclOffset = Vector3.Scale(vp.ScaleOffset, new Vector3(1,1,-1));
                    cVps.Add(
                        new VoxelPiece(
                            vp.Mesh, 
                            cBorders, 
                            vp.RotOfftset,
                            cSclOffset
                        )
                    );
                }
                vpsToAdd.AddRange(cVps);    
            }

            if (p.rotateX)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = vp.Mesh;
                    // Rotation for 90, 180 and 270
                    for (int i = 0; i < 3; i++)
                    {
                        cMesh = RotateMeshNinetyDegrees(cMesh, Axis.x);
                        cBorders = IdentifyPieceBorders(cMesh, p.IsGround);
                        cRotOffset = new Vector3(90*(i+1), 0, 0) + vp.RotOfftset;
                        cVps.Add(
                            new VoxelPiece(
                                vp.Mesh, 
                                cBorders,  
                                cRotOffset,
                                vp.ScaleOffset
                            )
                        ); 
                    }
                }
                vpsToAdd.AddRange(cVps);    
            }
            
            if (p.rotateY)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = vp.Mesh;
                    // Rotation for 90, 180 and 270
                    for (int i = 0; i < 3; i++)
                    {
                       cMesh = RotateMeshNinetyDegrees(cMesh, Axis.y);
                        cBorders = IdentifyPieceBorders(cMesh, p.IsGround);
                        cRotOffset = new Vector3(0, 90*(i+1), 0) + vp.RotOfftset;
                        cVps.Add(
                            new VoxelPiece(
                                vp.Mesh, 
                                cBorders, 
                                cRotOffset,
                                vp.ScaleOffset
                            )
                        ); 
                    }
                }
                vpsToAdd.AddRange(cVps);    
            }

            if (p.rotateZ)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = vp.Mesh;
                    // Rotation for 90, 180 and 270
                    for (int i = 0; i < 3; i++)
                    {
                        cMesh = RotateMeshNinetyDegrees(cMesh, Axis.z);
                        cBorders = IdentifyPieceBorders(cMesh, p.IsGround);
                        cRotOffset = new Vector3(0, 0, 90*(i+1)) + vp.RotOfftset;
                        cVps.Add(
                            new VoxelPiece(
                                vp.Mesh, 
                                cBorders, 
                                cRotOffset,
                                vp.ScaleOffset
                            )
                        ); 
                    }
                }
                vpsToAdd.AddRange(cVps);    
            } 

            _voxelPieces.AddRange(vpsToAdd);
        }

        Debug.Log("N Pieces: " + _voxelPieces.Count);
        Debug.Log("N Borders: " + _borders.Count);       
    }

    /// <summary>
    /// Identifies the borders of a mesh using Unity's coord system as reference and the size of the voxels.
    /// </summary>
    /// <param name="piece">Mesh associated with the piece.</param>
    /// <returns>Array of Borders.</returns>
    private int[] IdentifyPieceBorders(Mesh piece, bool isGroudnd)
    {
        Border[] cBorders = new Border[6];
        int[] cIntBorder = new int[6];
        
        float[] maxSides = new float[6];
        
        if (_voxelSize < 0)
            foreach (var v in piece.vertices)
            {
                if (v.x > maxSides[(int)Side.front])
                    maxSides[(int)Side.front] = v.x;
                if (v.x < maxSides[(int)Side.back])
                    maxSides[(int)Side.back] = v.x;
                if (v.y > maxSides[(int)Side.up])
                    maxSides[(int)Side.up] = v.y;
                if (v.y < maxSides[(int)Side.down])
                    maxSides[(int)Side.down] = v.y;
                if (v.z > maxSides[(int)Side.right])
                    maxSides[(int)Side.right] = v.z;
                if (v.z < maxSides[(int)Side.left])
                    maxSides[(int)Side.left] = v.z;
            }
        else
        {
            maxSides[(int)Side.front] = _voxelSize/2;
            maxSides[(int)Side.back] = -_voxelSize/2;
            maxSides[(int)Side.up] = _voxelSize;
            maxSides[(int)Side.down] = 0;
            maxSides[(int)Side.right] = _voxelSize/2;
            maxSides[(int)Side.left] = -_voxelSize/2;
        }


        List<Vector3>[] verts = new List<Vector3>[6];

        for (int i = 0; i < 6; i++)
            verts[i] = new List<Vector3>();

        RoundVertices(piece);

        foreach (var v in piece.vertices)
        {
            // Front Side
            if (Mathf.Abs(v.x - maxSides[(int)Side.front]) < _threshold)
                verts[(int)Side.front].Add(new Vector3(0, v.y, v.z));
            // Back Side
            if (Mathf.Abs(v.x - maxSides[(int)Side.back]) < _threshold)
                verts[(int)Side.back].Add(new Vector3(0, v.y, v.z));
            // Up Side
            if (Mathf.Abs(v.y - maxSides[(int)Side.up]) < _threshold)
                verts[(int)Side.up].Add(new Vector3(v.x, 0, v.z));
            // Down Side
            if (Mathf.Abs(v.y - maxSides[(int)Side.down]) < _threshold)
                verts[(int)Side.down].Add(new Vector3(v.x, 0, v.z));
            // Right Side
            if (Mathf.Abs(v.z - maxSides[(int)Side.right]) < _threshold)
                verts[(int)Side.right].Add(new Vector3(v.x, v.y, 0));
            // Left Side
            if (Mathf.Abs(v.z - maxSides[(int)Side.left]) < _threshold)
                verts[(int)Side.left].Add(new Vector3(v.x, v.y, 0));
        }

        for (int i = 0; i < 6; i++)
        {
            if (isGroudnd && i == (int)Side.down)
            {
                cIntBorder[i] = 0;
                continue;
            }

            cBorders[i] = new Border(verts[i]);
            int isBrder = IsBorder(cBorders[i]); 

            if (isBrder < 0)
            {
                _borders.Add(cBorders[i]);
                cIntBorder[i] = _borders.Count-1; 
            }   
            else 
                cIntBorder[i] = isBrder;
        }

        return cIntBorder;
    }

    /// <summary>
    /// Check if a given border is already registered in the border list. 
    /// Returns the index of the border if it's registered. 
    /// </summary>
    /// <param name="b">Border to check if it's registered.</param>
    /// <returns>Int > 0 if registered. -1 otherwise.</returns>
    private int IsBorder(Border b)
    {
        for(int i = 0; i < _borders.Count; i++)
        {
            Border bdr = _borders[i];
            if (Border.Compare(bdr, b))
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Reduce the decimals of the a Meshes's vertices's components to 2. 
    /// </summary>
    /// <param name="m">Mesh to round its vertices.</param>
    private void RoundVertices(Mesh m)
    {
        List<Vector3> vertices = new List<Vector3>();
        m.GetVertices(vertices);

        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = new Vector3 (
                    (float) Math.Round(vertices[i].x,2),
                    (float) Math.Round(vertices[i].y,2),
                    (float) Math.Round(vertices[i].z,2)
                );        
        }

    }

    /// <summary>
    /// Mirrors a given mesh against a given axis.
    /// </summary>
    /// <param name="piece">Mesth to mirror.</param>
    /// <param name="axis">Axis to mirror against.</param>
    /// <returns>Mesh mirrored.</returns>
    private Mesh MirrorMesh(Mesh piece, Axis axis)
    {
        Mesh mirroredMesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        piece.GetVertices(vertices);
        for (int i = 0; i < vertices.Count; i++)
        {
            switch (axis)
            {
                case Axis.x:
                    vertices[i] = new Vector3(-vertices[i].x, vertices[i].y, vertices[i].z);
                break;
                case Axis.y:
                    vertices[i] = new Vector3(vertices[i].x, -vertices[i].y, vertices[i].z);
                break;
                case Axis.z:
                    vertices[i] = new Vector3(vertices[i].x, vertices[i].y, -vertices[i].z);
                break;
            }
        }

        mirroredMesh.SetVertices(vertices);
        return mirroredMesh;
    }

    /// <summary>
    /// Rotates a given mesh 90 degrees around a given axis.
    /// </summary>
    /// <param name="piece">Mesh to be rotated.</param>
    /// <param name="axis">Axis to rotate around.</param>
    /// <returns>Mesh rotated.</returns>
    private Mesh RotateMeshNinetyDegrees(Mesh piece, Axis axis)
    {
        Mesh rotatedMesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        piece.GetVertices(vertices);

        /// final step for a transformation matrix op 
        /// https://en.wikipedia.org/wiki/Rotation_matrix
        for (int i = 0; i < vertices.Count; i++)
        {
            switch (axis)
            {
                case Axis.x:
                    vertices[i] = new Vector3(vertices[i].x, -vertices[i].z, vertices[i].y);
                break;
                case Axis.y:
                    vertices[i] = new Vector3(vertices[i].z, vertices[i].y, -vertices[i].x);
                break;
                case Axis.z:
                    vertices[i] = new Vector3(-vertices[i].y, vertices[i].x, vertices[i].z);
                break;
            }

        }

        rotatedMesh.SetVertices(vertices);
        return rotatedMesh;
    }
}
