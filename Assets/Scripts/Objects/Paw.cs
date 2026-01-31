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

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

		public virtual void SubscribeToInitializeEvents()
		{
			PawPointerListener.onPointerDrag += OnInputsPointerListenerPointerDrag;
			PawPointerListener.onPointerUp += OnPawPointerListenerPointerUp;
		}

		public virtual void UnsubscribeFromInitializeEvents()
		{
			PawPointerListener.onPointerDrag -= OnInputsPointerListenerPointerDrag;
			PawPointerListener.onPointerUp -= OnPawPointerListenerPointerUp;
		}

        private void OnInputsPointerListenerPointerDrag(PointerEventData pointerEventData)
		{
			gameObject.SetActive(true);
            float clampedPositionX = Mathf.Clamp(pointerEventData.position.x, leftLimitPoint.position.x, rightLimitPoint.position.x);
            float clampedPositionY = Mathf.Clamp(pointerEventData.position.y, downLimitPoint.position.y, upLimitPoint.position.y);
            transform.position = new Vector2(clampedPositionX, clampedPositionY);
		}

        private void OnPawPointerListenerPointerUp(PointerEventData pointerEventData)
		{
			gameObject.SetActive(false);
		}
    }
}