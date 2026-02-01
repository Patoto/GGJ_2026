using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GGJ_2026
{
	public class CatchEvent : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private PlayableDirector playableDirector;

        public static Action onStartedWatching;
        public static Action onStoppedWatching;
        public static Action onFinished;

        public void SubscribeToEnableEvents()
		{
			CatchHandler.onCaught += OnCatchHandlerCaught;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchHandler.onCaught -= OnCatchHandlerCaught;
        }

        public void StartCatchEvent()
		{
			playableDirector.Play();
		}

        public void StartWatching()
		{
			onStartedWatching?.Invoke();
		}

		public void StopWatching()
		{			
			onStoppedWatching?.Invoke();
		}

		public void OnFinished()
		{
			onFinished?.Invoke();
		}

        private void OnCatchHandlerCaught()
		{
			playableDirector.Stop();
		}
    }
}