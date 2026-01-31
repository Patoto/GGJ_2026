using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class MaskingStrip : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        [SerializeField] private RectTransform pullAreaRectTransform;

        private IEnumerator rollDownCoroutine;

        public void SubscribeToEnableEvents()
		{
			LevelPointerListener.onPointerDown += OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag += OnLevelPointerListenerPointerDrag;
            LevelPointerListener.onPointerUp += OnLevelPointerListenerPointerUp;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
			LevelPointerListener.onPointerDrag -= OnLevelPointerListenerPointerDrag;
            LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
        }

        private void OnLevelPointerListenerPointerDown(PointerEventData pointerEventData)
		{
			StopRollDownCoroutine();
		}

        private void OnLevelPointerListenerPointerDrag(PointerEventData pointerEventData)
		{
			if (pointerEventData.delta.y < 0 && RectTransformUtility.RectangleContainsScreenPoint(pullAreaRectTransform, pointerEventData.position, pointerEventData.pressEventCamera))
            {
                AddY(pointerEventData.delta.y);
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