using UnityEngine;

namespace ScoreModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Score", menuName = "GameName/CD_Score", order = 0)]
    public class CD_Score : ScriptableObject
    {
        public ScoreData ScoreData;
    } 
}
