using UnityEngine;

public class StackCubeMovementController : MonoBehaviour
{
    private bool _status;

    public void XAxisMovement(Transform objTransform, Vector2 maxMinPosX, float objSpeed)
    {
        if (objTransform.position.x > maxMinPosX.x)
        {
            _status = false;
        }
        else if (objTransform.position.x < maxMinPosX.y)
        {
            _status = true;
        }
        if (_status == true)
        {
            objTransform.Translate(objSpeed * Time.deltaTime, 0, 0);
        }
        else if (_status == false)
        {
            objTransform.Translate(-objSpeed * Time.deltaTime, 0, 0);
        }
    }
}
