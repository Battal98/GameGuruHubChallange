using UnityEngine;

namespace RunnerScoreModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Score", menuName = "StackGame/CD_Score", order = 0)]
    public class CD_Score : ScriptableObject
    {
        public ScoreData ScoreData;
    } 
}
