using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ_2026
{
	public class Trigger2DsListener : MyMonoBehaviour
	{
        [HideInInspector] public List<Collider2D> currentTriggeringCollider2DsList = new();

        public static Action<Trigger2DsListener, Collider2D> onTriggerEnter2D;
        public static Action<Trigger2DsListener, Collider2D> onTriggerStay2D;
        public static Action<Trigger2DsListener, Collider2D> onTriggerExit2D;

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            currentTriggeringCollider2DsList.AddIfNotInList(collider2D);
            onTriggerEnter2D?.Invoke(this, collider2D);
        }

        private void OnTriggerStay2D(Collider2D collider2D)
        {
            onTriggerStay2D?.Invoke(this, collider2D);
        }

        private void OnTriggerExit2D(Collider2D collider2D)
        {
            currentTriggeringCollider2DsList.RemoveIfIsInList(collider2D);
            onTriggerExit2D?.Invoke(this, collider2D);
        }

        public void DisableAndEnableColliders()
        {
            GetAllCollider2DsList().ForEach(iCollider => iCollider.DisableAndEnable());
        }

        private List<Collider2D> GetAllCollider2DsList()
        {
            return GetComponents<Collider2D>().ToList();
        }
	}
}