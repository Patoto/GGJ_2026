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

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

        public virtual void SubscribeToInitializeEvents()
		{
			CatchHandler.onCatched += OnCatchHandlerCatched;
		}

        public virtual void UnsubscribeFromInitializeEvents()
		{
			CatchHandler.onCatched -= OnCatchHandlerCatched;
		}

        private void OnCatchHandlerCatched()
		{
			GameManager.instance.StartCoroutine(OnCatchHandlerCatchedCoroutine());
		}

        private IEnumerator OnCatchHandlerCatchedCoroutine()
		{
			jumpscareBackgroundCanvasGroup.DoToggleFadeAndInteractableTween(true, 0.1f);
			jumpscareMom.transform.DOScale(5f, 0.25f).SetEase(Ease.Linear);
			yield return new WaitForSeconds(0.2f);
			gameOverBackground.DOFade(1f, 0.25f);
			yield return new WaitForSeconds(2f);
			yield return GameManager.instance.transitionsManager.PlayTransitionOutCoroutine();
			GameManager.instance.scenesManager.ResetScene();
		}
    }
}