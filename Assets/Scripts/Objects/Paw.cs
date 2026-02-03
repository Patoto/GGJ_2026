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
			LevelHandler.onStartedMyStartCoroutine += OnLevelHandlerStartedMyStartCoroutine;
			MaskingTape.onSetState += OnMaskingTapeSetState;
		}

		public virtual void UnsubscribeFromInitializeEvents()
		{
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag -= OnLevelPointerListenerPointerDrag;
			LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
			CatchHandler.onCaught -= OnCatchHandlerCaught;
			LevelHandler.onFinishedIntroTimeline -= OnLevelHandlerFinishedIntroTimeline;
			LevelHandler.onStartedMyStartCoroutine -= OnLevelHandlerStartedMyStartCoroutine;
			MaskingTape.onSetState -= OnMaskingTapeSetState;
		}

        private void OnLevelPointerListenerPointerDown(PointerEventData pointerEventData)
		{
			if (this != null && canReceiveInputs)
			{
				Toggle(true);
				UpdatePosition(pointerEventData);
			}
		}

        private void OnLevelPointerListenerPointerDrag(PointerEventData pointerEventData)
        {
			if (this != null && canReceiveInputs)
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
			if (this != null && canReceiveInputs)
			{
				Toggle(false);
			}
		}

        private void OnCatchHandlerCaught()
		{
			if (this != null)
			{
				ToggleCanReveiveInputs(false);
				Toggle(false);
			}
		}

		private void Toggle(bool on)
		{
			gameObject.SetActive(on);
			onToggled?.Invoke(on);
		}

        private void OnLevelHandlerFinishedIntroTimeline()
		{
			if (this != null)
			{
				ToggleCanReveiveInputs(true);
			}
		}

		private void ToggleCanReveiveInputs(bool on)
		{
			canReceiveInputs = on;
			onToggledCanReceiveInputs?.Invoke(on);
		}

        private void OnLevelHandlerStartedMyStartCoroutine()
		{
			if (this != null && !GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession())
			{
				ToggleCanReveiveInputs(true);
			}
		}

        private void OnMaskingTapeSetState(MaskingTape.State state)
		{
			if (this != null && state == MaskingTape.State.Empty)
			{
				ToggleCanReveiveInputs(false);
				Toggle(false);
			}
		}
    }
}