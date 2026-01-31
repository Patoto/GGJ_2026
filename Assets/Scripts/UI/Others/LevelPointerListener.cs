using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
	public class LevelPointerListener : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private PointerListener pointerListener;

        public static Action<PointerEventData> onPointerDown;
        public static Action<PointerEventData> onPointerDrag;
		public static Action<PointerEventData> onPointerUp;

        public void SubscribeToEnableEvents()
		{
			PointerListener.onPointerDown += OnPointerListenerPointerDown;
			PointerListener.onPointerDrag += OnPointerListenerPointerDrag;
			PointerListener.onPointerUp += OnPointerListenerPointerUp;
		}

        public void UnsubscribeFromEnableEvents()
        {
			PointerListener.onPointerDown -= OnPointerListenerPointerDown;
			PointerListener.onPointerDrag -= OnPointerListenerPointerDrag;
			PointerListener.onPointerUp -= OnPointerListenerPointerUp;
        }

        private void OnPointerListenerPointerDown(PointerListener pointerListener, PointerEventData pointerEventData)
		{
			onPointerDown?.Invoke(pointerEventData);
		}

        private void OnPointerListenerPointerDrag(PointerListener pointerListener, PointerEventData pointerEventData)
		{
			onPointerDrag?.Invoke(pointerEventData);
		}

        private void OnPointerListenerPointerUp(PointerListener pointerListener, PointerEventData pointerEventData)
		{
			onPointerUp?.Invoke(pointerEventData);
		}
    }
}