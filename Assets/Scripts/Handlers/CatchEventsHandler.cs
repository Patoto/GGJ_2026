using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ_2026
{
	public class CatchEventsHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private Transform catchEventsParent;
		
		private List<CatchEvent> catchEventsList = new();
		private bool invokedFirstCatchEvent;

        public void SubscribeToEnableEvents()
		{
			CatchEvent.onFinished += OnCatchEventFinished;
			Paw.onToggledCanReceiveInputs += OnPawToggledCanReceiveInputs;
		}

        public void UnsubscribeFromEnableEvents()
        {
			CatchEvent.onFinished -= OnCatchEventFinished;
			Paw.onToggledCanReceiveInputs -= OnPawToggledCanReceiveInputs;
        }

        protected override void Awake()
        {
            base.Awake();
			catchEventsList = catchEventsParent.gameObject.GetChildrenWithComponent<CatchEvent>();
        }

        private void InvokeNextCatchEvent()
		{
			InvokeActionAfterSeconds(StartRandomUnusedCatchEvent, UnityEngine.Random.Range(2.5f, 5f));
			invokedFirstCatchEvent = true;
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
			if (on && !invokedFirstCatchEvent)
			{
				InvokeNextCatchEvent();
			}
		}
    }
}