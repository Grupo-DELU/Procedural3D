using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[ExecuteInEditMode]
public class Generator : MonoBehaviour
{

    [SerializeField]
    private VoxelPieceInitializer _vpi = null;
    [SerializeField]
    private int _groundPiece = 0;
    [SerializeField]
    private int _sizeX = 4, _sizeY = 4, _sizeZ = 4; 

    public bool Execute = false;

    private List<VoxelPiece> _pieces;
    private int[,,] _map;
    BitMemory[,,] _voxelPosibilites = null;
    private bool _isGenerated = false;

    /// <summary>
    /// Gets the last map generated.
    /// </summary>
    /// <value></value>
    public int[,,] Map
    {
        get {
            if (!_isGenerated)
                Generate();

            return _map;
        }
    }

    /// <summary>
    /// Gets the pieces used to build the map.
    /// </summary>
    public List<VoxelPiece> Pieces
    {
        get {
            return _pieces;
        }
    }

    public void Generate() 
    {
        if (_sizeX < 2)
            Debug.Log("X size has to be greater than 2.");
        else if (_sizeY < 1)
            Debug.Log("Y size has to be greater than 2");
        else if (_sizeZ < 2)
            Debug.Log("Z size has to be greater than 2");
        else
        {
            _pieces = _vpi.GetPieces();
            InitMap();

            _voxelPosibilites = new BitMemory[_sizeX+1,_sizeY+1,_sizeZ+1];

            // init DP memory
            for (int i = 0; i < _sizeX+1; i++)
                for (int j = 0; j < _sizeY+1; j++)
                    for (int k = 0; k < _sizeZ+1; k++)
                        _voxelPosibilites[i,j,k] = new BitMemory(_pieces.Count);

            // start generation
            
            for (int j = 0; j < _sizeY+1; j++)
            {
                for (int k = 1; k < _sizeZ+1; k++)
                {
                    for (int i = 1; i < _sizeX+1; i++)
                    {
                        if (_map[i,j,k] > 0)
                            continue;

                        EvaluateTilePossibilities(i,j,k);
                        List<int> possibilities = _voxelPosibilites[i,j,k].GetValidIndex();
                        
                        if (possibilities.Count == 0)
                        {
                            Debug.Log("Oops");

                        }
                        else
                        {
                            _map[i,j,k] = possibilities[Random.Range(0, possibilities.Count)];
                        }
                        
                    }
                }     
            }
            Debug.Log("N Pieces: " + _pieces.Count);
            _isGenerated = true;
        }
    }

    /// <summary>
    /// Given the voxel coordinate, evaluates the possible pieces it can has.
    /// </summary>
    /// <param name="x">X coord</param>
    /// <param name="y">Y coord</param>
    /// <param name="z">Z Coord</param>
    private void EvaluateTilePossibilities(int x, int y, int z)
    {
        int[] _sideConstraint = new int[6];

        // If side has -1, then it has no constraints
        for (int i = 0; i < 6; i++)
            _sideConstraint[i] = -1;

        // Back constraint
        if (_map[x-1,y,z] > 0 && _map[x-1,y,z] < _pieces.Count)
            _sideConstraint[(int)Side.back] = _pieces[_map[x-1,y,z]][(int)Side.front];

        // Front constraint
        if (_map[x+1,y,z] > 0 && _map[x+1,y,z] < _pieces.Count)
            _sideConstraint[(int)Side.front] = _pieces[_map[x+1,y,z]][(int)Side.back];

        // Down constraint
        if (y > 0 && _map[x,y-1,z] > 0 && _map[x,y-1,z] < _pieces.Count)
            _sideConstraint[(int)Side.down] = _pieces[_map[x,y-1,z]][(int)Side.up];

        // Up constraint
        if (_map[x,y+1,z] > 0 && _map[x,y+1,z] < _pieces.Count)
            _sideConstraint[(int)Side.up] = _pieces[_map[x,y+1,z]][(int)Side.down];

        // Left constraint
        if (_map[x,y,z-1] > 0 && _map[x,y,z-1] < _pieces.Count )
            _sideConstraint[(int)Side.left] = _pieces[_map[x,y,z-1]][(int)Side.right];

        // Right constraint
        if (_map[x,y,z+1] > 0 && _map[x,y,z+1] < _pieces.Count)
            _sideConstraint[(int)Side.right] = _pieces[_map[x,y,z+1]][(int)Side.left];

        Debug.Log(x + "|" + y + "|" + z + "|");
        Debug.Log("Front: " + _sideConstraint[(int)Side.front]);
        Debug.Log("Back: " + _sideConstraint[(int)Side.back]);
        Debug.Log("Up: " + _sideConstraint[(int)Side.up]);
        Debug.Log("Down: " + _sideConstraint[(int)Side.down]);
        Debug.Log("Left: " + _sideConstraint[(int)Side.left]);
        Debug.Log("Right: " + _sideConstraint[(int)Side.right]);

        
        for (int i = 0; i < _pieces.Count; i++)
        {
            bool validPiece = true;
            int freeSides = 6;
            for (int j = 0; j < 6; j++)
            {
                if (_sideConstraint[j] < 1 )
                    continue;

                freeSides--;
                
                if (_sideConstraint[j] != _pieces[i][j])
                {
                    validPiece = false;
                    break;
                }
            }

            if (freeSides == 6)
                validPiece = false;

            _voxelPosibilites[x,y,z][i] = validPiece;
        }

        Debug.Log( _voxelPosibilites[x,y,z].ToString());

    }

    /// <summary>
    /// Initialize the map array with the correct dimensions and assign the borders of the cube.
    /// </summary>
    private void InitMap()
    {
        _map = new int[_sizeX+2,_sizeY+2,_sizeZ+2];

        // Build "Cage"
        for (int i = 1; i < _sizeY + 2; i++)
        {
            for (int j = 0; j < _sizeX + 2; j++)
            {
                _map[j,i, 0] = 0;
                _map[j,i,_sizeZ+1] = 0;
            }  

            for (int j = 0; j < _sizeZ + 2; j++)
            {
                _map[0,i,j] = 0;
                _map[_sizeX+1,i,j] = 0;
            }    
        }

        for (int i = 0; i < _sizeX + 2; i++)
        {
            for (int j = 0; i < _sizeZ + 2; i++)
            {
                _map[i,_sizeY+1, j] = 0;
            }  
        }

        // Build Bottom/ Layer Cage
        for (int i = 0; i < _sizeX+2; i++)
        {
            _map[i,0,0] = _groundPiece;
            _map[i,0,_sizeZ+1] = _groundPiece;
        }

        for (int i = 0; i < _sizeZ+2; i++)
        {
            _map[0,0,i] = _groundPiece;
            _map[_sizeX+1,0,i] = _groundPiece;
        }

        // Init rest of map with -1.
        for (int i = 1; i < _sizeX+1; i++)
        {
            for (int j = 1; j < _sizeY+1; j++)
            {
                for (int k = 1; k < _sizeZ+1; k++)
                {
                    _map[i,j,k] = -1;
                }
            }   
        }
    }

    
    
    void Update()
    {
        if (Execute)
        {
            Execute = false;
            Generate();
        }
    }

}