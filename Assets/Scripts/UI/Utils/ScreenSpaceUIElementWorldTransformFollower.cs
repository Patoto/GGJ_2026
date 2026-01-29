using UnityEngine;

namespace PortalRollerCoaster
{
    public class ScreenSpaceUIElementWorldTransformFollower : MyMonoBehaviour
    {
        private Transform worldTransformToFollow;

        public void SetWorldTransformToFollow(Transform transform)
        {
            worldTransformToFollow = transform;
        }

        private void LateUpdate()
        {
            if (worldTransformToFollow != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(worldTransformToFollow.position);
            }
        }
    }
}