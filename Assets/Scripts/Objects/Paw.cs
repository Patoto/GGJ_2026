using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class Paw : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Transform leftLimitPoint;
		[SerializeField] private Transform rightLimitPoint;
		[SerializeField] private Transform downLimitPoint;
		[SerializeField] private Transform upLimitPoint;

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
            float clampedPositionX = Mathf.Clamp(pointerEventData.position.x, leftLimitPoint.position.x, rightLimitPoint.position.x);
            float clampedPositionY = Mathf.Clamp(pointerEventData.position.y, downLimitPoint.position.y, upLimitPoint.position.y);
            transform.position = new Vector2(clampedPositionX, clampedPositionY);
		}
    }
}