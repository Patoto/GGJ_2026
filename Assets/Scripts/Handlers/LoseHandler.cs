using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ_2026
{
    public class LoseHandler : MyMonoBehaviour, IEventSubscriberDeclarator
    {
		[SerializeField] private CanvasGroup jumpscareBackgroundCanvasGroup;
		[SerializeField] private Image jumpscareMom;
		[SerializeField] private Image gameOverBackground;
		[SerializeField] private Sound loseMusicSoundPrefab;
		[SerializeField] private Sound jumpscareSoundPrefab;

        public static Action onAboutToShowJumpscareMom;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

        public virtual void SubscribeToInitializeEvents()
		{
			CatchHandler.onCaught += OnCatchHandlerCaught;
		}

        public virtual void UnsubscribeFromInitializeEvents()
		{
			CatchHandler.onCaught -= OnCatchHandlerCaught;
		}

        private void OnCatchHandlerCaught()
		{
			if (this != null)
			{
				gameObject.SetActive(true);
				StartCoroutine(OnCatchHandlerCatchedCoroutine());
			}
		}

        private IEnumerator OnCatchHandlerCatchedCoroutine()
		{
			yield return new WaitForSeconds(1f);
			onAboutToShowJumpscareMom?.Invoke();
			jumpscareBackgroundCanvasGroup.DoToggleFadeAndInteractableTween(true, 0.25f);
			jumpscareMom.transform.DOScale(5f, 0.5f).SetEase(Ease.Linear);
			GameManager.instance.audioManager.PlaySound(jumpscareSoundPrefab);
			yield return new WaitForSeconds(0.4f);
			gameOverBackground.DOFade(1f, 0.25f);
			GameManager.instance.audioManager.PlayMusic(loseMusicSoundPrefab);
			yield return new WaitForSeconds(5f);
			GameManager.instance.audioManager.currentMusicSound.FadeOutAndDestroy();
			yield return GameManager.instance.transitionsManager.PlayTransitionOutCoroutine();
			GameManager.instance.scenesManager.ResetScene();
		}
    }
}