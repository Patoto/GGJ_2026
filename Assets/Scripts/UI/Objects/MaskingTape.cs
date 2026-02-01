using System;
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

		private float tapePercentageLeftAmount = 1f;

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
			tapePercentageLeftAmount -= Mathf.Abs(y) * 0.00001f;
			if (tapePercentageLeftAmount <= 0f)
			{
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

        private void SetState(State state)
		{
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