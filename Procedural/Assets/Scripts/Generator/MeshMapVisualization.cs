using UnityEngine;
using System.Collections.Generic;
public class MeshMapVisualization : MapVisualization
{
    private Material _dfltMat = null;
    public MeshMapVisualization(Transform rootGo, Material dfltMat)
    {
        _rootGO = rootGo;
        _dfltMat = dfltMat;
    }

    /// <summary>
    /// Creates the GO associated with the given map a pieces.
    /// </summary>
    /// <param name="map">Map to represent.</param>
    /// <param name="pieces">Pieces to use to build the map.</param>
    public override void CreateGO(int[,,] map, List<VoxelPiece> pieces)
    {
        Clean();

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                for (int k = 0; k < map.GetLength(2); k++)
                {
                    if (map[i,j,k] < 1)
                        continue;

                    VoxelPiece cPiece = pieces[map[i,j,k]];
                    GameObject go = new GameObject(i + "|" + j + "|" + k);
                    go.transform.parent = _rootGO;
                    go.transform.localPosition = new Vector3(i,j,k);
                    go.transform.localEulerAngles = cPiece.RotOfftset;
                    go.transform.localScale = cPiece.ScaleOffset;
                    go.AddComponent<MeshFilter>().mesh = cPiece.Mesh;
                    go.AddComponent<MeshRenderer>().material = _dfltMat;
                }
            }
        }
    }
}