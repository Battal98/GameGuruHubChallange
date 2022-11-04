using System.Collections.Generic;
using UnityEngine;

namespace RunnerCutModule.Data
{
    [System.Serializable]
    public struct StackCubeData
    {
        public Vector2 MinMaxPushValueX;
        public float StackCubeSpeed;
        public Vector2 SpawnDotsX;
        public List<Color> CubeColors;
    } 
}
