using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Component that given an array of meshes, identifies ther borders of each.
/// </summary>
[Serializable]
public class VoxelPieceInitializer
{
    private enum Axis{
        x,y,z
    }

    [SerializeField]
    private RawVoxelPiece[] _candidates = null;
    [SerializeField]
    private float _voxelSize = 1;

    private List<VoxelPiece> voxelPieces; 
    private List<Border> borders;

    /// <summary>
    /// Creates the voxel pieces based on the raw voxel pieces, doing any post processing necesarry to the pieces, as well as creating
    /// all the needed aditional data.
    /// </summary>
    public List<VoxelPiece> GetPieces()
    {
        voxelPieces = new List<VoxelPiece>();
        borders = new List<Border>();

        // Empty Space
        Mesh emptyMesh = new Mesh();
        voxelPieces.Add(
            new VoxelPiece(
                emptyMesh,
                IdentifyPieceBorders(emptyMesh),
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
            cBorders = IdentifyPieceBorders(p.mesh);
            vpsToAdd.Add(new VoxelPiece(p.mesh, cBorders, Vector3.zero, Vector3.one));
            
            if (p.mirrorX)
            {
                cVps.Clear();

                foreach (VoxelPiece vp in vpsToAdd)
                {
                    Mesh cMesh = MirrorMesh(vp.Mesh, Axis.x);
                    cBorders = IdentifyPieceBorders(cMesh);
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
                    cBorders = IdentifyPieceBorders(cMesh);
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
                    cBorders = IdentifyPieceBorders(cMesh);
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
                    // Rotation for 90, 180 and 270
                    for (int i = 0; i < 3; i++)
                    {
                       Mesh cMesh = RotateMeshNinetyDegrees(vp.Mesh, Axis.x);
                        cBorders = IdentifyPieceBorders(cMesh);
                        cRotOffset = new Vector3(90*i, 0, 0) + vp.RotOfftset;
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
                    // Rotation for 90, 180 and 270
                    for (int i = 0; i < 3; i++)
                    {
                       Mesh cMesh = RotateMeshNinetyDegrees(vp.Mesh, Axis.y);
                        cBorders = IdentifyPieceBorders(cMesh);
                        cRotOffset = new Vector3(0, 90*i, 0) + vp.RotOfftset;
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
                    // Rotation for 90, 180 and 270
                    for (int i = 0; i < 3; i++)
                    {
                       Mesh cMesh = RotateMeshNinetyDegrees(vp.Mesh, Axis.z);
                        cBorders = IdentifyPieceBorders(cMesh);
                        cRotOffset = new Vector3(0, 0, 90*i) + vp.RotOfftset;
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

            voxelPieces.AddRange(vpsToAdd);
        }

        Debug.Log("N Borders: " + borders.Count);
        return voxelPieces;           
    }

    /// <summary>
    /// Identifies the borders of a mesh using Unity's coord system as reference and the size of the voxels.
    /// </summary>
    /// <param name="piece">Mesh associated with the piece.</param>
    /// <returns>Array of Borders.</returns>
    private int[] IdentifyPieceBorders(Mesh piece)
    {
        Border[] cBorders = new Border[6];
        int[] cIntBorder = new int[6];
        List<Vector3>[] verts = new List<Vector3>[6];

        for (int i = 0; i < 6; i++)
            verts[i] = new List<Vector3>();


        foreach (var v in piece.vertices)
        {
            // Front Side
            if (v.x == _voxelSize/2)
                verts[(int)Side.front].Add(new Vector3(0, v.y, v.z));
            // Back Side
            if (v.x == -_voxelSize/2)
                verts[(int)Side.back].Add(new Vector3(0, v.y, v.z));
            // Up Side
            if (v.y == _voxelSize)
                verts[(int)Side.up].Add(new Vector3(v.x, 0, v.z));
            // Down Side
            if (v.y == 0)
                verts[(int)Side.down].Add(new Vector3(v.x, 0, v.z));
            // Right Side
            if (v.z == _voxelSize/2)
                verts[(int)Side.right].Add(new Vector3(v.x, v.y, 0));
            // Left Side
            if (v.z == -_voxelSize/2)
                verts[(int)Side.left].Add(new Vector3(v.x, v.y, 0));
        }

        for (int i = 0; i < 6; i++)
        {
            cBorders[i] = new Border(verts[i]);
            int isBrder = isBorder(cBorders[i]); 

            if (isBrder < 0)
            {
                borders.Add(cBorders[i]);
                cIntBorder[i] = borders.Count-1; 
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
    private int isBorder(Border b)
    {
        for(int i = 0; i < borders.Count; i++)
        {
            Border bdr = borders[i];
            if (Border.Compare(bdr, b))
                return i;
        }
        return -1;
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
