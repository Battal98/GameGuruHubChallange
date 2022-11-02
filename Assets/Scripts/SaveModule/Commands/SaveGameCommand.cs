using SaveLoadModule.Interfaces;
using UnityEngine;

namespace SaveLoadModule.Command
{
    public class SaveGameCommand
    {
        public void Execute<T>(T _dataToSave,
            int _uniqueID) where T : ISavable
        {
            var _path = _dataToSave.GetKey().ToString() + _uniqueID + ".es3";

            var _dataKey = _dataToSave.GetKey().ToString();

            if (!ES3.FileExists(_path))
                ES3.Save(_dataKey,
                    _dataToSave,
                    _path);

            ES3.Save(_dataKey,
                _dataToSave,
                _path);
        }
    } 
}
