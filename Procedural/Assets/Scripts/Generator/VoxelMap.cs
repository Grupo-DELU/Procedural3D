using UnityEngine;


/// <summary>
/// Class that represents a voxel map component. 
/// </summary>
public class VoxelMap : MonoBehaviour
{

    [SerializeField]
    private VoxelPieceIdentifier _voxelPieceIdentifier = null;
    [SerializeField]
    private Generator.Type _generationType = Generator.Type.WFC;
    [SerializeField]
    private MapVisualization.Type _visualizationType = MapVisualization.Type.Mesh;

    // Generation variables
    [SerializeField]
    private int _groundPiece = 0;
    [SerializeField]
    private int _sizeX = 4, _sizeY = 4, _sizeZ = 4; 
    // Visualiztion Variables
    [SerializeField]
    private Material _dfltMat = null;

    private MapVisualization _visualization;
    private Generator _generator;
    private int[,,] _map = null;

    public void Initialize()
    {
        if (!_voxelPieceIdentifier)
            return;

        switch (_generationType)
        {
            case (Generator.Type.WFC):
                _generator = new WFCGenerator(
                                        _voxelPieceIdentifier.Pieces,
                                        _sizeX,
                                        _sizeY,
                                        _sizeZ,
                                        _groundPiece
                                    );
            break;
            case (Generator.Type.VPPreview):
                _generator = new VPPreviewGenerator(_voxelPieceIdentifier.Pieces);
            break;
        }

        switch (_visualizationType)
        {
            case (MapVisualization.Type.Mesh):
                _visualization = new MeshMapVisualization(transform, _dfltMat);
            break;
            case (MapVisualization.Type.Text):
                _visualization = new TextMapVisualization(transform);
            break;
        }
    }

    public void Generate()
    {
        _generator.CreateMap(_voxelPieceIdentifier.Pieces);
        _map = _generator.Map;
        _visualization.CreateGO(_map, _voxelPieceIdentifier.Pieces);
    }

    public void Clear()
    {
        if (_map == null)
            return;

        _visualization.Clean();
        _map = null;
    }

    public void Update()
    {
        if (_map == null)
            return;

        switch (_visualizationType)
        {
            case (MapVisualization.Type.Mesh):
                _visualization = new MeshMapVisualization(transform, _dfltMat);
            break;
            case (MapVisualization.Type.Text):
                _visualization = new TextMapVisualization(transform);
            break;
        }

        _visualization.CreateGO(_map, _voxelPieceIdentifier.Pieces);
    }

}