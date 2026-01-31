using System;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ_2026
{
	public class Cat : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		private enum State
		{
			Playing,
			Sleeping,
			Caught
		}

		[SerializeField] private Image image;
		[SerializeField] private Paw paw;
		[SerializeField] private Sprite playingSprite;
		[SerializeField] private Sprite sleepingSprite;
		[SerializeField] private Sprite caughtSprite;

        public void SubscribeToEnableEvents()
		{
			CatchHandler.onPawToggled += OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine += OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine += OnCatchHandlerStoppedWatchCoroutine;
			CatchHandler.onCaught += OnCatchHandlerCaught;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchHandler.onPawToggled -= OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine -= OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine -= OnCatchHandlerStoppedWatchCoroutine;
			CatchHandler.onCaught -= OnCatchHandlerCaught;
        }

        private void OnCatchHandlerPawToggled(bool pawOn, bool catchHandlerWatching)
		{
			SetState(pawOn ? State.Playing : catchHandlerWatching ? State.Sleeping : State.Playing);
		}

		private void SetSprite(Sprite sprite)
		{
			image.sprite = sprite;
		}

        private void OnCatchHandlerStartedWatchCoroutine()
		{
			if (!paw.gameObject.activeSelf)
			{
				SetState(State.Sleeping);
			}
		}

        private void OnCatchHandlerStoppedWatchCoroutine()
		{
			SetState(State.Playing);
		}

		private void SetState(State state)
		{
            switch (state)
            {
                case State.Playing:
					SetSprite(playingSprite);
                    break;
                case State.Sleeping:
					SetSprite(sleepingSprite);
                    break;
                case State.Caught:
					SetSprite(caughtSprite);
                    break;
            }
        }

        private void OnCatchHandlerCaught()
		{
			SetState(State.Caught);
		}
    }
}