using System;
using System.Collections;
using UnityEngine;

namespace GGJ_2026
{
    public class CatchHandler : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Paw paw;

        private IEnumerator watchCoroutine;

        public static Action onCatched;

        public void SubscribeToEnableEvents()
		{
			CatchEvent.onStartedWatching += OnCatchEventStartedWatching;
			CatchEvent.onStoppedWatching += OnCatchEventStoppedWatching;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchEvent.onStartedWatching -= OnCatchEventStartedWatching;
			CatchEvent.onStoppedWatching -= OnCatchEventStoppedWatching;
        }

        private void OnCatchEventStartedWatching()
		{
			StopWatchCoroutine();
			watchCoroutine = StartCoroutine(WatchCoroutine());
		}

        private void StopWatchCoroutine()
		{
			StopCoroutine(watchCoroutine);
		}

        private IEnumerator WatchCoroutine()
		{
			while (true)
			{
				if (paw != null && paw.gameObject.activeSelf)
				{
					Catch();
				}
				yield return null;
			}
		}

        private void Catch()
        {
            StopWatchCoroutine();
			onCatched?.Invoke();
        }

        private void OnCatchEventStoppedWatching()
		{
			StopWatchCoroutine();
		}
    }
}