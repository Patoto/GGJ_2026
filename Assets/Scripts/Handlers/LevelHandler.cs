using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace GGJ_2026
{
	public class LevelHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private PlayableDirector introPlayableDirector;

        public static Action onFinishedIntroTimeline;
        public static Action onStartedMyStartCoroutine;
        public static Action onFinishedTransitionInCoroutine;

        public void SubscribeToEnableEvents()
		{
			TitleHandler.onDeactivated += OnTitleHandlerDeactivated;
		}

        public void UnsubscribeFromEnableEvents()
        {
			TitleHandler.onDeactivated -= OnTitleHandlerDeactivated;
        }

        private void OnTitleHandlerDeactivated()
        {
			StartCoroutine(MyStartCoroutine());
        }

        private IEnumerator MyStartCoroutine()
		{
			onStartedMyStartCoroutine?.Invoke();
			yield return GameManager.instance.transitionsManager.TryToPlayTransitionInCoroutine();
			onFinishedTransitionInCoroutine?.Invoke();
			if (GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession())
			{
				introPlayableDirector.Play();
			}
		}

		public void OnFinishedIntroTimeline()
		{
			onFinishedIntroTimeline?.Invoke();
		}
    }
}