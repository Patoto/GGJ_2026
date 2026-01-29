using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PortalRollerCoaster
{
    public class ColliderPointerListener : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        [Header("Settings")]
        [SerializeField] private bool mustBeFirstRaycastHit;
        [SerializeField] private bool listenPointerEnter;
        [SerializeField] private bool listenPointerMove;
        [SerializeField] private bool listenPointerExit;
        [SerializeField] private bool listenPointerDown;
        [SerializeField] private bool listenPointerUp;
        [Header("References")]
        [SerializeField] private List<Rigidbody> rigidbodiesList = new();
        [SerializeField] private List<Collider> collidersList = new();

        public static Action<ColliderPointerListener, RaycastHit> onPointerEnter;
        public static Action<ColliderPointerListener, RaycastHit> onPointerMove;
        public static Action<ColliderPointerListener> onPointerExit;
        public static Action<ColliderPointerListener, RaycastHit> onPointerDown;
        public static Action<ColliderPointerListener, RaycastHit> onPointerUp;

        private bool pointerEntered;

        public void SubscribeToEnableEvents()
        {
            PointerListener.onPointerEnter += OnPointerListenerPointerEnter;
            PointerListener.onPointerMove += OnPointerListenerPointerMove;
            PointerListener.onPointerExit += OnPointerListenerPointerExit;
            PointerListener.onPointerDown += OnPointerListenerPointerDown;
            PointerListener.onPointerUp += OnPointerListenerPointerUp;
            onDisabled += OnMyMonoBehaviourDisabled;
        }

        public void UnsubscribeFromEnableEvents()
        {
            PointerListener.onPointerEnter -= OnPointerListenerPointerEnter;
            PointerListener.onPointerMove -= OnPointerListenerPointerMove;
            PointerListener.onPointerExit -= OnPointerListenerPointerExit;
            PointerListener.onPointerDown -= OnPointerListenerPointerDown;
            PointerListener.onPointerUp -= OnPointerListenerPointerUp;
            onDisabled -= OnMyMonoBehaviourDisabled;
        }

        private void OnPointerListenerPointerEnter(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            OnPointerListenerPointerMove(pointerListener, pointerEventData);
        }

        private void OnPointerListenerPointerMove(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (listenPointerEnter || listenPointerMove || listenPointerExit)
            {
                bool raycastFromScreenPositionHitsCollider = RaycastFromScreenPositionHitsCollider(pointerEventData.position, out RaycastHit raycastHit, mustBeFirstRaycastHit);
                if (raycastFromScreenPositionHitsCollider)
                {
                    if (listenPointerEnter && !pointerEntered)
                    {
                        pointerEntered = true;
                        onPointerEnter?.Invoke(this, raycastHit);
                    }
                    else if (listenPointerMove)
                    {
                        onPointerMove?.Invoke(this, raycastHit);
                    }
                }
                else
                {
                    CheckForPointerExit();
                }
            }
        }

        private void CheckForPointerExit()
        {
            if (listenPointerExit && pointerEntered)
            {
                pointerEntered = false;
                onPointerExit?.Invoke(this);
            }
        }

        private void OnPointerListenerPointerExit(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            OnPointerListenerPointerMove(pointerListener, pointerEventData);
        }

        private void OnPointerListenerPointerDown(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (listenPointerDown && RaycastFromScreenPositionHitsCollider(pointerEventData.position, out RaycastHit raycastHit, mustBeFirstRaycastHit))
            {
                onPointerDown?.Invoke(this, raycastHit);
            }
        }

        private void OnPointerListenerPointerUp(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (listenPointerUp && RaycastFromScreenPositionHitsCollider(pointerEventData.position, out RaycastHit raycastHit, mustBeFirstRaycastHit))
            {
                onPointerUp?.Invoke(this, raycastHit);
            }
        }

        public bool MouseIsOverCollider()
        {
            return RaycastFromScreenPositionHitsCollider(Input.mousePosition, out RaycastHit raycastHit, mustBeFirstRaycastHit);
        }

        private bool RaycastFromScreenPositionHitsCollider(Vector3 screenPosition, out RaycastHit raycastHit, bool mustBeFirstRaycastHit)
        {
            List<RaycastHit> raycastHitsList = Camera.main.GetRaycastHitsListAtScreenPosition(screenPosition);
            List<RaycastHit> collidersRaycastHitsList = raycastHitsList.Where(iRaycastHit => rigidbodiesList.Contains(iRaycastHit.rigidbody) || collidersList.Contains(iRaycastHit.collider)).ToList();
            raycastHit = collidersRaycastHitsList.FirstOrDefault();
            if (mustBeFirstRaycastHit)
            {
                raycastHit = raycastHitsList.FirstOrDefault();
            }
            return collidersRaycastHitsList.Contains(raycastHit);
        }

        private void OnMyMonoBehaviourDisabled(MyMonoBehaviour myMonoBehaviour)
        {
            if (myMonoBehaviour == this)
            {
                CheckForPointerExit();
            }
        }
    }
}