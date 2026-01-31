using System;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ_2026
{
	public class Cat : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private Image image;
		[SerializeField] private Paw paw;
		[SerializeField] private Sprite awakeSprite;
		[SerializeField] private Sprite asleepSprite;

        public void SubscribeToEnableEvents()
		{
			CatchHandler.onPawToggled += OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine += OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine += OnCatchHandlerStoppedWatchCoroutine;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchHandler.onPawToggled -= OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine -= OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine -= OnCatchHandlerStoppedWatchCoroutine;
        }

        private void OnCatchHandlerPawToggled(bool pawOn, bool catchHandlerWatching)
		{
			ToggleAwake(pawOn ? true : catchHandlerWatching ? false : true);
		}

        private void ToggleAwake(bool on)
		{
			SetSprite(on ? awakeSprite : asleepSprite);
		}

		private void SetSprite(Sprite sprite)
		{
			image.sprite = sprite;
		}

        private void OnCatchHandlerStartedWatchCoroutine()
		{
			if (!paw.gameObject.activeSelf)
			{
				ToggleAwake(false);
			}
		}

        private void OnCatchHandlerStoppedWatchCoroutine()
		{
			ToggleAwake(true);
		}
    }
}