using System;
using System.Collections;
using DG.Tweening;
using DigitalRubyShared;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class MaskingStrip : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        private IEnumerator rollDownCoroutine;

        public void SubscribeToEnableEvents()
		{
			LevelPointerListener.onPointerDown += OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag += OnLevelPointerListenerPointerDrag;
			InputsManager.onSwipeDownGestureEnded += OnInputsManagerSwipeDownGestureEnded;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag -= OnLevelPointerListenerPointerDrag;
			InputsManager.onSwipeDownGestureEnded -= OnInputsManagerSwipeDownGestureEnded;
        }

        private void OnLevelPointerListenerPointerDrag(PointerEventData pointerEventData)
		{
			if (pointerEventData.delta.y < 0)
            {
                AddY(pointerEventData.delta.y);
            }
        }

        private void AddY(float y)
        {
            transform.SetLocalPositionY(Mathf.Max(transform.localPosition.y + y, 10f));
        }

        private void OnInputsManagerSwipeDownGestureEnded(GestureRecognizer gestureRecognizer)
        {
            StopRollDownCoroutine();
            rollDownCoroutine = StartCoroutine(RollDownCoroutine(gestureRecognizer.VelocityY));
        }

        private void StopRollDownCoroutine()
        {
            StopCoroutine(rollDownCoroutine);
        }

        private IEnumerator RollDownCoroutine(float velocityY)
		{
            float acceleration = -velocityY * 0.5f;
			while (Mathf.Abs(velocityY) > 0.1f)
			{
				AddY(velocityY * Time.deltaTime);
                velocityY += acceleration * Time.deltaTime;
				yield return null;
			}
		}

        private void OnLevelPointerListenerPointerDown(PointerEventData pointerEventData)
		{
			StopRollDownCoroutine();
		}
    }
}