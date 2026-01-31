using System;
using System.Collections;
using UnityEngine;

namespace GGJ_2026
{
    public class CatchHandler : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private Paw paw;

        private IEnumerator watchCoroutine;
        private bool watching;

        public static Action onCatched;
        public static Action<bool, bool> onPawToggled;
        public static Action onStartedWatchCoroutine;
        public static Action onStoppedWatchCoroutine;

        public void SubscribeToEnableEvents()
		{
			CatchEvent.onStartedWatching += OnCatchEventStartedWatching;
			CatchEvent.onStoppedWatching += OnCatchEventStoppedWatching;
			Paw.onToggled += OnPawToggled;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchEvent.onStartedWatching -= OnCatchEventStartedWatching;
			CatchEvent.onStoppedWatching -= OnCatchEventStoppedWatching;
			Paw.onToggled -= OnPawToggled;
        }

        private void OnCatchEventStartedWatching()
		{
			StopWatchCoroutine();
			watchCoroutine = StartCoroutine(WatchCoroutine());
		}

        private void StopWatchCoroutine()
		{
			watching = false;
			StopCoroutine(watchCoroutine);
			onStoppedWatchCoroutine?.Invoke();
		}

        private IEnumerator WatchCoroutine()
		{
			onStartedWatchCoroutine?.Invoke();
			watching = true;
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

        private void OnPawToggled(bool on)
		{
			onPawToggled?.Invoke(on, watching);
		}
    }
}