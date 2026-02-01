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
			if (state == MaskingTape.State.Empty)
			{
				GameManager.instance.StartCoroutine(WinCoroutine());
			}
		}

        private IEnumerator WinCoroutine()
		{
			gameObject.SetActive(true);
			yield return new WaitForSeconds(1.5f);
			yield return backgroundImage.DOFade(1f, 0.25f).WaitForCompletion();
			yield return new WaitForSeconds(3f);
			yield return GameManager.instance.transitionsManager.PlayTransitionOutCoroutine();
			GameManager.instance.scenesManager.ResetScene();
		}
    }
}