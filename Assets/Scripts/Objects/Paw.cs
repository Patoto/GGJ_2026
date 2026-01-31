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

        public static Action<Vector2, Vector2> onUpdatedPosition;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

		public virtual void SubscribeToInitializeEvents()
		{
			LevelPointerListener.onPointerDown += OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag += OnLevelPointerListenerPointerDrag;
			LevelPointerListener.onPointerUp += OnLevelPointerListenerPointerUp;
		}

		public virtual void UnsubscribeFromInitializeEvents()
		{
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag -= OnLevelPointerListenerPointerDrag;
			LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
		}

        private void OnLevelPointerListenerPointerDown(PointerEventData pointerEventData)
		{
			gameObject.SetActive(true);
			UpdatePosition(pointerEventData);
		}

        private void OnLevelPointerListenerPointerDrag(PointerEventData pointerEventData)
        {
            UpdatePosition(pointerEventData);
        }

        private void UpdatePosition(PointerEventData pointerEventData)
        {
            float clampedPositionX = Mathf.Clamp(pointerEventData.position.x, leftLimitPoint.position.x, rightLimitPoint.position.x);
            float clampedPositionY = Mathf.Clamp(pointerEventData.position.y, downLimitPoint.position.y, upLimitPoint.position.y);
			Vector2 previousPosition = transform.position;
            transform.position = new Vector2(clampedPositionX, clampedPositionY);
			onUpdatedPosition?.Invoke(previousPosition, transform.position);
        }

        private void OnLevelPointerListenerPointerUp(PointerEventData pointerEventData)
		{
			gameObject.SetActive(false);
		}
    }
}