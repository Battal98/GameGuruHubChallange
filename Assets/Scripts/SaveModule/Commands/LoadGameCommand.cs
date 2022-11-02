using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveLoadModule.Interfaces;
using SaveLoadModule.Enums;

namespace SaveLoadModule.Command
{
    public class LoadGameCommand
    {
        public T Execute<T>(SaveLoadType key,
            int uniqueId) where T : ISavable
        {
            var _path = key.ToString() + uniqueId + ".es3";
            if (ES3.FileExists(_path))
            {
                if (ES3.KeyExists(key.ToString(),
                        _path))
                {
                    var objectToReturn = ES3.Load<T>(key.ToString(),
                        _path);
                    return objectToReturn;
                }

                return default;
            }

            return default;
        }
    } 
}
