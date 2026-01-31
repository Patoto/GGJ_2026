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

        private bool canReceiveInputs;

        public static Action<Vector2, Vector2> onUpdatedPosition;
        public static Action<bool> onToggled;
        public static Action<bool> onToggledCanReceiveInputs;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

		public virtual void SubscribeToInitializeEvents()
		{
			LevelPointerListener.onPointerDown += OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag += OnLevelPointerListenerPointerDrag;
			LevelPointerListener.onPointerUp += OnLevelPointerListenerPointerUp;
			CatchHandler.onCaught += OnCatchHandlerCaught;
			LevelHandler.onFinishedIntroTimeline += OnLevelHandlerFinishedIntroTimeline;
		}

		public virtual void UnsubscribeFromInitializeEvents()
		{
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag -= OnLevelPointerListenerPointerDrag;
			LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
			CatchHandler.onCaught -= OnCatchHandlerCaught;
			LevelHandler.onFinishedIntroTimeline -= OnLevelHandlerFinishedIntroTimeline;
		}

        private void OnLevelPointerListenerPointerDown(PointerEventData pointerEventData)
		{
			if (canReceiveInputs)
			{
				Toggle(true);
				UpdatePosition(pointerEventData);
			}
		}

        private void OnLevelPointerListenerPointerDrag(PointerEventData pointerEventData)
        {
			if (canReceiveInputs)
			{
				UpdatePosition(pointerEventData);
			}
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
			if (canReceiveInputs)
			{
				Toggle(false);
			}
		}

        private void OnCatchHandlerCaught()
		{
			ToggleCanReveiveInputs(false);
		}

		private void Toggle(bool on)
		{
			gameObject.SetActive(on);
			onToggled?.Invoke(on);
		}

        private void OnLevelHandlerFinishedIntroTimeline()
		{
			GameManager.instance.InvokeActionAfterSeconds(() => ToggleCanReveiveInputs(true), 0.5f);
		}

		private void ToggleCanReveiveInputs(bool on)
		{
			canReceiveInputs = on;
			onToggledCanReceiveInputs?.Invoke(on);
		}
    }
}