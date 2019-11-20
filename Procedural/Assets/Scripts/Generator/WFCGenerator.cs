using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WFCGenerator : Generator
{
    private struct VoxelCoord
    {
        public int x, y, z;
    }

    private int _groundPiece = 0;
    private int _sizeX = 4, _sizeY = 4, _sizeZ = 4; 
    private BitMemory[,,] _voxelPosibilites = null;

    public WFCGenerator (List<VoxelPiece> pieces, int sizeX, int sizeY, int sizeZ, int groundPiece)
    {
        _pieces = pieces;
        _sizeX = sizeX;
        _sizeY = sizeY;
        _sizeZ = sizeZ;
        _groundPiece = groundPiece;
    }

    /// <summary>
    /// Returns true if the preconditions are all met;
    /// </summary>
    /// <returns></returns>
    protected override bool CheckPrecondition()
    {
        if (!base.CheckPrecondition())
            return false;
        if (_sizeX < 2)
        {
            Debug.Log("X size has to be greater than 2.");
            return false;    
        }
        if (_sizeY < 1)
        {
            Debug.Log("Y size has to be greater than 2");
            return false;
        }
        if (_sizeZ < 2)
        {
            Debug.Log("Z size has to be greater than 2");
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// Initialize the map array with the correct dimensions and assign the borders of the cube.
    /// </summary>
    protected override void InitMap()
    {
        // Add 2 extra row/columns to build a cage around the generating space
        _map = new int[_sizeX+2,_sizeY+2,_sizeZ+2];

        // The 0 piece is an empty voxel
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
            for (int j = 0; j < _sizeY+1; j++)
            {
                for (int k = 1; k < _sizeZ+1; k++)
                {
                    _map[i,j,k] = -1;
                }
            }   
        }
    }

    /// <summary>
    /// Function to sort observable voxels.null It compares the posibilities of each voxel.
    /// </summary>
    private int CompareObservableVoxels(VoxelCoord x, VoxelCoord y)
    {
        if (_voxelPosibilites[x.x, x.y, x.z].nValidIndexes == _voxelPosibilites[y.x, y.y, y.z].nValidIndexes)
            return 0;

        if (_voxelPosibilites[x.x, x.y, x.z].nValidIndexes > _voxelPosibilites[y.x, y.y, y.z].nValidIndexes)
            return 1;

        return -1;
    }

    /// <summary>
    /// Generates a map based on the Wave Function Collapse Algotithm
    /// https://github.com/mxgmn/WaveFunctionCollapse
    /// </summary>
    protected override void Generate() 
    {
        // Build propagator

        _voxelPosibilites = new BitMemory[_sizeX+1,_sizeY+1,_sizeZ+1];

        // init DP memory
        for (int i = 0; i < _sizeX+1; i++)
            for (int j = 0; j < _sizeY+1; j++)
                for (int k = 0; k < _sizeZ+1; k++)
                    _voxelPosibilites[i,j,k] = new BitMemory(_pieces.Count);

        

        // start generation, all not-assigned spaces have the value -1
        List<VoxelCoord> observableVoxels = new List<VoxelCoord>();

        for (int j = 0; j < _sizeY+1; j++)
            for (int k = 1; k < _sizeZ+1; k++)
                for (int i = 1; i < _sizeX+1; i++)
                {
                    EvaluateTilePossibilities(i,j,k);
                    VoxelCoord vc = new VoxelCoord();
                    vc.x = i;
                    vc.y = j;
                    vc.z = k;
                    observableVoxels.Add(vc);
                }

        observableVoxels.Sort(CompareObservableVoxels);

        bool isContradiction;
        // do
        // {
            isContradiction = false;

            while (observableVoxels.Count > 0)
            {
                // Lowest entropy voxel
                VoxelCoord cVx = observableVoxels[0];

                if (_voxelPosibilites[cVx.x, cVx.y, cVx.z].nValidIndexes == 0)
                {
                    Debug.Log("Oops");
                    Debug.Log(cVx.x +"|"+ cVx.y +"|"+ cVx.z);
                    isContradiction = true;
                    break;
                }
                else
                {
                    Debug.Log("Non-Oops");
                    // Choose random possibility
                    Map[cVx.x, cVx.y, cVx.z] = Random.Range(0, _voxelPosibilites[cVx.x, cVx.y, cVx.z].nValidIndexes);
                    Map[cVx.x, cVx.y, cVx.z] = _voxelPosibilites[cVx.x, cVx.y, cVx.z].ValidIndexes[Map[cVx.x, cVx.y, cVx.z]];
                    
                    // Update neighboards entropy
                    observableVoxels.RemoveAt(0);
                    if (cVx.x < _sizeX)
                        EvaluateTilePossibilities(cVx.x+1,cVx.y,cVx.z);
                    if (cVx.x > 1)
                        EvaluateTilePossibilities(cVx.x-1,cVx.y,cVx.z);
                    if (cVx.y < _sizeY)
                        EvaluateTilePossibilities(cVx.x,cVx.y+1,cVx.z);
                    if (cVx.y > 0)
                        EvaluateTilePossibilities(cVx.x,cVx.y-1,cVx.z);
                    if (cVx.z < _sizeZ)
                        EvaluateTilePossibilities(cVx.x,cVx.y,cVx.z+1);
                    if (cVx.z > 1)
                        EvaluateTilePossibilities(cVx.x,cVx.y,cVx.z-1);

                    observableVoxels.Sort(CompareObservableVoxels);
                }
            }
        // } while (isContradiction);
        
        // for (int j = 0; j < _sizeY+1; j++)
        // {
        //     for (int k = 1; k < _sizeZ+1; k++)
        //     {
        //         for (int i = 1; i < _sizeX+1; i++)
        //         {
        //             if (_map[i,j,k] > 0)
        //                 continue;

        //             EvaluateTilePossibilities(i,j,k);
        //             List<int> possibilities = _voxelPosibilites[i,j,k].ValidIndexes;
                    
        //             if (possibilities.Count == 0)
        //             {
        //                 Debug.Log("Oops");

        //             }
        //             else
        //             {
        //                 _map[i,j,k] = possibilities[Random.Range(0, possibilities.Count)];
        //             }
                    
        //         }
        //     }     
        // }
    }

    /// <summary>
    /// Given a voxel coordinate, evaluates the possible pieces it can has.
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
        if (_map[x-1,y,z] >= 0 && _map[x-1,y,z] < _pieces.Count)
            _sideConstraint[(int)Side.back] = _pieces[_map[x-1,y,z]][(int)Side.front];

        // Front constraint
        if (_map[x+1,y,z] >= 0 && _map[x+1,y,z] < _pieces.Count)
            _sideConstraint[(int)Side.front] = _pieces[_map[x+1,y,z]][(int)Side.back];

        // Down constraint
        if (y > 0 && _map[x,y-1,z] >= 0 && _map[x,y-1,z] < _pieces.Count)
            _sideConstraint[(int)Side.down] = _pieces[_map[x,y-1,z]][(int)Side.up];

        // Up constraint
        if (_map[x,y+1,z] >= 0 && _map[x,y+1,z] < _pieces.Count)
            _sideConstraint[(int)Side.up] = _pieces[_map[x,y+1,z]][(int)Side.down];

        // Left constraint
        if (_map[x,y,z-1] >= 0 && _map[x,y,z-1] < _pieces.Count )
            _sideConstraint[(int)Side.left] = _pieces[_map[x,y,z-1]][(int)Side.right];

        // Right constraint
        if (_map[x,y,z+1] >= 0 && _map[x,y,z+1] < _pieces.Count)
            _sideConstraint[(int)Side.right] = _pieces[_map[x,y,z+1]][(int)Side.left];

        // Debug.Log(x + "|" + y + "|" + z + "|");
        // Debug.Log("Front: " + _sideConstraint[(int)Side.front]);
        // Debug.Log("Back: " + _sideConstraint[(int)Side.back]);
        // Debug.Log("Up: " + _sideConstraint[(int)Side.up]);
        // Debug.Log("Down: " + _sideConstraint[(int)Side.down]);
        // Debug.Log("Left: " + _sideConstraint[(int)Side.left]);
        // Debug.Log("Right: " + _sideConstraint[(int)Side.right]);

        
        for (int i = 0; i < _pieces.Count; i++)
        {
            bool validPiece = true;
            for (int j = 0; j < 6; j++)
            {
                if (_sideConstraint[j] < 0 )
                    continue;

                
                if (_sideConstraint[j] != _pieces[i][j])
                {
                    validPiece = false;
                    break;
                }
            }

  

            _voxelPosibilites[x,y,z][i] = validPiece;
        }

        // Debug.Log( _voxelPosibilites[x,y,z].ToString());

    }

}