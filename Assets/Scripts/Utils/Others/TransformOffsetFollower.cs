using NaughtyAttributes;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class TransformOffsetFollower : MyMonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;
        [SerializeField] private bool followZ = true;
        [SerializeField] private bool setTransformToFollowOffsetManually;
        [SerializeField][ShowIf(nameof(setTransformToFollowOffsetManually))] private Vector3 transformToFollowOffset;
        [Header("References")]
        [SerializeField] public Transform transformToFollow;

        protected override void Awake()
        {
            base.Awake();
            Setup(transformToFollow);
        }

        public void Setup(Transform transformToFollow)
        {
            Setup(transform.position, transformToFollow);
        }

        public void Setup(Vector3 currentPosition, Transform transformToFollow)
        {
            transform.position = currentPosition;
            SetTransformToFollow(transformToFollow);
            TryToUpdatePosition();
        }

        private void SetTransformToFollow(Transform transformToFollow)
        {
            this.transformToFollow = transformToFollow;
            TryToSetupTransformToFollowOffset();
        }

        private void TryToSetupTransformToFollowOffset()
        {
            if (!setTransformToFollowOffsetManually && transformToFollow != null)
            {
                SetTransformToFollowOffset(transform.position - transformToFollow.position);
            }
        }

        private void LateUpdate()
        {
            TryToUpdatePosition();
        }

        private void SetTransformToFollowOffset(Vector3 transformToFollowOffset)
        {
            this.transformToFollowOffset = transformToFollowOffset;
        }

        private void TryToUpdatePosition()
        {
            if (transformToFollow != null)
            {
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            Vector3 position = GetNewPosition();
            transform.position = position;
        }

        private Vector3 GetNewPosition()
        {
            Vector3 newPosition = transform.position;
            Vector3 transformToFollowPosition = transformToFollow.transform.position;
            if (followX)
            {
                newPosition.x = transformToFollowPosition.x + transformToFollowOffset.x;
            }
            if (followY)
            {
                newPosition.y = transformToFollowPosition.y + transformToFollowOffset.y;
            }
            if (followZ)
            {
                newPosition.z = transformToFollowPosition.z + transformToFollowOffset.z;
            }
            return newPosition;
        }

        public void Toggle(bool on)
        {
            enabled = on;
        }
    }
}