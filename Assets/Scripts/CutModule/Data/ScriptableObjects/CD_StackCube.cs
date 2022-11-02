using System.Collections.Generic;
using UnityEngine;

namespace CutModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_StackCube", menuName = "StackGame/CD_StackCube", order = 0)]
    public class CD_StackCube : ScriptableObject
    {
        public StackCubeData StackCubeData;
        public List<int> StackCountsEachLevel;
    } 
}
