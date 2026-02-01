using System;
using System.Collections;
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

		[SerializeField] private RectTransform rectTransform;
		[SerializeField] private Image image;
		[SerializeField] private Paw paw;
		[SerializeField] private Sprite playingSprite;
		[SerializeField] private Sprite sleepingSprite;
		[SerializeField] private Sprite caughtSprite;
		[SerializeField] private Image sleepingFaceImage;
		[SerializeField] private Sprite sleepingFaceSprite1;
		[SerializeField] private Sprite sleepingFaceSprite2;

        private IEnumerator sleepLookCoroutine;

        public void SubscribeToEnableEvents()
		{
			CatchHandler.onPawToggled += OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine += OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine += OnCatchHandlerStoppedWatchCoroutine;
			CatchHandler.onCaught += OnCatchHandlerCaught;
			LevelHandler.onStartedMyStartCoroutine += OnLevelHandlerStartedMyStartCoroutine;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchHandler.onPawToggled -= OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine -= OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine -= OnCatchHandlerStoppedWatchCoroutine;
			CatchHandler.onCaught -= OnCatchHandlerCaught;
			LevelHandler.onStartedMyStartCoroutine -= OnLevelHandlerStartedMyStartCoroutine;
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
			sleepingFaceImage.gameObject.SetActive(false);
			StopCoroutine(sleepLookCoroutine);
            switch (state)
            {
                case State.Playing:
					SetSprite(playingSprite);
                    break;
                case State.Sleeping:
					SetSprite(sleepingSprite);
					sleepingFaceImage.gameObject.SetActive(true);
					sleepLookCoroutine = StartCoroutine(SleepLookCoroutine());
                    break;
                case State.Caught:
					SetSprite(caughtSprite);
                    break;
            }
        }

        private IEnumerator SleepLookCoroutine()
		{
			sleepingFaceImage.sprite = sleepingFaceSprite1;
			yield return new WaitForSeconds(1f);
			while (true)
			{
				sleepingFaceImage.sprite = sleepingFaceImage.sprite == sleepingFaceSprite1 ? sleepingFaceSprite2 : sleepingFaceSprite1;
				yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.4f));
			}
		}

        private void OnCatchHandlerCaught()
		{
			SetState(State.Caught);
		}

        private void OnLevelHandlerStartedMyStartCoroutine()
		{
			if (!GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession())
			{
				rectTransform.SetPositionX(0f);
			}
		}
    }
}