using System;
using System.Collections;
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
            Paw.onUpdatedPosition += OnPawUpdatedPosition;
            LevelPointerListener.onPointerUp += OnLevelPointerListenerPointerUp;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
            Paw.onUpdatedPosition -= OnPawUpdatedPosition;
            LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
        }

        private void OnLevelPointerListenerPointerDown(PointerEventData pointerEventData)
		{
			StopRollDownCoroutine();
		}

        private void OnPawUpdatedPosition(Vector2 previousPosition, Vector2 newPosition)
        {
            float deltaY = newPosition.y - previousPosition.y;
			if (deltaY < 0f)
            {
                AddY(deltaY);
            }
        }

        private void AddY(float y)
        {
            transform.SetLocalPositionY(Mathf.Max(transform.localPosition.y + y, 10f));
        }

        private void OnLevelPointerListenerPointerUp(PointerEventData pointerEventData)
        {
            StopRollDownCoroutine();
            //rollDownCoroutine = StartCoroutine(RollDownCoroutine(pointerEventData.delta.y * 5f));
        }

        private void StopRollDownCoroutine()
        {
            StopCoroutine(rollDownCoroutine);
        }

        private IEnumerator RollDownCoroutine(float speed)
		{
			while (Mathf.Abs(speed) > 100f)
			{
				AddY(speed * Time.deltaTime);
                speed -= 250f * Time.deltaTime;
				yield return null;
			}
		}
    }
}