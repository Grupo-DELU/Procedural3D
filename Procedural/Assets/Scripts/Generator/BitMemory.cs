using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Data structure to store boolean data in the bits of an array of uint.abstract Dynamic programming memory.
/// </summary>
public class BitMemory
{
    private uint[] _bits = null;
    private int _size = 0;
    private List<int> _validIndexes = null;

    public BitMemory(int size)
    {
        _size = size;
        float nArrays = size/32 + 1;
        _bits = new uint[(int)Math.Ceiling(nArrays)];
    }

    /// <summary>
    /// Indexer to modify the individual bits.
    /// </summary>
    /// <value></value>
    public bool this[int index] {
        set {
            if (value)
               _bits[index/32] |=  ((uint)1<<(index%32));
            else
                _bits[index/32] &=  ~((uint)1<<(index%32));
            CalculateValidIndex();
        }
        get {
            uint cInt =_bits[index/32];
            return (0 != (cInt & (1<<(index%32))));
        }
    }

    /// <summary>
    /// Get a List with all indexes that are true.
    /// </summary>
    public List<int> ValidIndexes
    {
        get {
            if (_validIndexes == null)
                _validIndexes = new List<int>();
            return _validIndexes;
        }
    }

    /// <summary>
    /// Returns the number of valid indexes.
    /// </summary>
    public int nValidIndexes 
    {
        get {
            if (_validIndexes == null)
                _validIndexes = new List<int>();
            return _validIndexes.Count;
        }
    }

    /// <summary>
    /// Creates a List with all indexes that are true and stores it in _validIndexes.
    /// </summary>
    private void CalculateValidIndex()
    {
        _validIndexes = new List<int>();

        for (int i = 0; i < _size; i++)
        {
            if (this[i])
                _validIndexes.Add(i);
        }
    }

    /// <summary>
    /// Returns a string of the bits.
    /// </summary>
    /// <returns>String of the bits.</returns>
    public override string ToString()
    {
        string str = "";

        foreach(var ui in _bits)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((1<<i & ui) != 0)
                {
                    str = "1" + str;
                }
                else
                    str = "0" + str;
            }
        }

        return str;
    }

}