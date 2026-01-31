using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
	public class PawPointerListener : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private PointerListener pointerListener;

        public static Action<PointerEventData> onPointerDrag;
		public static Action<PointerEventData> onPointerUp;

        public void SubscribeToEnableEvents()
		{
			PointerListener.onPointerDrag += OnPointerListenerPointerDrag;
			PointerListener.onPointerUp += OnPointerListenerPointerUp;
		}

        public void UnsubscribeFromEnableEvents()
        {
			PointerListener.onPointerDrag -= OnPointerListenerPointerDrag;
			PointerListener.onPointerUp -= OnPointerListenerPointerUp;
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