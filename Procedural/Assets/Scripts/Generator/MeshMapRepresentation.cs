using UnityEngine;
public class MeshMapRepresentation : MapRepresentation
{
    [SerializeField]
    private Material _baseMaterial = null;

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
                    go.transform.localEulerAngles = cPiece.RotOfftset;
                    go.transform.localScale = cPiece.ScaleOffset;
                    go.AddComponent<MeshFilter>().mesh = cPiece.Mesh;
                    go.AddComponent<MeshRenderer>().material = _baseMaterial;
                }
            }
        }
    }
}