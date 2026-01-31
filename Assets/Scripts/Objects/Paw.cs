using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class Paw : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        public void SubscribeToEnableEvents()
		{
			InputsPointerListener.onPointerDrag += OnInputsPointerListenerPointerDrag;
		}

        public void UnsubscribeFromEnableEvents()
		{			
			InputsPointerListener.onPointerDrag -= OnInputsPointerListenerPointerDrag;
		}

        private void OnInputsPointerListenerPointerDrag(PointerEventData pointerEventData)
		{
			transform.position = pointerEventData.position;
		}
    }
}