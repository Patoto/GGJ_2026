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

        private void Start()
        {
			InvokeActionAfterSeconds(() => StartCoroutine(MyStartCoroutine()), 0.1f);
        }

        private IEnumerator MyStartCoroutine()
		{
			yield return GameManager.instance.transitionsManager.TryToPlayTransitionInCoroutine();
			if (!GameManager.instance.persistentDataManager.showedIntroThisPlaySession)
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