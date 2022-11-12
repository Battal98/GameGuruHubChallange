using UnityEngine;
using RunnerCutModule;
using System.Collections.Generic;
using PoolModule.Signals;
using PoolModule.Interfaces;
using PoolModule.Enums;
using RunnerAudioModule.Signals;
using RunnerAudioModule.Enums;
using DG.Tweening;

namespace RunnerCutModule.Controllers
{
    public class StackCubeCutController : MonoBehaviour, IGetPoolObject
    {
        [SerializeField]
        private StackCubeSpawnerManager stackCubeSpawnerManager;

        private float pitchValue = 1f;
        private int comboValue;

        public void CutObject(List<GameObject> _stackCubes)
        {
            float edge = stackCubeSpawnerManager.GetCutEdge();
            float direction = edge > 0 ? 1f : -1f;

            float stackCubeXSize = _stackCubes[_stackCubes.Count - 2].transform.localScale.x - Mathf.Abs(edge);
            float cuttedCubeSize = _stackCubes[_stackCubes.Count - 1].transform.localScale.x - stackCubeXSize;

            float stackCubeXPosition = _stackCubes[_stackCubes.Count - 2].transform.position.x + (edge / 2);

            float cuttedCubeEdge = _stackCubes[_stackCubes.Count - 1].transform.position.x + (stackCubeXSize / 2 * direction);
            float cuttedCubeXPosition = cuttedCubeEdge + cuttedCubeSize / 2f * direction;

            CheckStackCube(edge, stackCubeXSize, stackCubeXPosition, cuttedCubeXPosition, cuttedCubeSize , _stackCubes, direction);
        }

        private void CheckStackCube(float edge, float stackCubeXSize, float stackCubeXPosition, float cuttedCubeXPosition, float cuttedCubeSize, List<GameObject> _stackCubes, float direction)
        {
            if (Mathf.Abs(edge) <= 0.1f)
            {
                comboValue++;
                AudioSignals.Instance.onPlaySound(SoundType.Correct, pitchValue);
                if (pitchValue <= 2f)
                    pitchValue += 0.1f;
            }
            else
            {
                pitchValue = 1f;
                if (comboValue > 3)
                {
                    AudioSignals.Instance.onPlaySound(SoundType.Incorrect, pitchValue);
                }
                comboValue = 0;
                if (stackCubeXSize>0)
                {
                    _stackCubes[_stackCubes.Count - 1].transform.localScale = new Vector3(stackCubeXSize,
                        _stackCubes[_stackCubes.Count - 1].transform.localScale.y,
                        _stackCubes[_stackCubes.Count - 1].transform.localScale.z);

                    _stackCubes[_stackCubes.Count - 1].transform.position = new Vector3(stackCubeXPosition,
                        _stackCubes[_stackCubes.Count - 1].transform.position.y,
                        _stackCubes[_stackCubes.Count - 1].transform.position.z);
                    SpawnCuttedCube(cuttedCubeXPosition, cuttedCubeSize, _stackCubes, direction);
                }

            }
        }
        private void SpawnCuttedCube(float cuttedCubeXPosition, float cuttedCubeSize, List<GameObject> _stackCubes, float direction)
        {
            var cuttedObj = GetObject(PoolType.CuttedCubes);

            cuttedObj.transform.localScale = new Vector3(cuttedCubeSize,
                _stackCubes[_stackCubes.Count - 1].transform.localScale.y,
                _stackCubes[_stackCubes.Count - 1].transform.localScale.z);
            cuttedObj.transform.position = new Vector3(cuttedCubeXPosition,
                _stackCubes[_stackCubes.Count - 1].transform.position.y,
                _stackCubes[_stackCubes.Count - 1].transform.position.z);

            cuttedObj.GetComponentInChildren<MeshRenderer>().material.color = _stackCubes[_stackCubes.Count - 1].GetComponentInChildren<MeshRenderer>().material.color;
            var dir = new Vector3(0, 0, direction * 360);
            cuttedObj.transform.DORotate(dir, 1f, RotateMode.LocalAxisAdd);
        }

        public GameObject GetObject(PoolType poolType)
        {
            return PoolSignals.Instance.onGetObjectFromPool(poolType);
        }
    }
}
