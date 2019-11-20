using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates a visual representation of a given map with text meshes.
/// </summary>
public class TextMapVisualization : MapVisualization
{
    public TextMapVisualization(Transform rootGo)
    {
        _rootGO = rootGo;
    }

    /// <summary>
    /// Creates a text visualization GO for a given map and pieces.
    /// </summary>
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
                    go.transform.localPosition = new Vector3(i,j+.5f,k);
                    go.transform.localScale = Vector3.one * .2f;
                    go.AddComponent<TextMesh>().text = map[i,j,k].ToString();

                    // Borders visualizaion
                    for (int side = 0; side < 6; side++)
                    {
                        GameObject gos = new GameObject(i + "|" + j + "|" + k + "|Side" + side);
                        gos.transform.parent = go.transform;
                        gos.transform.localScale = Vector3.one*.5f;
                        gos.AddComponent<TextMesh>().text = (pieces[map[i,j,k]][side]).ToString();
                        gos.GetComponent<TextMesh>().color = Color.red;
                        switch (side)
                        {
                            case 0:
                                gos.transform.localPosition = new Vector3(2.25f,0,0);
                            break;
                            case 1:
                                gos.transform.localPosition = new Vector3(0,0,2.25f);
                            break;
                            case 2:
                                gos.transform.localPosition = new Vector3(-2.25f,0,0);
                            break;
                            case 3:
                                gos.transform.localPosition = new Vector3(0,0,-2.25f);
                            break;
                            case 4:
                                gos.transform.localPosition = new Vector3(0,2.25f,0);
                            break;
                            case 5:
                                gos.transform.localPosition = new Vector3(0,-2.25f,0);
                            break;
                        }
                    }
                }
            }
        }
    }
}