using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class CollisionsListener : MyMonoBehaviour
    {
        [HideInInspector] public List<Collision> currentCollidingCollisionsList = new();

        public static Action<CollisionsListener, Collision> onCollisionEnter;
        public static Action<CollisionsListener, Collision> onCollisionStay;
        public static Action<CollisionsListener, Collision> onCollisionExit;

        private void OnCollisionEnter(Collision collision)
        {
            currentCollidingCollisionsList.AddIfNotInList(collision);
            onCollisionEnter?.Invoke(this, collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            onCollisionStay?.Invoke(this, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            currentCollidingCollisionsList.RemoveIfIsInList(collision);
            onCollisionExit?.Invoke(this, collision);
        }

        public void DisableAndEnableColliders()
        {
            GetAllCollidersList().ForEach(iCollider => iCollider.DisableAndEnable());
        }

        private List<Collider> GetAllCollidersList()
        {
            return GetComponents<Collider>().ToList();
        }
    }
}