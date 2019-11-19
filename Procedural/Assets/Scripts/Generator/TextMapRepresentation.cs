using UnityEngine;
public class TextMapRepresentation : MapRepresentation
{
    public override void CreateGO()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < _map.GetLength(0); i++)
        {
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                for (int k = 0; k < _map.GetLength(2); k++)
                {
                    if (_map[i,j,k] < 1)
                        continue;

                    VoxelPiece cPiece = _pieces[_map[i,j,k]];
                    GameObject go = new GameObject(i + "|" + j + "|" + k);
                    go.transform.parent = transform;
                    go.transform.localPosition = new Vector3(i,j,k);
                    go.transform.localScale = Vector3.one * .2f;
                    go.AddComponent<TextMesh>().text = _map[i,j,k].ToString();

                    for (int side = 0; side < 6; side++)
                    {
                        GameObject gos = new GameObject(i + "|" + j + "|" + k + "|Side" + side);
                        gos.transform.parent = go.transform;
                        gos.transform.localScale = Vector3.one*.5f;
                        gos.AddComponent<TextMesh>().text = (_pieces[_map[i,j,k]][side]).ToString();
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