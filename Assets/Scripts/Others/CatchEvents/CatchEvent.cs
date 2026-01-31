using System;
using UnityEngine;
using UnityEngine.Playables;

namespace GGJ_2026
{
	public class CatchEvent : MyMonoBehaviour
	{
		[SerializeField] private PlayableDirector playableDirector;

        public static Action onStartedWatching;
        public static Action onStoppedWatching;
        public static Action onFinished;

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
	}
}