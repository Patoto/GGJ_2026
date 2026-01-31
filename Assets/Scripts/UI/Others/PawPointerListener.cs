using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
	public class PawPointerListener : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private PointerListener pointerListener;

        public static Action<PointerEventData> onPointerDrag;

        public void SubscribeToEnableEvents()
		{
			PointerListener.onPointerDrag += OnPointerListenerPointerDrag;
		}

        public void UnsubscribeFromEnableEvents()
        {
			PointerListener.onPointerDrag -= OnPointerListenerPointerDrag;
        }

        private void OnPointerListenerPointerDrag(PointerListener pointerListener, PointerEventData pointerEventData)
		{
			onPointerDrag?.Invoke(pointerEventData);
		}
    }
}