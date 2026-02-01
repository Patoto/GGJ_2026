using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ_2026
{
	public class WinHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private Image backgroundImage;
		[SerializeField] private Sound winMusicSoundPrefab;
		[SerializeField] private CanvasGroup levelCanvasGroup;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

        public virtual void SubscribeToInitializeEvents()
		{
			MaskingTape.onSetState += OnMaskingTapeSetState;
		}
		
        public virtual void UnsubscribeFromInitializeEvents()
		{
			MaskingTape.onSetState -= OnMaskingTapeSetState;
		}

        private void OnMaskingTapeSetState(MaskingTape.State state)
		{
			if (this != null && state == MaskingTape.State.Empty)
			{
				GameManager.instance.StartCoroutine(WinCoroutine());
			}
		}

        private IEnumerator WinCoroutine()
		{
			gameObject.SetActive(true);
			GameManager.instance.audioManager.currentMusicSound.Stop();
			yield return new WaitForSeconds(2f);
			GameManager.instance.audioManager.PlayMusic(winMusicSoundPrefab);
			levelCanvasGroup.alpha = 0;
			yield return new WaitForSeconds(5f);
			GameManager.instance.audioManager.currentMusicSound.FadeOutAndDestroy();
			yield return GameManager.instance.transitionsManager.PlayTransitionOutCoroutine();
			GameManager.instance.scenesManager.ResetScene();
		}
    }
}