using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public abstract class MapRepresentation : MonoBehaviour
{
    [SerializeField]
    private Generator _generator = null;

    protected int[,,] _map = null;
    protected List<VoxelPiece> _pieces = null;

    public bool LetsGetCreative = false;

    public abstract void CreateGO();

    void Update ()
    {
        if (LetsGetCreative)
        {
            LetsGetCreative = false;
            if (!_generator)
            {
                Debug.Log("No Generator provided.");
                return;
            }

            _map = _generator.Map;
            _pieces = _generator.Pieces;
            CreateGO();
        }
    }

}