using System;
using System.Collections;
using TapticPlugin;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class MaskingStrip : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        [SerializeField] private RectTransform currentTopMaskingStripImageRectTransform;
        [SerializeField] private RectTransform currentBottomMaskingStripImageRectTransform;
        [SerializeField] private RectTransform bottomMaskingStripLimitPoint;
        [SerializeField] private Sound maskingSoundPrefab;

        private IEnumerator rollDownCoroutine;
        private float maskingStripImageYDifference;
        private Sound maskingSound;

        public static Action<float> onAddedY;

        public void SubscribeToEnableEvents()
		{
			LevelPointerListener.onPointerDown += OnLevelPointerListenerPointerDown;
            Paw.onUpdatedPosition += OnPawUpdatedPosition;
            LevelPointerListener.onPointerUp += OnLevelPointerListenerPointerUp;
            MaskingTape.onSetState += OnMaskingTapeSetState;
            Paw.onToggled += OnPawToggled;
		}

        public void UnsubscribeFromEnableEvents()
        {
			LevelPointerListener.onPointerDown -= OnLevelPointerListenerPointerDown;
            Paw.onUpdatedPosition -= OnPawUpdatedPosition;
            LevelPointerListener.onPointerUp -= OnLevelPointerListenerPointerUp;
            MaskingTape.onSetState -= OnMaskingTapeSetState;
            Paw.onToggled -= OnPawToggled;
        }

        private void Start()
        {
            maskingStripImageYDifference = currentTopMaskingStripImageRectTransform.position.y - currentBottomMaskingStripImageRectTransform.position.y;
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
            transform.SetLocalPositionY(transform.localPosition.y + y);
            if(currentBottomMaskingStripImageRectTransform.position.y < bottomMaskingStripLimitPoint.position.y)
            {
                currentBottomMaskingStripImageRectTransform.SetPositionY(currentBottomMaskingStripImageRectTransform.position.y + (maskingStripImageYDifference * 2f));
                RectTransform tempCurrentTopMaskingStripImageRectTransform = currentTopMaskingStripImageRectTransform;
                currentTopMaskingStripImageRectTransform = currentBottomMaskingStripImageRectTransform;
                currentBottomMaskingStripImageRectTransform = tempCurrentTopMaskingStripImageRectTransform;
                currentTopMaskingStripImageRectTransform.SetSiblingIndex(0);
            }
            if(maskingSound == null)
            {
                maskingSound = GameManager.instance.audioManager.PlaySound(maskingSoundPrefab);
            }
            TapticManager.Impact(ImpactFeedback.Light);
            onAddedY?.Invoke(y);
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

        private void OnMaskingTapeSetState(MaskingTape.State state)
        {
            if(state == MaskingTape.State.Empty)
            {
                Destroy(gameObject);
            }
        }

        private void OnPawToggled(bool on)
        {
            if(!on)
            {
                maskingSound?.Stop();
            }
        }
    }
}