using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GGJ_2026
{
	public class TimeHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private TextMeshProUGUI timeText;
		[SerializeField] private GameObject timeOutGameObject;

		private int currentSecondsRemaining = 120;

        public static Action onTimeFinished;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

        public virtual void SubscribeToInitializeEvents()
		{
			CatchEventsHandler.onAboutToInvokeFirstCatchEvent += OnCatchEventsHandlerAboutToInvokeFirstCatchEvent;
			LoseHandler.onAboutToShowJumpscareMom += OnLoseHandlerAboutToShowJumpscareMom;
			CatchHandler.onCaught += OnCatchHandlerCaught;
		}

        public virtual void UnsubscribeFromInitializeEvents()
		{
			CatchEventsHandler.onAboutToInvokeFirstCatchEvent -= OnCatchEventsHandlerAboutToInvokeFirstCatchEvent;
			LoseHandler.onAboutToShowJumpscareMom -= OnLoseHandlerAboutToShowJumpscareMom;
			CatchHandler.onCaught -= OnCatchHandlerCaught;
		}

        private void OnCatchHandlerCaught()
		{
			gameObject.SetActive(false);
			StopAllCoroutines();
		}

        private void Start()
        {
            UpdateText();
        }

        private void OnCatchEventsHandlerAboutToInvokeFirstCatchEvent()
        {
			gameObject.SetActive(true);
            StartCoroutine(UpdateTimeCoroutine());
        }

        private IEnumerator UpdateTimeCoroutine()
        {
            while (currentSecondsRemaining >= 0)
            {
                yield return new WaitForSeconds(1f);
                currentSecondsRemaining--;
                UpdateText();
            }
            OnTimeFinished();
        }

        private void OnTimeFinished()
        {
			timeOutGameObject.SetActive(true);
			gameObject.SetActive(false);
            onTimeFinished?.Invoke();
        }

        private void UpdateText()
		{
			timeText.text = currentSecondsRemaining.ToMinutesFormattedString();
		}

        private void OnLoseHandlerAboutToShowJumpscareMom()
		{
			timeOutGameObject.SetActive(false);
		}
    }
}