using System;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ_2026
{
	public class MaskingTape : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		public enum State
		{
			Full,
			Middle,
			Low,
			Empty
		}

		[SerializeField] private Image image;
		[SerializeField] private Sprite fullSprite;
		[SerializeField] private Sprite middleSprite;
		[SerializeField] private Sprite lowSprite;
		[SerializeField] private Sprite emptySprite;
		[SerializeField] private UIParticle onBecameEmptyUIParticle;
		[SerializeField] private Sound onEmptySoundPrefab;

		private float tapePercentageLeftAmount = 1f;
		private State currentState;

        public static Action<State> onSetState;

        public void SubscribeToEnableEvents()
		{
			MaskingStrip.onAddedY += OnMaskingStripAddedY;
		}

        public void UnsubscribeFromEnableEvents()
        {
			MaskingStrip.onAddedY -= OnMaskingStripAddedY;
        }

        private void OnMaskingStripAddedY(float y)
		{
			if (currentState != State.Empty)
			{
#if UNITY_IOS || UNITY_ANDROID
				const float MULTIPLIER = 0.00001f;
#else
				const float MULTIPLIER = 0.00002f;
#endif
				tapePercentageLeftAmount -= Mathf.Abs(y) * MULTIPLIER;
				if (tapePercentageLeftAmount <= 0f)
				{
					tapePercentageLeftAmount = 0f;
					SetState(State.Empty);
				}
				else if (tapePercentageLeftAmount <= 0.33f)
				{
					SetState(State.Low);
				}
				else if (tapePercentageLeftAmount <= 0.66f)
				{
					SetState(State.Middle);
				}
				else
				{
					SetState(State.Full);
				}
			}
		}

        private void SetState(State state)
		{
			currentState = state;
            switch (state)
            {
                case State.Full:
                    SetSprite(fullSprite);
                    break;
                case State.Middle:
                    SetSprite(middleSprite);
                    break;
                case State.Low:
                    SetSprite(lowSprite);
                    break;
                case State.Empty:
                    SetSprite(emptySprite);
					transform.DoPunchSequence();
					GameManager.instance.audioManager.PlaySound(onEmptySoundPrefab);
					onBecameEmptyUIParticle.Play();
                    break;
            }
			onSetState?.Invoke(state);
        }

        private void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}