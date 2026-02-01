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
			Caught,
			Happy
		}

		[SerializeField] private RectTransform rectTransform;
		[SerializeField] private Image image;
		[SerializeField] private Paw paw;
		[SerializeField] private Sprite playingSprite;
		[SerializeField] private Sprite sleepingSprite;
		[SerializeField] private Sprite caughtSprite1;
		[SerializeField] private Sprite caughtSprite2;
		[SerializeField] private Sprite happySprite;
		[SerializeField] private Image sleepingFaceImage;
		[SerializeField] private Sprite sleepingFaceSprite1;
		[SerializeField] private Sprite sleepingFaceSprite2;
		[SerializeField] private Animator animator;

        private IEnumerator sleepLookCoroutine;
		private State currentState;

		private const string SHAKE_ANIMATION_NAME = "Shake";

        public void SubscribeToEnableEvents()
		{
			CatchHandler.onPawToggled += OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine += OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine += OnCatchHandlerStoppedWatchCoroutine;
			CatchHandler.onCaught += OnCatchHandlerCaught;
			LevelHandler.onStartedMyStartCoroutine += OnLevelHandlerStartedMyStartCoroutine;
			LoseHandler.onAboutToShowJumpscareMom += OnLoseHandlerAboutToShowJumpscareMom;
			MaskingTape.onSetState += OnMaskingTapeSetState;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchHandler.onPawToggled -= OnCatchHandlerPawToggled;
			CatchHandler.onStartedWatchCoroutine -= OnCatchHandlerStartedWatchCoroutine;
			CatchHandler.onStoppedWatchCoroutine -= OnCatchHandlerStoppedWatchCoroutine;
			CatchHandler.onCaught -= OnCatchHandlerCaught;
			LevelHandler.onStartedMyStartCoroutine -= OnLevelHandlerStartedMyStartCoroutine;
			LoseHandler.onAboutToShowJumpscareMom -= OnLoseHandlerAboutToShowJumpscareMom;
			MaskingTape.onSetState -= OnMaskingTapeSetState;
        }

        private void OnCatchHandlerPawToggled(bool pawOn, bool catchHandlerWatching)
		{
			if (currentState != State.Caught && currentState != State.Happy)
			{
				SetState(pawOn ? State.Playing : catchHandlerWatching ? State.Sleeping : State.Playing);
			}
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
			currentState = state;
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
					SetSprite(caughtSprite1);
                    break;
				case State.Happy:
					SetSprite(happySprite);
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

        private void OnLoseHandlerAboutToShowJumpscareMom()
		{
			SetSprite(caughtSprite2);
			animator.PlayAnimationFromStart(SHAKE_ANIMATION_NAME);
		}

        private void OnMaskingTapeSetState(MaskingTape.State state)
		{
			if (state == MaskingTape.State.Empty)
			{
				SetState(State.Happy);
			}
		}
    }
}