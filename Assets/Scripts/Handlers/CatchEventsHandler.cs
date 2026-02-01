using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ_2026
{
	public class CatchEventsHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private Transform catchEventsParent;
		
        private IEnumerator invokeStartRandomUnusedCatchEventCoroutine;
		private List<CatchEvent> catchEventsList = new();
        private bool invokedFirstCatchEvent;

        public static Action onAboutToInvokeFirstCatchEvent;

        public void SubscribeToEnableEvents()
		{
			CatchEvent.onFinished += OnCatchEventFinished;
			Paw.onToggledCanReceiveInputs += OnPawToggledCanReceiveInputs;
			LevelHandler.onFinishedTransitionInCoroutine += OnLevelHandlerFinishedTransitionInCoroutine;
			MaskingTape.onSetState += OnMaskingTapeSetState;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchEvent.onFinished -= OnCatchEventFinished;
			Paw.onToggledCanReceiveInputs -= OnPawToggledCanReceiveInputs;
			LevelHandler.onFinishedTransitionInCoroutine -= OnLevelHandlerFinishedTransitionInCoroutine;
			MaskingTape.onSetState -= OnMaskingTapeSetState;
        }

        protected override void Awake()
        {
            base.Awake();
			catchEventsList = catchEventsParent.gameObject.GetChildrenWithComponent<CatchEvent>();
        }

        private void InvokeNextCatchEvent()
        {
			if (!invokedFirstCatchEvent)
			{
				onAboutToInvokeFirstCatchEvent?.Invoke();
			}
            StopInvokeStartRandomUnusedCatchEventCoroutine();
            invokeStartRandomUnusedCatchEventCoroutine = InvokeActionAfterSeconds(StartRandomUnusedCatchEvent, UnityEngine.Random.Range(2.5f, 5f));
            invokedFirstCatchEvent = true;
        }

        private void StopInvokeStartRandomUnusedCatchEventCoroutine()
        {
            StopCoroutine(invokeStartRandomUnusedCatchEventCoroutine);
        }

        private void StartRandomUnusedCatchEvent()
		{
			catchEventsList.GetUnusedElement(Extensions.UnusedElementsListData<object>.GetOrderType.Random).StartCatchEvent();
		}

        private void OnCatchEventFinished()
		{
			InvokeNextCatchEvent();
		}

        private void OnPawToggledCanReceiveInputs(bool on)
		{
			if (on && GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession() && !invokedFirstCatchEvent)
			{
				InvokeNextCatchEvent();
			}
		}

        private void OnLevelHandlerFinishedTransitionInCoroutine()
		{
			if (!GameManager.instance.persistentDataManager.IsFirstTimePlayingALevelThisSession())
			{
				InvokeNextCatchEvent();
			}
		}

        private void OnMaskingTapeSetState(MaskingTape.State state)
		{
			if (state == MaskingTape.State.Empty)
			{
				StopInvokeStartRandomUnusedCatchEventCoroutine();
			}
		}
    }
}