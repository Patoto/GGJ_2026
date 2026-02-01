using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace GGJ_2026
{
	public class TimeHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private TextMeshProUGUI timeText;

		private int currentSecondsRemaining = 120;

        public static Action onTimeFinished;

        public void SubscribeToEnableEvents() { }

        public void UnsubscribeFromEnableEvents() { }

        public virtual void SubscribeToInitializeEvents()
		{
			CatchEventsHandler.onAboutToInvokeFirstCatchEvent += OnCatchEventsHandlerAboutToInvokeFirstCatchEvent;
		}

        public virtual void UnsubscribeFromInitializeEvents()
		{
			CatchEventsHandler.onAboutToInvokeFirstCatchEvent -= OnCatchEventsHandlerAboutToInvokeFirstCatchEvent;
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
			onTimeFinished?.Invoke();
		}

        private void UpdateText()
		{
			timeText.text = currentSecondsRemaining.ToMinutesFormattedString();
		}
    }
}