using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelPieceIdentifier))]
public class VoxelPieceIdentifierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VoxelPieceIdentifier myTarget = (VoxelPieceIdentifier)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Update Pieces"))
            myTarget.IdentifyPieces();

    }
}