using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Player", menuName = "StackGame/CD_Player", order = 0)]
    public class CD_Player: ScriptableObject
    {
        public PlayerData PlayerData;
    }

}