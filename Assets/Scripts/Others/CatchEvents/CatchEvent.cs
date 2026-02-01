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
			MaskingTape.onSetState += OnMaskingTapeSetState;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchHandler.onCaught -= OnCatchHandlerCaught;
			MaskingTape.onSetState -= OnMaskingTapeSetState;
        }

        public void StartCatchEvent()
		{
			playableDirector.Play();
			playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(UnityEngine.Random.Range(1f, 2f));
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
            StopPlayableDirector();
        }

        private void StopPlayableDirector()
        {
            playableDirector.Stop();
        }

        private void OnMaskingTapeSetState(MaskingTape.State state)
		{
			if (state == MaskingTape.State.Empty)
			{
				StopPlayableDirector();
			}
		}
    }
}