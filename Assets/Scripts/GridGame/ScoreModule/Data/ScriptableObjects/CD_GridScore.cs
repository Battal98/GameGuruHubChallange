using UnityEngine;

namespace GridGame.ScoreModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_GridScore", menuName = "GridGame/CD_GridScore", order = 0)]
    public class CD_GridScore : ScriptableObject
    {
        public GridScoreData GridScoreData;
    } 
}
