using UnityEngine;

namespace GGJ_2026
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