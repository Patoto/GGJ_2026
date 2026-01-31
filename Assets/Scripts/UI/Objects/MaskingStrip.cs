using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class MaskingStrip : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        public void SubscribeToEnableEvents()
		{
			LevelPointerListener.onPointerDrag += OnLevelPointerListenerPointerDrag;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LevelPointerListener.onPointerDrag -= OnLevelPointerListenerPointerDrag;
        }

        private void OnLevelPointerListenerPointerDrag(PointerEventData pointerEventData)
		{
			if (pointerEventData.delta.y < 0)
			{
				transform.SetLocalPositionY(Mathf.Max(transform.localPosition.y + pointerEventData.delta.y, 10f));
			}
		}
    }
}