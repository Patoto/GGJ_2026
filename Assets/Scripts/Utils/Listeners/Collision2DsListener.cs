using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ_2026
{
    public class Collision2DsListener : MyMonoBehaviour
    {
        [HideInInspector] public List<Collision2D> currentCollidingCollision2DsList = new();

        public static Action<Collision2DsListener, Collision2D> onCollision2DEnter;
        public static Action<Collision2DsListener, Collision2D> onCollision2DStay;
        public static Action<Collision2DsListener, Collision2D> onCollision2DExit;

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            currentCollidingCollision2DsList.AddIfNotInList(collision2D);
            onCollision2DEnter?.Invoke(this, collision2D);
        }

        private void OnCollisionStay2D(Collision2D collision2D)
        {
            onCollision2DStay?.Invoke(this, collision2D);
        }

        private void OnCollisionExit2D(Collision2D collision2D)
        {
            currentCollidingCollision2DsList.RemoveIfIsInList(collision2D);
            onCollision2DExit?.Invoke(this, collision2D);
        }

        public void DisableAndEnableColliders()
        {
            GetAllCollider2DsList().ForEach(iCollider2D => iCollider2D.DisableAndEnable());
        }

        private List<Collider2D> GetAllCollider2DsList()
        {
            return GetComponents<Collider2D>().ToList();
        }
    }
}