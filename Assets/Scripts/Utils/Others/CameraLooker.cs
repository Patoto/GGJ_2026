using UnityEngine;

namespace PortalRollerCoaster
{
    public class CameraLooker : MyMonoBehaviour
    {
        private void LateUpdate()
        {
            LookAtCamera();
        }

        private void LookAtCamera()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}