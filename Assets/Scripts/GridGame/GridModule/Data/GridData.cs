using UnityEngine;
using System;

[Serializable]
public struct GridData 
{
    public int GridSize;
    [Tooltip("'y' Value is offset to z \n 'x' Value is offset to x")]
    public Vector2 GridOffsets;
}
