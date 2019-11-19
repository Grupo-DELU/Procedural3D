using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public abstract class MapRepresentation : MonoBehaviour
{
    [SerializeField]
    protected Generator _generator = null;

    protected int[,,] _map = null;
    protected List<VoxelPiece> _pieces = null;

    public bool LetsGetCreative = false;
    public bool LetsAgreeNotBeCreativeAgaian = false;


    public abstract void CreateGO();

    protected void Clean()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
        
    }

    public virtual void Update ()
    {
        if (LetsGetCreative)
        {
            LetsGetCreative = false;
            if (!_generator)
            {
                Debug.Log("No Generator provided.");
                return;
            }

            _generator.Generate();
            _map = _generator.Map;
            _pieces = _generator.Pieces;
            CreateGO();
        }

        if (LetsAgreeNotBeCreativeAgaian)
        {
            LetsAgreeNotBeCreativeAgaian = false;
            Clean();
        }
    }

}