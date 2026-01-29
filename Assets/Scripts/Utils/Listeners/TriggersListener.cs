using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class TriggersListener : MyMonoBehaviour
    {
        [HideInInspector] public List<Collider> currentTriggeringCollidersList = new();

        public static Action<TriggersListener, Collider> onTriggerEnter;
        public static Action<TriggersListener, Collider> onTriggerStay;
        public static Action<TriggersListener, Collider> onTriggerExit;

        private void OnTriggerEnter(Collider collider)
        {
            currentTriggeringCollidersList.AddIfNotInList(collider);
            onTriggerEnter?.Invoke(this, collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            onTriggerStay?.Invoke(this, collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            currentTriggeringCollidersList.RemoveIfIsInList(collider);
            onTriggerExit?.Invoke(this, collider);
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