using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PortalRollerCoaster
{
    public class PointerListener : MyMonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private bool listenPointerEnter;
        [SerializeField] private bool listenPointerMove;
        [SerializeField] private bool listenPointerExit;
        [SerializeField] private bool listenPointerDown;
        [SerializeField] private bool listenPointerUp;
        [SerializeField] private bool listenPointerClick;

        public static Action<PointerListener, PointerEventData> onPointerEnter;
        public static Action<PointerListener, PointerEventData> onPointerMove;
        public static Action<PointerListener, PointerEventData> onPointerExit;
        public static Action<PointerListener, PointerEventData> onPointerDown;
        public static Action<PointerListener, PointerEventData> onPointerUp;
        public static Action<PointerListener, PointerEventData> onPointerClick;
        public static Action<PointerListener, PointerEventData> onPointerDrag;

        public bool pointerIsIn { get; private set; }
        public bool pointerIsDown { get; private set; }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            if (listenPointerEnter)
            {
                pointerIsIn = true;
                onPointerEnter?.Invoke(this, pointerEventData);
            }
        }

        public void OnPointerMove(PointerEventData pointerEventData)
        {
            if (listenPointerMove)
            {
                if (pointerIsDown)
                {
                    onPointerDrag?.Invoke(this, pointerEventData);
                }
                onPointerMove?.Invoke(this, pointerEventData);
            }
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            if (listenPointerExit)
            {
                pointerIsIn = false;
                onPointerExit?.Invoke(this, pointerEventData);
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (listenPointerDown)
            {
                pointerIsDown = true;
                onPointerDown?.Invoke(this, pointerEventData);
            }
        }

        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (listenPointerUp)
            {
                pointerIsDown = false;
                onPointerUp?.Invoke(this, pointerEventData);
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (listenPointerClick)
            {
                onPointerClick?.Invoke(this, pointerEventData);
            }
        }
    }
}