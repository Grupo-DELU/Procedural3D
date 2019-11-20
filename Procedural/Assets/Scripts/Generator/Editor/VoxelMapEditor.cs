using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoxelMap))]
public class VoxelMapEditor : Editor
{
    private SerializedProperty _voxelPieceIdentifier ;
    private SerializedProperty _generationType;
    private SerializedProperty _visualizationType;

    // Generation variables
    private SerializedProperty _groundPiece;
    private SerializedProperty _sizeX, _sizeY, _sizeZ; 
    
    // Visualiztion Variables
    private SerializedProperty _dfltMat = null;

    private void InitInspector()
    {
        _voxelPieceIdentifier = serializedObject.FindProperty("_voxelPieceIdentifier");
        _generationType = serializedObject.FindProperty("_generationType");
        _visualizationType = serializedObject.FindProperty("_visualizationType");

        _groundPiece = serializedObject.FindProperty("_groundPiece");
        _sizeX = serializedObject.FindProperty("_sizeX");
        _sizeY = serializedObject.FindProperty("_sizeY");
        _sizeZ = serializedObject.FindProperty("_sizeZ");

        _dfltMat = serializedObject.FindProperty("_dfltMat");
    }

    public void OnEnable()
    {
        InitInspector();
    }

    public override void OnInspectorGUI()
    {
        VoxelMap myTarget = (VoxelMap)target;

        serializedObject.Update();

        EditorGUILayout.LabelField("Basic Settings");
        EditorGUILayout.PropertyField(_voxelPieceIdentifier);
        
        EditorGUILayout.PropertyField(_generationType);

        switch (_generationType.intValue)
        {
            case (int)Generator.Type.WFC:
                EditorGUILayout.PropertyField(_groundPiece);
                EditorGUILayout.PropertyField(_sizeX);
                EditorGUILayout.PropertyField(_sizeY);
                EditorGUILayout.PropertyField(_sizeZ);
            break;
        }
        
        EditorGUILayout.PropertyField(_visualizationType);

        switch (_visualizationType.intValue)
        {
            case (int)MapVisualization.Type.Mesh:
                EditorGUILayout.PropertyField(_dfltMat);
            break;
        }


        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Generate"))
        {
            myTarget.Initialize();
            myTarget.Generate();
        }

        if (GUILayout.Button("Update"))
            myTarget.Update();

        if (GUILayout.Button("Clear"))
            myTarget.Clear();

    }
}