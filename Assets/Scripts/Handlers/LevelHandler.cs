using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace GGJ_2026
{
	public class LevelHandler : MyMonoBehaviour
	{
		[SerializeField] private PlayableDirector introPlayableDirector;

        public static Action onFinishedIntroTimeline;
        public static Action onStartedMyStartCoroutine;
        public static Action onFinishedTransitionInCoroutine;

        private void Start()
        {
			GameManager.instance.persistentDataManager.startedALevelThisSessionAmount++;
			InvokeActionAfterSeconds(() => StartCoroutine(MyStartCoroutine()), 0.1f);
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