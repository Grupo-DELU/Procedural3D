using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class BitMemory
{
    private uint[] _bits;
    private int _size;

    public BitMemory(int size)
    {
        _size = size;
        float nArrays = size/32 + 1;
        _bits = new uint[(int)Math.Ceiling(nArrays)];
    }

    public bool this[int index] {
        set {
            if (value)
               _bits[index/32] |=  ((uint)1<<(index%32));
            else
                _bits[index/32] &=  ~((uint)1<<(index%32));
        }
        get {
            uint cInt =_bits[index/32];
            return (0 == (cInt & 1<<(index%32)));
        }
    }

    /// <summary>
    /// Returns a List with all indexes that are true.
    /// </summary>
    /// <returns>List of indexes that holds a true value.</returns>
    public List<int> GetValidIndex()
    {
        List<int> vIndxs = new List<int>();

        for (int i = 0; i < _size; i++)
        {
            if (this[i])
                vIndxs.Add(i);
        }

        return vIndxs;
    }

}